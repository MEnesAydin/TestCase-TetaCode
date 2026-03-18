using FluentValidation;
using MediateX;
using Microsoft.AspNetCore.Http;
using TestCase.Application.Common;
using TestCase.Application.Features.FileService;
using TestCase.Application.Services;
using TestCase.Domain.Features.GenericRepository;
using TestCase.Domain.Grades;

namespace TestCase.Application.Grades;

public sealed class GradeCreateCommand : IRequest<Result<string>>
{
    public string CourseName { get; set; } = default!;
    public string Description { get; set; } = default!;
    public IFormFile? File { get; set; }
}

public sealed class GradeCreateCommandValidator : AbstractValidator<GradeCreateCommand>
{
    public GradeCreateCommandValidator()
    {
        RuleFor(p => p.CourseName)
            .NotEmpty().WithMessage("Ders adı boş bırakılamaz.")
            .MaximumLength(200).WithMessage("Ders adı en fazla 200 karakter olabilir.");
        
        RuleFor(p => p.Description)
            .NotEmpty().WithMessage("Açıklama boş bırakılamaz.")
            .MaximumLength(1000).WithMessage("Açıklama en fazla 1000 karakter olabilir.");
    }
}

internal sealed class GradeCreateCommandHandler(
    IGradeRepository gradeRepository,
    IClaimContext claimContext,
    IUnitOfWork unitOfWork) : IRequestHandler<GradeCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(GradeCreateCommand request, CancellationToken cancellationToken)
    {
        var userId = claimContext.GetUserId();
        
        string? fileName = null;
        if (request.File is not null)
        {
            fileName = FileService.FileSaveToServer(request.File, "wwwroot/images/");
        }
        
        Grade grade = new()
        {
            CourseName = request.CourseName,
            Description = request.Description,
            FileName = fileName ?? string.Empty,
            UserId = userId
        };

        await gradeRepository.AddAsync(grade, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        string message = request.File is not null 
            ? "Not başarıyla oluşturuldu ve dosya yüklendi." 
            : "Not başarıyla oluşturuldu.";
        
        return Result<string>.Succeed(message);
    }
}