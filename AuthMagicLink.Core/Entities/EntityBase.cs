namespace AuthMagicLink.Core.Entities;

public abstract class EntityBase
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    protected EntityBase() { }
}