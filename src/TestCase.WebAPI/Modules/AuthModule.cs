using MediateX;
using TestCase.Application.Auth;
using TestCase.Application.Common;

namespace TestCase.WebAPI.Modules;

public static class AuthModule
{
    public static void MapAuth(this IEndpointRouteBuilder builder)
    {
        var app = builder.MapGroup("/auth").WithTags("Auth");

        app.MapPost("/login",
                async (LoginCommand request, ISender sender, CancellationToken cancellationToken) =>
                {
                    var res = await sender.Send(request, cancellationToken);
                    return res.IsSuccessful ? Results.Ok(res) : Results.InternalServerError(res);
                })
            .Produces<Result<LoginCommandResponse>>()
            .RequireRateLimiting("login-fixed");
    }
}