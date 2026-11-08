using HelpDesk.Domain.Common;

namespace HelpDesk.Domain.Entities;

public class NotificationOutbox : BaseEntity<Guid>
{
    public string Type { get; set; }
    public string Payload { get; set; }
    public DateTimeOffset ProcessedAtUtc { get; set; }
    public int Attempts { get; set; }
}
