using Fiap.CloudGames.Application.Orders.Dtos;
using FluentValidation;

namespace Fiap.CloudGames.Application.Orders.Validators;

public class CreateOrderDtoValidator : AbstractValidator<CreateOrderDto>
{
    public CreateOrderDtoValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("O usuário é obrigatório.");

        RuleFor(x => x.PaymentMethod)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("O método de pagamento é obrigatório.")
            .MaximumLength(40).WithMessage("O método de pagamento pode ter no máximo 40 caracteres.")
            .Must(v => v == null || v == v.Trim())
            .WithMessage("O método de pagamento não deve conter espaços no início ou no fim.");

        RuleFor(x => x.PaymentTransactionId)
            .MaximumLength(100).WithMessage("O identificador de transação pode ter no máximo 100 caracteres.")
            .Must(v => v == null || v == v.Trim())
            .WithMessage("O identificador de transação não deve conter espaços no início ou no fim.");

        RuleFor(x => x.Items)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Os itens do pedido são obrigatórios.")
            .NotEmpty().WithMessage("Informe ao menos um item no pedido.");

        RuleForEach(x => x.Items)
            .SetValidator(new CreateOrderItemDtoValidator());
    }
}
