using MediateX;
using Microsoft.EntityFrameworkCore;
using TestCase.Application.Common;
using TestCase.Domain.Features.GenericRepository;
using TestCase.Domain.Grades;

namespace TestCase.Application.Grades;

public sealed record GradeDeleteCommand(
    Guid Id) : IRequest<Result<string>>;

internal sealed class GradeDeleteCommandHandler(
    IGradeRepository gradesRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<GradeDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(GradeDeleteCommand request, CancellationToken cancellationToken)
    {
        var grade = await gradesRepository.FirstOrDefaultAsync(g => g.Id == request.Id, cancellationToken);
        if (grade is null)
        {
            return Result<string>.Failure("Not bulunamadı!");
        }

        if (grade.IsDeleted == false)
        {
            grade.IsDeleted = true;
            gradesRepository.Update(grade);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return "Not arşive taşındı";
        }
        else
        {
            gradesRepository.Delete(grade);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return "Not kalıcı olarak silindi";
        }
    }
}