using MediatR;

namespace Notes.Application.Notes.Queries.GetNoteDetails
{
    public class GetNoteDetailsQuery : IRequest<NoteDetailsViewModel>
    {
        public Guid UserId { get; set; }
        public Guid Id { get; set; }
    }
}
