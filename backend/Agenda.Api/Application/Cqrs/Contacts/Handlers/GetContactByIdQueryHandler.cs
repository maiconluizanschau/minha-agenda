using System.Threading;
using System.Threading.Tasks;
using Agenda.Api.Application.Cqrs.Contacts.Queries;
using Agenda.Api.Application.Dtos;
using Agenda.Api.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace Agenda.Api.Application.Cqrs.Contacts.Handlers
{
    public class GetContactByIdQueryHandler : IRequestHandler<GetContactByIdQuery, ContactDto?>
    {
        private readonly IContactRepository _repository;
        private readonly IMapper _mapper;

        public GetContactByIdQueryHandler(IContactRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ContactDto?> Handle(GetContactByIdQuery request, CancellationToken cancellationToken)
        {
            var contact = await _repository.GetByIdAsync(request.Id);
            if (contact is null || !contact.Ativo) return null;
            return _mapper.Map<ContactDto>(contact);
        }
    }
}
