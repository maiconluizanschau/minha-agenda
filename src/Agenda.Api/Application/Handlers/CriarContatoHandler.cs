using Agenda.Api.Application;
using Agenda.Api.Data;
using Agenda.Api.Domain;
using Agenda.Api.Models;
using AutoMapper;
using MediatR;

namespace Agenda.Api.Handlers;

public class CriarContatoHandler : IRequestHandler<CriarContatoCommand, ContatoDto>
{
    private readonly IContatoRepository _repo;
    private readonly IMapper _mapper;

    public CriarContatoHandler(IContatoRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<ContatoDto> Handle(CriarContatoCommand request, CancellationToken cancellationToken)
    {
        if (await _repo.EmailExisteAsync(request.Contato.Email))
        {
            throw new ApplicationException("Já existe um contato cadastrado com este e-mail.");
        }

        var entity = new Contato(
            request.Contato.Nome,
            request.Contato.Email,
            request.Contato.Telefone,
            request.Contato.Observacoes
        );

        await _repo.AdicionarAsync(entity);
        await _repo.SaveChangesAsync();

        return _mapper.Map<ContatoDto>(entity);
    }
}
