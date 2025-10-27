using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillManager.Domain.Entities;
using SkillManager.Domain.Entities.Enums;

namespace SkillManager.Infrastructure.Identity.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasIndex(u => u.UtCode).IsUnique();
        builder.HasIndex(u => u.RefId).IsUnique();
        builder.HasIndex(u => u.Eid).IsUnique();

        builder.Property(u => u.Status).HasConversion<string>();
        builder.Property(u => u.DeliveryType).HasConversion<string>();

        builder.Property(u => u.FirstName).HasMaxLength(100);
        builder.Property(u => u.LastName).HasMaxLength(100);
        builder.Property(u => u.UtCode).HasMaxLength(50);
        builder.Property(u => u.RefId).HasMaxLength(100);
        builder.Property(u => u.Domain).HasMaxLength(100);
        builder.Property(u => u.Eid).HasMaxLength(50);

        builder
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(
            new User
            {
                UserId = 2,
                FirstName = "Vitthal",
                LastName = "Seetah",
                UtCode = "UT002",
                Domain = "DIR",
                Eid = "vithal.seetah",
                RefId = "Rf00",
                RoleId = 1,
                DeliveryType = DeliveryType.Onshore,
                Status = UserStatus.Active,
            },
            new User
            {
                UserId = 1,
                FirstName = "Girish",
                LastName = "Jagroop",
                UtCode = "UT003",
                Domain = "DIR",
                Eid = "girish.s.jagroop",
                RefId = "Rf00",
                RoleId = 1,
                DeliveryType = DeliveryType.Onshore,
                Status = UserStatus.Active,
            }
        );
    }
}
