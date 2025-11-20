using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Agenda.Api.Application.Cqrs.Contacts.Queries;
using Agenda.Api.Application.Dtos;
using Agenda.Api.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace Agenda.Api.Application.Cqrs.Contacts.Handlers
{
    public class GetAllContactsQueryHandler : IRequestHandler<GetAllContactsQuery, IEnumerable<ContactDto>>
    {
        private readonly IContactRepository _repository;
        private readonly IMapper _mapper;

        public GetAllContactsQueryHandler(IContactRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ContactDto>> Handle(GetAllContactsQuery request, CancellationToken cancellationToken)
        {
            var contacts = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<ContactDto>>(contacts);
        }
    }
}
