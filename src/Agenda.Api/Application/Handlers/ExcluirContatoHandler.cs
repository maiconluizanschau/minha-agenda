using Agenda.Api.Application;
using Agenda.Api.Data;
using MediatR;

namespace Agenda.Api.Handlers;

public class ExcluirContatoHandler : IRequestHandler<ExcluirContatoCommand, Unit>
{
    private readonly IContatoRepository _repo;

    public ExcluirContatoHandler(IContatoRepository repo)
    {
        _repo = repo;
    }

    public async Task<Unit> Handle(ExcluirContatoCommand request, CancellationToken cancellationToken)
    {
        var contato = await _repo.ObterPorIdAsync(request.Id)
            ?? throw new KeyNotFoundException("Contato não encontrado.");

        await _repo.RemoverAsync(contato);
        await _repo.SaveChangesAsync();

        return Unit.Value;
    }
}
