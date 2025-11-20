using System;
using System.Threading;
using System.Threading.Tasks;
using Agenda.Api.Application.Cqrs.Contacts.Commands;
using Agenda.Api.Application.Dtos;
using Agenda.Api.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace Agenda.Api.Application.Cqrs.Contacts.Handlers
{
    public class UpdateContactCommandHandler : IRequestHandler<UpdateContactCommand, ContactDto?>
    {
        private readonly IContactRepository _repository;
        private readonly IMapper _mapper;

        public UpdateContactCommandHandler(IContactRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ContactDto?> Handle(UpdateContactCommand request, CancellationToken cancellationToken)
        {
            var contact = await _repository.GetByIdAsync(request.Id);
            if (contact is null || !contact.Ativo) return null;

            contact.Nome = request.Dto.Nome;
            contact.Telefone = request.Dto.Telefone;
            contact.AtualizadoEm = DateTime.UtcNow;

            await _repository.UpdateAsync(contact);
            return _mapper.Map<ContactDto>(contact);
        }
    }
}
