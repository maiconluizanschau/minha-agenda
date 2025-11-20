using Agenda.Api.Application;
using Agenda.Api.Data;
using Agenda.Api.Models;
using AutoMapper;
using MediatR;

namespace Agenda.Api.Handlers;

public class ObterContatoHandler : IRequestHandler<ObterContatoQuery, ContatoDto?>
{
    private readonly IContatoRepository _repo;
    private readonly IMapper _mapper;

    public ObterContatoHandler(IContatoRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<ContatoDto?> Handle(ObterContatoQuery request, CancellationToken cancellationToken)
    {
        var contato = await _repo.ObterPorIdAsync(request.Id);
        return contato is null ? null : _mapper.Map<ContatoDto>(contato);
    }
}
