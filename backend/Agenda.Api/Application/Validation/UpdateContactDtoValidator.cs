using Agenda.Api.Application.Dtos;
using FluentValidation;

namespace Agenda.Api.Application.Validation
{
    public class UpdateContactDtoValidator : AbstractValidator<UpdateContactDto>
    {
        public UpdateContactDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.Nome)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(100);

            RuleFor(x => x.Telefone)
                .NotEmpty()
                .Matches(@"^\d{10,11}$");
        }
    }
}
