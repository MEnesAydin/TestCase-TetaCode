using FluentValidation;
using MediateX;
using TestCase.Application.Common;
using TestCase.Application.Services;
using TestCase.Domain.Features.GenericRepository;
using TestCase.Domain.Users;

namespace TestCase.Application.Auth;

public sealed record LoginCommand(
    string Username,
    string Password) : IRequest<Result<LoginCommandResponse>>;

public sealed record LoginCommandResponse
{
    public string? Token { get; set; }
}

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(p => p.Username).NotEmpty().WithMessage("Geçerli bir mail ya da kullanıcı adı girin");
        RuleFor(p => p.Password).NotEmpty().WithMessage("Geçerli bir şifre girin");
    }
}

public sealed class LoginCommandHandler(
    IUserRepository userRepository,
    //IUnitOfWork unitOfWork,
    IJwtProvider jwtProvider) : IRequestHandler<LoginCommand, Result<LoginCommandResponse>>
{
    public async Task<Result<LoginCommandResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FirstOrDefaultAsync(p =>
            p.UserName == request.Username);

        if (user is null)
        {
            return Result<LoginCommandResponse>.Failure("Kullanıcı adı ya da şifre yanlış");
        }

        var checkPassword = user.VerifyPasswordHash(request.Password);

        if (!checkPassword)
        {
            return Result<LoginCommandResponse>.Failure("Kullanıcı adı ya da şifre yanlış");
        }
        var token = await jwtProvider.CreateLoginToken(user, cancellationToken);
        var res = new LoginCommandResponse()
        {
            Token = token,
        };
        return res;
    }
}