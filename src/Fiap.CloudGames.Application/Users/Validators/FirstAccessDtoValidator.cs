using Fiap.CloudGames.Application.Users.Dtos;
using FluentValidation;

namespace Fiap.CloudGames.Application.Users.Validators;

public class FirstAccessDtoValidator : AbstractValidator<FirstAccessDto>
{
    public FirstAccessDtoValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("O token é obrigatório.");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("A senha é obrigatória.")
            .MinimumLength(8).WithMessage("A senha deve ter pelo menos 8 caracteres.")
            .Matches("^(?=.*[A-Za-z])(?=.*\\d)(?=.*[^A-Za-z0-9]).+$")
            .WithMessage("A senha deve conter letras, números e caracteres especiais.");
    }
}
