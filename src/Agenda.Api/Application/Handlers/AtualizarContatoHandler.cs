using Agenda.Api.Application;
using Agenda.Api.Data;
using Agenda.Api.Models;
using AutoMapper;
using MediatR;

namespace Agenda.Api.Handlers;

public class AtualizarContatoHandler : IRequestHandler<AtualizarContatoCommand, ContatoDto>
{
    private readonly IContatoRepository _repo;
    private readonly IMapper _mapper;

    public AtualizarContatoHandler(IContatoRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<ContatoDto> Handle(AtualizarContatoCommand request, CancellationToken cancellationToken)
    {
        var contato = await _repo.ObterPorIdAsync(request.Id)
            ?? throw new KeyNotFoundException("Contato não encontrado.");

        if (await _repo.EmailExisteAsync(request.Contato.Email, request.Id))
        {
            throw new ApplicationException("Já existe um contato cadastrado com este e-mail.");
        }

        contato.Atualizar(
            request.Contato.Nome,
            request.Contato.Email,
            request.Contato.Telefone,
            request.Contato.Observacoes
        );

        await _repo.AtualizarAsync(contato);
        await _repo.SaveChangesAsync();

        return _mapper.Map<ContatoDto>(contato);
    }
}
