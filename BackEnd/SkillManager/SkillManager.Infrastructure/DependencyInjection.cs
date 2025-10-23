using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SkillManager.Application.Abstractions.Identity;
using SkillManager.Infrastructure.Abstractions.Identity;
using SkillManager.Infrastructure.Abstractions.Repository;
using SkillManager.Infrastructure.Identity;
using SkillManager.Infrastructure.Identity.DbContext;
using SkillManager.Infrastructure.Identity.Models;
using SkillManager.Infrastructure.Identity.Services;
using SkillManager.Infrastructure.Identity.Settings;
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
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();

        return services;
    }

    private static IServiceCollection AddIdentityServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // DbContext
        services.AddDbContext<ApplicationIdentityDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        // Identity
        services
            .AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
            .AddDefaultTokenProviders();

        // JWT Settings
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        var jwtSettings = configuration.GetSection("JwtSettings");
        var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidateAudience = true,
                    ValidAudience = jwtSettings["Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                };
            });

        return services;
    }

    // --------------------------
    // Run this AFTER building the app to safely seed
    // --------------------------
    public static async Task SeedIdentityAsync(this IServiceProvider serviceProvider)
    {
        //  await db.Database.MigrateAsync(); // Apply migrations first

        //  await IdentitySeeder.SeedAsync(roleManager, userManager); // Safe seeding
    }
}
