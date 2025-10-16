using Microsoft.Extensions.DependencyInjection;
using SkillManager.Application.Abstractions.Services;
using SkillManager.Application.Services;

namespace AppManagement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserSkillService, UserSkillService>();
        return services;
    }
}
