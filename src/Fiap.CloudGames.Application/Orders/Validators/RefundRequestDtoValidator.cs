using Fiap.CloudGames.Application.Orders.Dtos;
using FluentValidation;

namespace Fiap.CloudGames.Application.Orders.Validators;

public class RefundRequestDtoValidator : AbstractValidator<RefundRequestDto>
{
    public RefundRequestDtoValidator()
    {
        RuleFor(x => x.Reason)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("O motivo do estorno é obrigatório.")
            .MaximumLength(500).WithMessage("O motivo do estorno pode ter no máximo 500 caracteres.")
            .Must(v => v == null || v == v.Trim())
            .WithMessage("O motivo do estorno não deve conter espaços no início ou no fim.");
    }
}
