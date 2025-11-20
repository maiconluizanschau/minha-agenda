using Agenda.Api.Application.Cqrs.Contacts.Commands;
using Agenda.Api.Application.Cqrs.Contacts.Queries;
using Agenda.Api.Application.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Agenda.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "AdminOnly")]
    public class ContactsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContactsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactDto>>> GetAll()
        {
            var result = await _mediator.Send(new GetAllContactsQuery());
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ContactDto>> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetContactByIdQuery(id));
            if (result is null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ContactDto>> Create([FromBody] CreateContactDto dto)
        {
            var created = await _mediator.Send(new CreateContactCommand(dto));
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ContactDto>> Update(Guid id, [FromBody] UpdateContactDto dto)
        {
            dto.Id = id;
            var updated = await _mediator.Send(new UpdateContactCommand(id, dto));
            if (updated is null) return NotFound();
            return Ok(updated);
        }

        [HttpPatch("{id:guid}/favorite")]
        public async Task<ActionResult<ContactDto>> ToggleFavorite(Guid id)
        {
            var updated = await _mediator.Send(new ToggleFavoriteContactCommand(id));
            if (updated is null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _mediator.Send(new DeleteContactCommand(id));
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
