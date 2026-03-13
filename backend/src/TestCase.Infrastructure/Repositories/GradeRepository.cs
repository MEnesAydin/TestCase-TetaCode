using TestCase.Domain.Features.GenericRepository;
using TestCase.Domain.Grades;
using TestCase.Infrastructure.Context;

namespace TestCase.Infrastructure.Repositories;

internal sealed class GradeRepository : Repository<Grade,ApplicationDbContext>,IGradeRepository
{
    public GradeRepository(ApplicationDbContext context) : base(context)
    {
        
    }
}