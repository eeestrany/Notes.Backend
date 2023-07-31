using Microsoft.AspNetCore.Mvc;
using Notes.Application.Notes.Queries.GetNoteList;
using Notes.Application.Notes.Queries.GetNoteDetails;
using Notes.WebApi.Models;
using AutoMapper;
using Notes.Application.Notes.Commands.CreateNote;
using Notes.Application.Notes.Commands.DeleteNote;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Notes.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : BaseController
    {
        private readonly IMapper _mapper;

        public NoteController(IMapper mapper, IMediator mediator) : base(mediator)
        {
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<NoteListViewModel>> GetAll()
        {
            var query = new GetNoteListQuery()
            {
                UserId = UserId
            };

            var vm = await Mediator.Send(query);
            return (Ok(vm));
        }

        [HttpGet("{id:guid}")]
        [Authorize]
        public async Task<ActionResult<NoteDetailsViewModel>> Get(Guid id)
        {
            var query = new GetNoteDetailsQuery()
            {
                UserId = UserId,
                Id = id
            };

            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateNoteDto note)
        {
            var command = _mapper.Map<CreateNoteCommand>(note);
            command.UserId = UserId;
            var noteId = await Mediator.Send(command);
            return (Ok(noteId));
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Updata([FromBody] UpdateNoteDto note)
        {
            var command = _mapper.Map<UpdateNoteDto>(note);
            command.UserId = UserId;
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteNoteCommand
            {
                Id = id,
                UserId = UserId
            };
            await Mediator.Send(command);
            return NoContent();
        }
    }
}