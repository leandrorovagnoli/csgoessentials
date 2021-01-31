namespace CsgoEssentials.Domain.Interfaces.Entities
{
    public interface IEntity<out TKey>
    {
        TKey Id { get; }
    }
}
