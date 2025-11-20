using System;
using System.Threading;
using System.Threading.Tasks;
using Agenda.Api.Application.Cqrs.Contacts.Commands;
using Agenda.Api.Domain.Interfaces;
using MediatR;

namespace Agenda.Api.Application.Cqrs.Contacts.Handlers
{
    public class DeleteContactCommandHandler : IRequestHandler<DeleteContactCommand, bool>
    {
        private readonly IContactRepository _repository;

        public DeleteContactCommandHandler(IContactRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteContactCommand request, CancellationToken cancellationToken)
        {
            var contact = await _repository.GetByIdAsync(request.Id);
            if (contact is null || !contact.Ativo) return false;

            contact.Ativo = false;
            contact.AtualizadoEm = DateTime.UtcNow;
            await _repository.UpdateAsync(contact);
            return true;
        }
    }
}
