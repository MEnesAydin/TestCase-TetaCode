using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestCase.Domain.Grades;

namespace TestCase.Infrastructure.Configurations;

internal sealed class GradeConfiguration : IEntityTypeConfiguration<Grade>
{
    public void Configure(EntityTypeBuilder<Grade> builder)
    {
        builder.ToTable("Grades");
        builder.HasKey(x => x.Id);
    }
}