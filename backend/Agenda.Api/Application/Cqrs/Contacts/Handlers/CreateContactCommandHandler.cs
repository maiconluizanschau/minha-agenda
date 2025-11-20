using System;
using System.Threading;
using System.Threading.Tasks;
using Agenda.Api.Application.Cqrs.Contacts.Commands;
using Agenda.Api.Application.Cqrs.Contacts.Events;
using Agenda.Api.Application.Dtos;
using Agenda.Api.Domain.Entities;
using Agenda.Api.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace Agenda.Api.Application.Cqrs.Contacts.Handlers
{
    public class CreateContactCommandHandler : IRequestHandler<CreateContactCommand, ContactDto>
    {
        private readonly IContactRepository _repository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public CreateContactCommandHandler(IContactRepository repository, IMapper mapper, IMediator mediator)
        {
            _repository = repository;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<ContactDto> Handle(CreateContactCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            var existing = await _repository.GetByEmailAsync(dto.Email);
            if (existing is not null && existing.Ativo)
                throw new InvalidOperationException("Já existe um contato ativo com esse e-mail.");

            var entity = _mapper.Map<Contact>(dto);
            await _repository.AddAsync(entity);

            var resultDto = _mapper.Map<ContactDto>(entity);

            // Publica evento de domínio para mensageria
            await _mediator.Publish(new ContactCreatedEvent(resultDto), cancellationToken);

            return resultDto;
        }
    }
}
