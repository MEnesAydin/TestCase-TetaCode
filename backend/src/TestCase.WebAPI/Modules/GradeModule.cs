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


        app.MapGet("/file/{id}",
                async (Guid id, ISender sender, IWebHostEnvironment env, CancellationToken cancellationToken) =>
                {
                    var res = await sender.Send(new GradeGetAllQuery(new GradeQueryParameters()), cancellationToken);
                    
                    if (!res.IsSuccessful || res.Data == null)
                        return Results.NotFound();

                    var grade = res.Data.Items.FirstOrDefault(g => g.Id == id);
                    if (grade == null || string.IsNullOrEmpty(grade.FileName))
                        return Results.NotFound();

                    var filePath = Path.Combine(env.WebRootPath, "images", grade.FileName);
                    
                    if (!File.Exists(filePath))
                        return Results.NotFound();

                    var fileBytes = await File.ReadAllBytesAsync(filePath, cancellationToken);
                    var fileName = grade.FileName;
                    
                    // Determine content type based on file extension
                    var extension = Path.GetExtension(fileName).ToLowerInvariant();
                    var contentType = extension switch
                    {
                        ".jpg" or ".jpeg" => "image/jpeg",
                        ".png" => "image/png",
                        ".gif" => "image/gif",
                        ".pdf" => "application/pdf",
                        ".doc" => "application/msword",
                        ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                        ".xls" => "application/vnd.ms-excel",
                        ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        _ => "application/octet-stream"
                    };

                    return Results.File(fileBytes, contentType, fileName);
                })
            .Produces(200)
            .Produces(404);
    }
}