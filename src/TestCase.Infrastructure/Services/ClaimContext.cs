using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using TestCase.Application.Services;

namespace TestCase.Infrastructure.Services;

internal sealed class ClaimContext(
    IHttpContextAccessor httpContextAccessor) : IClaimContext
{
    public Guid GetUserId()
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext is null)
        {
            throw new ArgumentNullException("context bilgisi bulunamadı");
        }
        var claims = httpContext.User.Claims;
        string? userId = claims.FirstOrDefault(i => i.Type == ClaimTypes.NameIdentifier)?.Value;
        if (userId is null)
        {
            throw new ArgumentNullException("Kullanıcı bilgisi bulunamadı");
        }
        try
        {
            Guid id = Guid.Parse(userId);
            return id;
        }
        catch (Exception)
        {
            throw new ArgumentException("Kullanıcı id uygun Guid formatında değil");
        }
    }
}