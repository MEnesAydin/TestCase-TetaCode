using MediateX;
using Microsoft.AspNetCore.Mvc;
using TestCase.Application.Common;
using TestCase.Application.Grades;

namespace TestCase.WebAPI.Modules;

public static class GradeModule
{
    public static void MapGrade(this IEndpointRouteBuilder builder)
    {
        var app = builder
            .MapGroup("/grades")
            .RequireRateLimiting("fixed")
            .RequireAuthorization()
            .WithTags("Grades");

        app.MapGet("/getall",
                async ([AsParameters] GradeQueryParameters parameters,
                    ISender sender,
                    CancellationToken cancellationToken) =>
                {
                    var res = await sender.Send(new GradeGetAllQuery(parameters), cancellationToken);

                    return res.IsSuccessful ? Results.Ok(res) : Results.BadRequest(res);
                })
            .Produces<PagedResult<GradeGetAllQueryResponse>>();
            
        
        app.MapPost("/",
                async ([FromForm] GradeCreateCommand request, ISender sender, CancellationToken cancellationToken) =>
                {
                    var res = await sender.Send(request, cancellationToken);
                    return res.IsSuccessful ? Results.Ok(res) : Results.BadRequest(res);
                })
            .DisableAntiforgery()
            .Accepts<GradeCreateCommand>("multipart/form-data")
            .Produces<Result<string>>(200)
            .Produces<Result<string>>(400)
            .Produces(401);
        
        app.MapPut("/",
                async ([FromForm] GradeUpdateCommand request, ISender sender, CancellationToken cancellationToken) =>
                {
                    var res = await sender.Send(request, cancellationToken);
                    return res.IsSuccessful ? Results.Ok(res) : Results.BadRequest(res);
                })
            .DisableAntiforgery()
            .Accepts<GradeUpdateCommand>("multipart/form-data")
            .Produces<Result<string>>(200)
            .Produces<Result<string>>(400)
            .Produces(401);

        app.MapDelete("/delete/{id}",
                async (Guid id, ISender sender, CancellationToken cancellationToken) =>
                {
                    var res = await sender.Send(new GradeDeleteCommand(id), cancellationToken);
                    return res.IsSuccessful ? Results.Ok(res) : Results.BadRequest(res);
                })
            .Produces<Result<string>>(200)
            .Produces<Result<string>>(400);
    }
}
