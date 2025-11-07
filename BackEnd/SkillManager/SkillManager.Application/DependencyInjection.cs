using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SkillManager.Application.DTOs.Category;
using SkillManager.Application.DTOs.SubCategory;
using SkillManager.Application.DTOs.User;
using SkillManager.Application.Interfaces.Repositories;
using SkillManager.Application.Interfaces.Repositories.m;
using SkillManager.Application.Interfaces.Services;
using SkillManager.Application.Services;
using SkillManager.Application.Validators;
using SkillManager.Application.Validators.Category;

namespace AppManagement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserSkillService, UserSkillService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ILevelService, LevelService>();
        services.AddScoped<ISkillService, SkillService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<ITeamService, TeamService>();
        // For scanning an assembly to register all validators
        services.AddValidatorsFromAssemblyContaining<CreateSkillValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateSkillValidator>();

        // For single validator registration
        services.AddScoped<IValidator<CreateUserDto>, CreateUserValidator>();
        services.AddScoped<IValidator<UpdateUserDto>, UpdateUserValidator>();
        services.AddScoped<IValidator<CreateCategoryDto>, CreateCategoryValidator>();
        services.AddScoped<IValidator<UpdateCategoryDto>, UpdateCategoryValidator>();
        services.AddScoped<IValidator<CreateSubCategoryDto>, CreateSubCategoryValidator>();
        services.AddScoped<IValidator<UpdateSubCategoryDto>, UpdateSubCategoryValidator>();
        services.AddScoped<IValidator<CreateCategoryTypeDto>, CreateCategoryTypeValidator>();
        services.AddScoped<IValidator<UpdateCategoryTypeDto>, UpdateCategoryTypeValidator>();
        return services;
    }
}
