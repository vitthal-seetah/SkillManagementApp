using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SkillManager.Application.DTOs.User;
using SkillManager.Application.Interfaces.Repositories;
using SkillManager.Application.Interfaces.Repositories.m;
using SkillManager.Application.Interfaces.Services;
using SkillManager.Application.Services;
using SkillManager.Application.Validators;

namespace AppManagement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserSkillService, UserSkillService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ILevelService, LevelService>();
        services.AddScoped<ISkillService, SkillService>();

        // For single validator registration
        services.AddScoped<IValidator<CreateUserDto>, CreateUserValidator>();

        // For scanning an assembly to register all validators
        services.AddValidatorsFromAssemblyContaining<UpdateUserValidator>();
        services.AddValidatorsFromAssemblyContaining<CreateSkillValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateSkillValidator>();
        return services;
    }
}
