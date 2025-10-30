using Fiap.CloudGames.Application.Orders.Dtos;
using FluentValidation;

namespace Fiap.CloudGames.Application.Orders.Validators;

public class CreateOrderItemDtoValidator : AbstractValidator<CreateOrderItemDto>
{
    public CreateOrderItemDtoValidator()
    {
        RuleFor(x => x.GameId)
            .NotEmpty().WithMessage("O jogo é obrigatório.");

        RuleFor(x => x.Quantity)
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0).WithMessage("A quantidade deve ser maior que zero.")
            .LessThanOrEqualTo(50).WithMessage("A quantidade máxima por item é 50.");

        RuleFor(x => x.UnitPrice)
            .GreaterThan(0m).WithMessage("O preço unitário deve ser maior que zero.");
    }
}
