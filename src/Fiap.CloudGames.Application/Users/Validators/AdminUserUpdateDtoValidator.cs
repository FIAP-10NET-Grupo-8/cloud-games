using Fiap.CloudGames.Application.Users.Dtos;
using FluentValidation;

namespace Fiap.CloudGames.Application.Users.Validators;

public class AdminUserUpdateDtoValidator : AbstractValidator<AdminUserUpdateDto>
{
	public AdminUserUpdateDtoValidator()
	{
		RuleFor(x => x.Id).NotEmpty().WithMessage("Id do usuário é obrigatório.");

		When(x => x.Name != null, () =>
		{
			RuleFor(x => x.Name)
				.NotEmpty().WithMessage("O nome não pode estar vazio.")
				.MinimumLength(2).WithMessage("O nome deve ter ao menos 2 caracteres.");
		});

		When(x => x.Email != null, () =>
		{
			RuleFor(x => x.Email)
				.NotEmpty().WithMessage("O email não pode estar vazio.")
				.EmailAddress().WithMessage("O email deve ter formato válido.");
		});

		When(x => x.Role.HasValue, () =>
		{
			RuleFor(x => x.Role).IsInEnum().WithMessage("Role inválido.");
		});

		When(x => x.Status.HasValue, () =>
		{
			RuleFor(x => x.Status).IsInEnum().WithMessage("Status inválido.");
		});
	}
}
