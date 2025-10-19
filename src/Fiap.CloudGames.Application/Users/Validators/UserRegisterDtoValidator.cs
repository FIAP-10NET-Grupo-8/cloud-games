using Fiap.CloudGames.Application.Users.Dtos;
using FluentValidation;

namespace Fiap.CloudGames.Application.Users.Validators;

public class UserRegisterDtoValidator : AbstractValidator<UserRegisterDto>
{
	public UserRegisterDtoValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty().WithMessage("O nome é obrigatório.")
			.MinimumLength(2).WithMessage("O nome deve ter ao menos 2 caracteres.");

		RuleFor(x => x.Email)
			.NotEmpty().WithMessage("O email é obrigatório.")
			.EmailAddress().WithMessage("O email deve ter formato válido.");

		RuleFor(x => x.Password)
			.NotEmpty().WithMessage("A senha é obrigatória.")
			.MinimumLength(8).WithMessage("A senha deve ter pelo menos 8 caracteres.")
			.Matches("^(?=.*[A-Za-z])(?=.*\\d)(?=.*[^A-Za-z0-9]).+$")
			.WithMessage("A senha deve conter letras, números e caracteres especiais.");
	}
}
