using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestCase.Application.Services;
using TestCase.Domain.Features.GenericRepository;
using TestCase.Domain.Users;
using TestCase.Infrastructure.Context;
using TestCase.Infrastructure.Options;
using TestCase.Infrastructure.Repositories;
using TestCase.Infrastructure.Services;


namespace TestCase.Infrastructure;

public static class ServiceRegistrar
{
    
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
        services.ConfigureOptions<JwtSetupOptions>();
        services.AddAuthentication().AddJwtBearer();
        services.AddAuthorization();
        services.AddDbContext<ApplicationDbContext>(opt =>
        {
            string con = configuration.GetConnectionString("mssql")!;
            opt.UseSqlServer(con);
        });

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<IUnitOfWork>(srv => srv.GetRequiredService<ApplicationDbContext>());
        
        return services;
    }
}