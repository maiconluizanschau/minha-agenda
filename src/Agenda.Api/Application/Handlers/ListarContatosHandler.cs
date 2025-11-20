using Agenda.Api.Application;
using Agenda.Api.Data;
using Agenda.Api.Models;
using AutoMapper;
using MediatR;

namespace Agenda.Api.Handlers;

public class ListarContatosHandler : IRequestHandler<ListarContatosQuery, IReadOnlyList<ContatoDto>>
{
    private readonly IContatoRepository _repo;
    private readonly IMapper _mapper;

    public ListarContatosHandler(IContatoRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<ContatoDto>> Handle(ListarContatosQuery request, CancellationToken cancellationToken)
    {
        var contatos = await _repo.ListarAsync(request.Filtro);
        return _mapper.Map<IReadOnlyList<ContatoDto>>(contatos);
    }
}
