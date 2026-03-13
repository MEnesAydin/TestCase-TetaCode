using Microsoft.AspNetCore.Diagnostics;
using TestCase.Application.Common;

namespace TestCase.WebAPI;

public sealed class ExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        Result<string> errorResult;

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = 500;

        var actualException = exception is AggregateException agg && agg.InnerException != null
            ? agg.InnerException
            : exception;

        var exceptionType = actualException.GetType();
        
        errorResult = Result<string>.Failure("Bir hata oluştu. Lütfen daha sonra tekrar deneyin.");

        await httpContext.Response.WriteAsJsonAsync(errorResult);

        return true;
    }
}