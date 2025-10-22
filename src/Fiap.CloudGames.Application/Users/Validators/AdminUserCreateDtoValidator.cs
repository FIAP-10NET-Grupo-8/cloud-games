using Fiap.CloudGames.Application.Users.Dtos;
using FluentValidation;

namespace Fiap.CloudGames.Application.Users.Validators;

public class AdminUserCreateDtoValidator : AbstractValidator<AdminUserCreateDto>
{
	public AdminUserCreateDtoValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty().WithMessage("O nome é obrigatório.")
			.MinimumLength(2).WithMessage("O nome deve ter ao menos 2 caracteres.");

		RuleFor(x => x.Email)
			.NotEmpty().WithMessage("O email é obrigatório.")
			.EmailAddress().WithMessage("O email deve ter formato válido.");

		RuleFor(x => x.Role)
			.IsInEnum().WithMessage("Role inválido.");
	}
}
