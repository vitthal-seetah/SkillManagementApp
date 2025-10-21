using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SkillManager.Infrastructure.Identity.Configurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
    {
        builder.HasData(
            new IdentityUserRole<string>
            {
                RoleId = "4a0e59b4-9c1e-4536-b43f-119d13556b8e", // Admin
                UserId = "a4950d3d-ca05-40ab-b8ff-7791c173ba98",
            },
            new IdentityUserRole<string>
            {
                RoleId = "4c31900d-90f7-43a2-beec-c2bf0af83dea", // Tech Lead
                UserId = "8310a350-45e3-4b03-82d6-3120d3edad80",
            },
            new IdentityUserRole<string>
            {
                RoleId = "310e3593-d06a-4617-887b-2f9153edea09", // Employee
                UserId = "a6146e7c-febf-4fbb-83ab-97fccabb044c",
            }
        );
    }
}
