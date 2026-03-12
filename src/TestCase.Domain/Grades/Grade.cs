using TestCase.Domain.Abstractions;
using TestCase.Domain.Users;

namespace TestCase.Domain.Grades;

public sealed class Grade : Entity
{
    public string CourseName { get; set; } = default!;
    public string Description { get; set; } = default!;
    public FileTypeEnum FileType { get; set; } = default!;
    public string FileName { get; set; } = default!;
    public string FilePath { get; set; } = default!;

    public Guid UserId { get; set; }
    public User? User { get; set; }
}