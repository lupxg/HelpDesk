using HelpDesk.Domain.Common;

namespace HelpDesk.Domain.Entities;

public class TicketComment : BaseEntity<Guid>
{
    public Guid TicketId { get; private set; }
    public Guid AuthorId { get; private set; }
    public string Body { get; private set; } = string.Empty;

    // EF
    private TicketComment() { }

    public TicketComment(Guid ticketId, Guid authorId, string body) : base(Guid.NewGuid())
    {
        if(ticketId == Guid.Empty)
            throw new ArgumentException("Ticket ID cannot be empty.", nameof(ticketId));

        if (authorId == Guid.Empty)
            throw new ArgumentException("Author ID cannot be empty.", nameof(authorId));

        if (string.IsNullOrWhiteSpace(body))
            throw new ArgumentException("Comment body cannot be empty.", nameof(body));

        TicketId = ticketId;
        AuthorId = authorId;
        Body = body.Trim();
    }
}
