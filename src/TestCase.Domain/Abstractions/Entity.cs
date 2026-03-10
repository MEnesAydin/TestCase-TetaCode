namespace TestCase.Domain.Abstractions;

public abstract class Entity
{
    protected Entity()
    {
        Id = Guid.CreateVersion7();
    }
    
    public Guid Id { get; protected set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}