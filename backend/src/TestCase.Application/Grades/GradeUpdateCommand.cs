using FluentValidation;
using MediateX;
using Microsoft.AspNetCore.Http;
using TestCase.Application.Common;
using TestCase.Application.Features.FileService;
using TestCase.Application.Services;
using TestCase.Domain.Features.GenericRepository;
using TestCase.Domain.Grades;

namespace TestCase.Application.Grades;

public sealed record GradeUpdateCommand(
    Guid Id,
    string CourseName,
    string Description,
    IFormFile? File) : IRequest<Result<string>>;

public sealed class GradeUpdateCommandValidator : AbstractValidator<GradeUpdateCommand>
{
    public GradeUpdateCommandValidator()
    {
        RuleFor(p => p.Id)
            .NotEmpty().WithMessage("Grade ID boş bırakılamaz.");
        
        RuleFor(p => p.CourseName)
            .NotEmpty().WithMessage("Ders adı boş bırakılamaz.")
            .MaximumLength(200).WithMessage("Ders adı en fazla 200 karakter olabilir.");
        
        RuleFor(p => p.Description)
            .NotEmpty().WithMessage("Açıklama boş bırakılamaz.")
            .MaximumLength(1000).WithMessage("Açıklama en fazla 1000 karakter olabilir.");
    }
}

internal sealed class GradeUpdateCommandHandler(
    IGradeRepository gradeRepository,
    IClaimContext claimContext,
    IUnitOfWork unitOfWork) : IRequestHandler<GradeUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(GradeUpdateCommand request, CancellationToken cancellationToken)
    {
        var userId = claimContext.GetUserId();
        
        var grade = await gradeRepository.GetByExpressionWithTrackingAsync(
            g => g.Id == request.Id && g.UserId == userId,
            cancellationToken);
        
        if (grade is null)
        {
            return Result<string>.Failure("Not bulunamadı veya bu nota erişim yetkiniz yok.");
        }
        
        // Update basic properties
        grade.CourseName = request.CourseName;
        grade.Description = request.Description;
        
        // Update file if provided
        if (request.File is not null)
        {
            // Delete old file if exists
            if (!string.IsNullOrEmpty(grade.FileName))
            {
                FileService.FileDeleteToServer(grade.FileName);
            }
            
            // Save new file
            string fileName = FileService.FileSaveToServer(request.File, "wwwroot/images");
            grade.FileName = fileName;
            grade.FileType = DetermineFileType(fileName);
        }
        
        gradeRepository.Update(grade);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result<string>.Succeed("Not başarıyla güncellendi.");
    }
    
    private FileTypeEnum DetermineFileType(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return FileTypeEnum.Other;
        
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        
        return extension switch
        {
            ".pdf" => FileTypeEnum.Pdf,
            ".doc" or ".docx" => FileTypeEnum.Word,
            ".xls" or ".xlsx" => FileTypeEnum.Excel,
            ".jpg" or ".jpeg" or ".png" or ".gif" or ".bmp" or ".webp" => FileTypeEnum.Image,
            _ => FileTypeEnum.Other
        };
    }
}
