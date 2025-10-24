using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillManager.Domain.Entities;

namespace SkillManager.Infrastructure.Identity.AppDbContext.Configurations;

public class UserSkillConfiguration : IEntityTypeConfiguration<UserSkill>
{
    public void Configure(EntityTypeBuilder<UserSkill> builder)
    {
        builder.ToTable("UserSkills");

        builder.HasKey(us => new { us.UserId, us.SkillId });

        builder
            .HasOne(us => us.User)
            .WithMany(u => u.UserSkills)
            .HasForeignKey(us => us.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(us => us.Skill)
            .WithMany(s => s.UserSkills)
            .HasForeignKey(us => us.SkillId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(us => us.Level)
            .WithMany(l => l.UserSkills)
            .HasForeignKey(us => us.LevelId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(us => us.UserId);
        builder.HasIndex(us => us.SkillId);
    }
}
