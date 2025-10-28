using Fiap.CloudGames.Application.Orders.Dtos;
using FluentValidation;

namespace Fiap.CloudGames.Application.Orders.Validators;

public class CreateOrderDtoValidator : AbstractValidator<CreateOrderDto>
{
	public CreateOrderDtoValidator()
	{
		RuleFor(x => x.PlayerId)
			.NotEmpty().WithMessage("O Id do jogador é obrigatório.");

		RuleFor(x => x.TotalValue)
			.GreaterThanOrEqualTo(0).WithMessage("O valor total do pedido não pode ser negativo.");

		RuleFor(x => x.PaymentTransactionId)
			.NotEmpty().WithMessage("O Id da transação de pagamento é obrigatório.")
			.MaximumLength(255).WithMessage("O Id da transação de pagamento não pode exceder 255 caracteres.");
	}
}
