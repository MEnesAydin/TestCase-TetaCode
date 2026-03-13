using MediateX;
using Microsoft.EntityFrameworkCore;
using TestCase.Application.Common;
using TestCase.Application.Services;
using TestCase.Domain.Abstractions;
using TestCase.Domain.Grades;

namespace TestCase.Application.Grades;

public sealed record GradeGetAllQuery(GradeQueryParameters Parameters)
    : IRequest<PagedResult<GradeGetAllQueryResponse>>;

public sealed class GradeGetAllQueryResponse : EntityDto
{
    public string CourseName { get; set; } = default!;
    public string Description { get; set; } = default!;
    public FileTypeEnum FileType { get; set; } = default!;
    public string FileName { get; set; } = default!;
}

internal sealed class GradeGetAllQueryHandler(
    IGradeRepository gradeRepository,
    IClaimContext claimContext)
    : IRequestHandler<GradeGetAllQuery, PagedResult<GradeGetAllQueryResponse>>
{
    public async Task<PagedResult<GradeGetAllQueryResponse>> Handle(GradeGetAllQuery request,
        CancellationToken cancellationToken)
    {
        var userId = claimContext.GetUserId();
        
        var gradeQuery = gradeRepository.GetAll()
            .Where(g => g.UserId == userId);

        // IsDeleted parametresi varsa filtrele
        if (request.Parameters.IsDeleted.HasValue)
        {
            gradeQuery = gradeQuery.Where(g => g.IsDeleted == request.Parameters.IsDeleted.Value);
        }

        var query = gradeQuery.Select(entity => new GradeGetAllQueryResponse
            {
                Id = entity.Id,
                CourseName = entity.CourseName,
                Description = entity.Description,
                FileType = entity.FileType,
                FileName = entity.FileName,
                IsDeleted = entity.IsDeleted,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                DeletedAt = entity.DeletedAt,
            })
            .OrderBy(p => p.Id);
        
        var pagedResult = await query.ToPagedResultAsync(
            request.Parameters,
            cancellationToken);

        return pagedResult;
    }
}