using Fiap.CloudGames.Application.Carts.Dtos;
using FluentValidation;

namespace Fiap.CloudGames.Application.Carts.Validators;

public sealed class AddCartItemDtoValidator : AbstractValidator<AddCartItemDto>
{
    public AddCartItemDtoValidator()
    {
        RuleFor(x => x.GameId)
            .NotEmpty().WithMessage("O identificador do jogo é obrigatório.");
    }
}
