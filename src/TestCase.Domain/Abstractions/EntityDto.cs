namespace TestCase.Domain.Abstractions;

public abstract class EntityDto
{
    public Guid Id { get; set; } = default!;
    public bool IsDeleted { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}