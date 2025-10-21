using Microsoft.Extensions.DependencyInjection;
using SkillManager.Infrastructure.Abstractions.Services;
using SkillManager.Infrastructure.Services;

namespace AppManagement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserSkillService, UserSkillService>();
        return services;
    }
}
