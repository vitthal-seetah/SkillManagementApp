using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillManager.Application.Abstractions.Identity;

namespace SkillManager.Infrastructure.Identity.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.HasData(
            new IdentityRole
            {
                Id = "310e3593-d06a-4617-887b-2f9153edea09",
                Name = RoleName.User,
                NormalizedName = RoleName.User.ToUpper(),
            },
            new IdentityRole
            {
                Id = "4a0e59b4-9c1e-4536-b43f-119d13556b8e",
                Name = RoleName.Admin,
                NormalizedName = RoleName.Admin.ToUpper(),
            },
            new IdentityRole
            {
                Id = "4c31900d-90f7-43a2-beec-c2bf0af83dea",
                Name = RoleName.Leader,
                NormalizedName = RoleName.Leader.ToUpper(),
            }
        );
    }
}
