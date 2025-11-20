using System;
using System.Threading;
using System.Threading.Tasks;
using Agenda.Api.Application.Cqrs.Contacts.Commands;
using Agenda.Api.Application.Cqrs.Contacts.Events;
using Agenda.Api.Application.Dtos;
using Agenda.Api.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace Agenda.Api.Application.Cqrs.Contacts.Handlers
{
    public class ToggleFavoriteContactCommandHandler : IRequestHandler<ToggleFavoriteContactCommand, ContactDto?>
    {
        private readonly IContactRepository _repository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public ToggleFavoriteContactCommandHandler(IContactRepository repository, IMapper mapper, IMediator mediator)
        {
            _repository = repository;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<ContactDto?> Handle(ToggleFavoriteContactCommand request, CancellationToken cancellationToken)
        {
            var contact = await _repository.GetByIdAsync(request.Id);
            if (contact is null || !contact.Ativo) return null;

            contact.Favorito = !contact.Favorito;
            contact.AtualizadoEm = DateTime.UtcNow;

            await _repository.UpdateAsync(contact);

            var dto = _mapper.Map<ContactDto>(contact);

            // dispara evento (pode ser usado para logging, RabbitMQ, etc.)
            await _mediator.Publish(new ContactFavoritedEvent(dto), cancellationToken);

            return dto;
        }
    }
}
