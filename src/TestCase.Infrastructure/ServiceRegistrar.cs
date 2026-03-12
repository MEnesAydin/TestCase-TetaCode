using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestCase.Application.Services;
using TestCase.Domain.Users;
using TestCase.Infrastructure.Context;
using TestCase.Infrastructure.Repositories;
using TestCase.Infrastructure.Services;


namespace TestCase.Infrastructure;

public static class ServiceRegistrar
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(opt =>
        {
            string con = configuration.GetConnectionString("mssql")!;
            opt.UseSqlServer(con);
        });

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IJwtProvider, JwtProvider>();
        
        return services;
    }
}