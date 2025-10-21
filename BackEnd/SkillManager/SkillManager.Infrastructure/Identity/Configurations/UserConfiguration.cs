using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillManager.Domain.Enums;
using SkillManager.Infrastructure.Identity.Models;

namespace SkillManager.Infrastructure.Identity.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        var hasher = new PasswordHasher<ApplicationUser>();

        builder.HasData(
            new ApplicationUser
            {
                Id = "a4950d3d-ca05-40ab-b8ff-7791c173ba98",
                Email = "admin@localhost.com",
                NormalizedEmail = "ADMIN@LOCALHOST.COM",
                FirstName = "System",
                LastName = "Admin",
                UserName = "admin@localhost.com",
                NormalizedUserName = "ADMIN@LOCALHOST.COM",
                PasswordHash = hasher.HashPassword(null, "P@sser123"),
                EmailConfirmed = true,
                UTCode = "UT001",
                RefId = "HR001",
                RoleId = 1, // Admin
                Status = UserStatus.Active,
                DeliveryType = DeliveryType.Onshore,
            },
            new ApplicationUser
            {
                Id = "a6146e7c-febf-4fbb-83ab-97fccabb044c",
                Email = "user1@localhost.com",
                NormalizedEmail = "USER1@LOCALHOST.COM",
                FirstName = "System",
                LastName = "User",
                UserName = "user1@localhost.com",
                NormalizedUserName = "USER1@LOCALHOST.COM",
                PasswordHash = hasher.HashPassword(null, "P@sser123"),
                EmailConfirmed = true,
                UTCode = "UT002",
                RefId = "HR002",
                RoleId = 5, // Employee
                Status = UserStatus.Active,
                DeliveryType = DeliveryType.Onshore,
            },
            new ApplicationUser
            {
                Id = "8310a350-45e3-4b03-82d6-3120d3edad80",
                Email = "leader@localhost.com",
                NormalizedEmail = "LEADER@LOCALHOST.COM",
                FirstName = "System",
                LastName = "Leader",
                UserName = "leader@localhost.com",
                NormalizedUserName = "LEADER@LOCALHOST.COM",
                PasswordHash = hasher.HashPassword(null, "P@sser123"),
                EmailConfirmed = true,
                UTCode = "UT003",
                RefId = "HR003",
                RoleId = 2, // Tech Lead
                Status = UserStatus.Active,
                DeliveryType = DeliveryType.Onshore,
            }
        );
    }
}
