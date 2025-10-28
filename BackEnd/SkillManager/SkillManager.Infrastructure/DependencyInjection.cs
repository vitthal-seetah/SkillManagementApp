using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SkillManager.Application.Interfaces.Repositories;
using SkillManager.Infrastructure.Identity.AppDbContext;
using SkillManager.Infrastructure.Persistence.Repositories;
using SkillManager.Infrastructure.Repositories;

namespace AppManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // --------------------------
        // Identity & JWT
        // --------------------------
        services.AddIdentityServices(configuration);

        // --------------------------
        // Repositories & Services
        // --------------------------
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserSkillRepository, UserSkillRepository>();
        services.AddScoped<IClaimsTransformation, RoleClaimsTransformer>();
        services.AddScoped<ILevelRepository, LevelRepository>();

        return services;
    }

    private static IServiceCollection AddIdentityServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // DbContext
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        return services;
    }
}
