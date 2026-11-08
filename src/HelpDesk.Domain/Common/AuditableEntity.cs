namespace HelpDesk.Domain.Common;

public  class AuditableEntity
{
    public DateTimeOffset CreatedAtUtc { get; private set; }

    public DateTimeOffset? UpdatedAtUtc { get; private set; }

    protected AuditableEntity() { }

    protected void MarkCreated(DateTimeOffset now)
    {
        CreatedAtUtc = now;
    }

    protected void MarkUpdated(DateTimeOffset now)
    {
        UpdatedAtUtc = now;
    }
}
