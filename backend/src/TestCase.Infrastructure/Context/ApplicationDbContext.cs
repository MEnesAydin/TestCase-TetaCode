using Microsoft.EntityFrameworkCore;
using TestCase.Domain.Abstractions;
using TestCase.Domain.Features.GenericRepository;

namespace TestCase.Infrastructure.Context;

internal sealed class ApplicationDbContext : DbContext,IUnitOfWork
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        modelBuilder.ApplyGlobalFilters();
        base.OnModelCreating(modelBuilder);
    }
    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<Entity>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property(p => p.CreatedAt)
                    .CurrentValue = DateTimeOffset.UtcNow;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Property(p => p.UpdatedAt)
                    .CurrentValue = DateTimeOffset.UtcNow;
            }
            
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}