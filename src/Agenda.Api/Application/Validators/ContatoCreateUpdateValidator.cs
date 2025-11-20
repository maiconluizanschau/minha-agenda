using Agenda.Api.Models;
using FluentValidation;

namespace Agenda.Api.Application.Validators;

public class ContatoCreateUpdateValidator : AbstractValidator<ContatoCreateUpdateDto>
{
    public ContatoCreateUpdateValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome é obrigatório.")
            .MaximumLength(150);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email é obrigatório.")
            .EmailAddress().WithMessage("Email inválido.")
            .MaximumLength(200);

        RuleFor(x => x.Telefone)
            .NotEmpty().WithMessage("Telefone é obrigatório.")
            .MaximumLength(20);

        RuleFor(x => x.Observacoes)
            .MaximumLength(500);
    }
}
