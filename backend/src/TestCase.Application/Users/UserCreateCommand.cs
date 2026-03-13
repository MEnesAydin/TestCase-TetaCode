using FluentValidation;
using MediateX;
using TestCase.Application.Common;
using TestCase.Domain.Features.GenericRepository;
using TestCase.Domain.Users;
using TestCase.Domain.Users.ValueObjects;

namespace TestCase.Application.Users;

public sealed record UserCreateCommand(
    string UserName,
    string Password) : IRequest<Result<string>>;

public sealed class UserCreateCommandValidator : AbstractValidator<UserCreateCommand>
{
    public UserCreateCommandValidator()
    {
        RuleFor(p => p.UserName).NotEmpty().WithMessage("Geçerli bir kullanıcı adı girin");
        RuleFor(p => p.Password)
            .NotEmpty().WithMessage("Şifre alanı boş bırakılamaz.")
            .MinimumLength(6).WithMessage("Şifre en az 6 karakter uzunluğunda olmalıdır.");
    }
}

internal sealed class UserCreateCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UserCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UserCreateCommand request, CancellationToken cancellationToken)
    {
        var userNameExists = await userRepository.AnyAsync(p => p.UserName == request.UserName, cancellationToken);
        if (userNameExists)
        {
            return Result<string>.Failure("Bu kullanıcı adı daha önce kullanılmış");
        }
        
        User user = new User
        {
            UserName = request.UserName,
            Password = new Password(request.Password),
        };


        userRepository.Add(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Kullanıcı başarıyla oluşturuldu";
    }
}