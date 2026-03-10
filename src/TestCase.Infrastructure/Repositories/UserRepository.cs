using TestCase.Domain.Features.GenericRepository;
using TestCase.Domain.Users;
using TestCase.Infrastructure.Context;

namespace TestCase.Infrastructure.Repositories;

internal sealed class UserRepository : Repository<User,ApplicationDbContext>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }
    
}