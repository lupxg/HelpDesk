namespace HelpDesk.Domain.Common;

public interface IHasKey<TKey>
{
    TKey Id { get; }
}
