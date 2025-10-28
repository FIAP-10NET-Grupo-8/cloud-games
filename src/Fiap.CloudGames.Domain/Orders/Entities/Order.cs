using Fiap.CloudGames.Domain.Orders.Enums;

namespace Fiap.CloudGames.Domain.Orders.Entities;

/// <summary>
/// Entidade que vai representar a tabela 'order' (pedido) no banco.
/// </summary>
public class Order
{
	public Guid Id { get; private set; }
	public Guid PlayerId { get; private set; }
	public DateTime PurchaseDate { get; private set; } = DateTime.UtcNow;
	public decimal TotalValue { get; private set; }
	public OrderStatus Status { get; private set; } = OrderStatus.Pending;
	public bool RefundRequested { get; private set; } = false;
	public string? RefundReason { get; private set; }
	public DateTime? RefundRequestDate { get; private set; }
	public DateTime? RefundDate { get; private set; }
	public string? PaymentTransactionId { get; private set; }
	public bool PaymentConfirmed { get; private set; }

	public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
	public DateTime? UpdatedAt { get; private set; }

	private Order() { }

	private Order(
		Guid id,
		Guid playerId,
		DateTime purchaseDate,
		decimal totalValue,
		OrderStatus status,
		bool refundRequested,
		string? refundReason,
		DateTime? refundRequestDate,
		DateTime? refundDate,
		string? paymentTransactionId,
		bool paymentConfirmed,
		DateTime createdAt,
		DateTime? updatedAt)
	{
		Id = id;
		PlayerId = playerId;
		PurchaseDate = purchaseDate;
		TotalValue = totalValue;
		Status = status;
		RefundRequested = refundRequested;
		RefundReason = refundReason;
		RefundRequestDate = refundRequestDate;
		RefundDate = refundDate;
		PaymentTransactionId = paymentTransactionId;
		PaymentConfirmed = paymentConfirmed;
		CreatedAt = createdAt;
		UpdatedAt = updatedAt;
	}

	/// <summary>
	/// Factory Method to create a new Order with validation and default values.
	/// </summary>
	/// <param name="playerId"></param>
	/// <param name="totalValue"></param>
	/// <param name="paymentTransactionId"></param>
	/// <returns></returns>
	/// <exception cref="ArgumentException"></exception>
	public static Order Create(
		Guid playerId,
		decimal totalValue,
		string paymentTransactionId)
	{
		if (totalValue < 0)
			throw new ArgumentException("Valor total do pedido não pode ser negativo.", nameof(totalValue));

		if (string.IsNullOrEmpty(paymentTransactionId))
			throw new ArgumentException("ID da transação de pagamento não pode ser vazio.", nameof(paymentTransactionId));

		return new Order(
			id: Guid.NewGuid(),
			playerId: playerId,
			purchaseDate: DateTime.UtcNow,
			totalValue: totalValue,
			status: OrderStatus.Completed,
			refundRequested: false,
			refundReason: null,
			refundRequestDate: null,
			refundDate: null,
			paymentTransactionId: paymentTransactionId,
			paymentConfirmed: true,
			createdAt: DateTime.UtcNow,
			updatedAt: null);
	}

	/// <summary>
	/// Method to request a refund for the order.
	/// </summary>
	/// <param name="reason">(Opcional) Motivo para o reembolso</param>
	/// <exception cref="InvalidOperationException"></exception>
	public void Refund(string? reason)
	{
		if (RefundRequested)
			throw new InvalidOperationException("Reembolso já foi solicitado para este pedido.");

		if ((DateTime.UtcNow - PurchaseDate).Days > 30)
			throw new InvalidOperationException("Pedido tem mais de 1 mês e não pode ser estornado.");

		RefundRequested = true;
		RefundReason = reason;
		RefundRequestDate = DateTime.UtcNow;
		Status = OrderStatus.Refunded;
		UpdatedAt = DateTime.UtcNow;
	}
}
