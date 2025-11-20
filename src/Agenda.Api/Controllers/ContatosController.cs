using Agenda.Api.Application;
using Agenda.Api.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Agenda.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ContatosController : ControllerBase
{
    private readonly IMediator _mediator;

    public ContatosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ContatoDto>>> Get([FromQuery] string? filtro)
    {
        var contatos = await _mediator.Send(new ListarContatosQuery(filtro));
        return Ok(contatos);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ContatoDto>> GetById(Guid id)
    {
        var contato = await _mediator.Send(new ObterContatoQuery(id));
        if (contato is null)
            return NotFound();

        return Ok(contato);
    }

    [HttpPost]
    public async Task<ActionResult<ContatoDto>> Post([FromBody] ContatoCreateUpdateDto dto)
    {
        var created = await _mediator.Send(new CriarContatoCommand(dto));
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ContatoDto>> Put(Guid id, [FromBody] ContatoCreateUpdateDto dto)
    {
        var updated = await _mediator.Send(new AtualizarContatoCommand(id, dto));
        return Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new ExcluirContatoCommand(id));
        return NoContent();
    }
}
