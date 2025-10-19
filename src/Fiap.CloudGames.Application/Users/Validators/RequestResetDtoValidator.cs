using Fiap.CloudGames.Application.Users.Dtos;
using FluentValidation;

namespace Fiap.CloudGames.Application.Users.Validators;

public class RequestResetDtoValidator : AbstractValidator<RequestResetDto>
{
	public RequestResetDtoValidator()
	{
		RuleFor(x => x.Email)
			.NotEmpty().WithMessage("O email é obrigatório.")
			.EmailAddress().WithMessage("O email deve ter formato válido.");
	}
}
