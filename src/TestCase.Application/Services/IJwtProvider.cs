using TestCase.Domain.Users;

namespace TestCase.Application.Services;

public interface IJwtProvider
{
    Task<string> CreateLoginToken(User user, CancellationToken cancellationToken = default);
}