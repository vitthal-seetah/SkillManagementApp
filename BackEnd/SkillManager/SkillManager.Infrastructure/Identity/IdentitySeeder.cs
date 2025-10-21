using Microsoft.AspNetCore.Identity;
using SkillManager.Application.Abstractions.Identity;
using SkillManager.Infrastructure.Identity.Models;

namespace SkillManager.Infrastructure.Identity;

public static class IdentitySeeder
{
    public static async Task SeedAsync(
        RoleManager<IdentityRole> roleManager,
        UserManager<ApplicationUser> userManager
    )
    {
        await SeedRolesAsync(roleManager);
        await SeedAdminAsync(userManager);
    }

    static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        string[] roleNames =
        {
            RoleName.Admin,
            RoleName.Manager,
            RoleName.Leader,
            RoleName.SME,
            RoleName.User,
        };

        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
                await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    static async Task SeedAdminAsync(UserManager<ApplicationUser> userManager)
    {
        var adminEmail = "admin@skillmanager.com";
        var adminPassword = "Admin123!";

        var existingAdmin = await userManager.FindByEmailAsync(adminEmail);

        if (existingAdmin == null)
        {
            var adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                FirstName = "System",
                LastName = "Admin",
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);

            if (result.Succeeded)
                await userManager.AddToRoleAsync(adminUser, RoleName.Admin);
        }
    }
}
