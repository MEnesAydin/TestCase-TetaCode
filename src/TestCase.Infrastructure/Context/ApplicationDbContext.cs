using Microsoft.EntityFrameworkCore;
using TestCase.Domain.Features.GenericRepository;

namespace TestCase.Infrastructure.Context;

internal sealed class ApplicationDbContext : DbContext,IUnitOfWork
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }
}