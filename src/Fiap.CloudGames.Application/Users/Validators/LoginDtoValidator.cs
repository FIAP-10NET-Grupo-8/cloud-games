using Fiap.CloudGames.Application.Users.Dtos;
using FluentValidation;

namespace Fiap.CloudGames.Application.Users.Validators;

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
	public LoginDtoValidator()
	{
		RuleFor(x => x.Email)
			.NotEmpty().WithMessage("Email é obrigatório.")
			.EmailAddress().WithMessage("Email inválido.");

        RuleFor(x => x.Password).NotEmpty().WithMessage("A senha é obrigatória.");
	}
}
