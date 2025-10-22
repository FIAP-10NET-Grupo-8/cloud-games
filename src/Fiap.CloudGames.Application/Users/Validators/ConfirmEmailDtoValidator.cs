using Fiap.CloudGames.Application.Users.Dtos;
using FluentValidation;

namespace Fiap.CloudGames.Application.Users.Validators;

public class ConfirmEmailDtoValidator : AbstractValidator<ConfirmEmailDto>
{
	public ConfirmEmailDtoValidator()
	{
		RuleFor(x => x.Token).NotEmpty().WithMessage("O token de confirmação é obrigatório.");
	}
}
