using Fiap.CloudGames.Application.Orders.Dtos;
using FluentValidation;

namespace Fiap.CloudGames.Application.Orders.Validators;

public class RefundRequestDtoValidator : AbstractValidator<RefundRequestDto>
{
	public RefundRequestDtoValidator()
	{
		RuleFor(x => x.OrderId)
			.NotEmpty().WithMessage("O Id do pedido é obrigatório.");

		RuleFor(x => x.Reason)
			.MaximumLength(1000).WithMessage("O motivo do reembolso não pode exceder 1000 caracteres.");
	}
}
