namespace HelpDesk.Domain.Common;

public abstract class BaseEntity<TKey> : AuditableEntity, IHasKey<TKey> where TKey : notnull
{
    public TKey Id { get; protected set; } = default!;

    protected BaseEntity() { }

    protected BaseEntity(TKey id) { Id = id; }
}
