using Fiap.CloudGames.Application.Games.Dtos;
using FluentValidation;

namespace Fiap.CloudGames.Application.Games.Validators;

public class CreateGameDtoValidator : AbstractValidator<CreateGameDto>
{
    public CreateGameDtoValidator()
    {
        RuleFor(x => x.Title)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("O título é obrigatório.")
            .MaximumLength(255).WithMessage("O título pode ter no máximo 255 caracteres.")
            .Must(v => v == null || v == v.Trim())
            .WithMessage("O título não deve conter espaços no início ou no fim.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("A descrição pode ter no máximo 1000 caracteres.")
            .Must(v => v == null || v == v.Trim())
            .WithMessage("A descrição não deve conter espaços no início ou no fim.");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).WithMessage("O preço não pode ser negativo.");

        RuleFor(x => x.ReleaseDate)
            .NotEmpty().WithMessage("A data de lançamento é obrigatória.");

        RuleFor(x => x.Developer)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("O desenvolvedor é obrigatório.")
            .MaximumLength(100).WithMessage("O desenvolvedor pode ter no máximo 100 caracteres.")
            .Must(v => v == null || v == v.Trim())
            .WithMessage("O desenvolvedor não deve conter espaços no início ou no fim.");

        RuleFor(x => x.Publisher)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("A publicadora é obrigatória.")
            .MaximumLength(100).WithMessage("A publicadora pode ter no máximo 100 caracteres.")
            .Must(v => v == null || v == v.Trim())
            .WithMessage("A publicadora não deve conter espaços no início ou no fim.");

        RuleFor(x => x.Genre)
            .MaximumLength(100).WithMessage("O gênero pode ter no máximo 100 caracteres.")
            .Must(v => v == null || v == v.Trim())
            .WithMessage("O gênero não deve conter espaços no início ou no fim.");

        RuleFor(x => x.Platforms)
            .MaximumLength(200).WithMessage("As plataformas podem ter no máximo 200 caracteres.")
            .Must(v => v == null || v == v.Trim())
            .WithMessage("As plataformas não devem conter espaços no início ou no fim.");
    }
}