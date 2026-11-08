using HelpDesk.Domain.Common;

namespace HelpDesk.Domain.Entities;

public class TicketAttachment : BaseEntity<Guid>
{
    public Guid TicketId { get; set; }
    public string StorageKey { get; set; }
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public long SizeInBytes { get; set; }
}
