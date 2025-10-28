using Microsoft.Extensions.DependencyInjection;
using SkillManager.Application.Interfaces.Repositories.m;
using SkillManager.Application.Interfaces.Services;
using SkillManager.Application.Services;

namespace AppManagement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserSkillService, UserSkillService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ILevelService, LevelService>();

        return services;
    }
}
