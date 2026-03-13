using MediateX;
using TestCase.Application.Common;
using TestCase.Application.Users;

namespace TestCase.WebAPI.Modules;

public static class UserModule
{
    public static void MapUser(this IEndpointRouteBuilder builder)
    {

        var app = builder
            .MapGroup("/users")
            .RequireRateLimiting("fixed")
            .RequireAuthorization()
            .WithTags("Users");

        app.MapPost("/register",
            async (UserCreateCommand request, ISender sender, CancellationToken cancellationToken) =>
            {
                var res = await sender.Send(request, cancellationToken);
                return res.IsSuccessful ? Results.Ok(res) : Results.InternalServerError(res);
            })
            .AllowAnonymous()
            .Produces<Result<string>>();
    }
}