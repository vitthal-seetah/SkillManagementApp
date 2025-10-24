using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillManager.Domain.Entities;

namespace SkillManager.Infrastructure.Identity.AppDbContext.Configurations;

public class UserSMEConfiguration : IEntityTypeConfiguration<UserSME>
{
    public void Configure(EntityTypeBuilder<UserSME> builder)
    {
        builder.ToTable("UserSMEs");

        builder.HasKey(us => new
        {
            us.UserId,
            us.SkillId,
            us.CategoryTypeId,
        });

        builder
            .HasOne(us => us.User)
            .WithMany(u => u.UserSMEs)
            .HasForeignKey(us => us.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(us => us.Skill)
            .WithMany(s => s.UserSMEs)
            .HasForeignKey(us => us.SkillId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(us => us.CategoryType)
            .WithMany()
            .HasForeignKey(us => us.CategoryTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(us => us.UserId);
        builder.HasIndex(us => us.SkillId);
    }
}
