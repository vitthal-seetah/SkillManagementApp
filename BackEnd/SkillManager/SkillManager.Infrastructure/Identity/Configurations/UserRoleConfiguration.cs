using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillManager.Domain.Entities;

namespace SkillManager.Infrastructure.Identity.Configurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.HasIndex(r => r.Name).IsUnique();
        builder.Property(r => r.Name).HasMaxLength(50);

        builder.HasData(
            new UserRole { RoleId = 1, Name = "SuperAdmin" },
            new UserRole { RoleId = 2, Name = "Admin" },
            new UserRole { RoleId = 3, Name = "Manager" },
            new UserRole { RoleId = 4, Name = "TeamLead" },
            new UserRole { RoleId = 5, Name = "Employee" }
        );
    }
}
