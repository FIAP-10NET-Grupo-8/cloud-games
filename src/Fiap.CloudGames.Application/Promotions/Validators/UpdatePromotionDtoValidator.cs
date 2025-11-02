using FluentValidation;
using Fiap.CloudGames.Application.Promotions.Dtos;

namespace Fiap.CloudGames.Application.Promotions.Validators;

public class UpdatePromotionDtoValidator : AbstractValidator<UpdatePromotionDto>
{
	public UpdatePromotionDtoValidator()
	{
		RuleFor(x => x.Name)
			.Cascade(CascadeMode.Stop)
			.NotEmpty().WithMessage("O nome é obrigatório.")
			.MaximumLength(200).WithMessage("O nome pode ter no máximo200 caracteres.")
			.Must(v => v == null || v == v.Trim()).WithMessage("O nome não deve conter espaços no início ou no fim.");

		RuleFor(x => x.StartDate)
			.NotEmpty().WithMessage("A data de início é obrigatória.");

		RuleFor(x => x.EndDate)
			.NotEmpty().WithMessage("A data de término é obrigatória.")
			.Must((dto, end) => end > dto.StartDate).WithMessage("A data de término deve ser posterior à data de início.");

		RuleFor(x => x.Discount)
			.GreaterThan(0).WithMessage("O percentual de desconto deve ser maior que0.")
			.LessThanOrEqualTo(100).WithMessage("O percentual de desconto deve ser menor ou igual a100.");

		RuleFor(x => x.ElligibleGames)
			.NotNull().WithMessage("Os jogos aplicáveis são obrigatórios.");

		When(x => x.Status.HasValue, () =>
		{
			RuleFor(x => x.Status).IsInEnum().WithMessage("Status inválido.");
		});
	}
}
