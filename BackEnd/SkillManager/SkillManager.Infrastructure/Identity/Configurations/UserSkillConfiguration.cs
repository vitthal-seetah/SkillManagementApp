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

        // Seed data
        builder.HasData(
            new UserSkill
            {
                UserId = 2,
                SkillId = 26,
                LevelId = 1,
            },
            new UserSkill
            {
                UserId = 2,
                SkillId = 15,
                LevelId = 2,
            },
            new UserSkill
            {
                UserId = 2,
                SkillId = 17,
                LevelId = 2,
            },
            new UserSkill
            {
                UserId = 2,
                SkillId = 18,
                LevelId = 2,
            },
            new UserSkill
            {
                UserId = 2,
                SkillId = 21,
                LevelId = 2,
            },
            new UserSkill
            {
                UserId = 2,
                SkillId = 23,
                LevelId = 2,
            },
            new UserSkill
            {
                UserId = 2,
                SkillId = 28,
                LevelId = 2,
            },
            new UserSkill
            {
                UserId = 2,
                SkillId = 29,
                LevelId = 2,
            },
            new UserSkill
            {
                UserId = 2,
                SkillId = 1,
                LevelId = 3,
            },
            new UserSkill
            {
                UserId = 2,
                SkillId = 2,
                LevelId = 3,
            },
            new UserSkill
            {
                UserId = 2,
                SkillId = 3,
                LevelId = 3,
            },
            new UserSkill
            {
                UserId = 2,
                SkillId = 16,
                LevelId = 3,
            },
            new UserSkill
            {
                UserId = 2,
                SkillId = 19,
                LevelId = 3,
            },
            new UserSkill
            {
                UserId = 2,
                SkillId = 22,
                LevelId = 3,
            },
            new UserSkill
            {
                UserId = 2,
                SkillId = 24,
                LevelId = 3,
            },
            new UserSkill
            {
                UserId = 2,
                SkillId = 20,
                LevelId = 4,
            },
            new UserSkill
            {
                UserId = 2,
                SkillId = 25,
                LevelId = 4,
            },
            new UserSkill
            {
                UserId = 2,
                SkillId = 27,
                LevelId = 4,
            },
            new UserSkill
            {
                UserId = 1,
                SkillId = 26,
                LevelId = 1,
            },
            new UserSkill
            {
                UserId = 1,
                SkillId = 15,
                LevelId = 2,
            },
            new UserSkill
            {
                UserId = 1,
                SkillId = 17,
                LevelId = 2,
            },
            new UserSkill
            {
                UserId = 1,
                SkillId = 18,
                LevelId = 2,
            },
            new UserSkill
            {
                UserId = 1,
                SkillId = 21,
                LevelId = 2,
            },
            new UserSkill
            {
                UserId = 1,
                SkillId = 23,
                LevelId = 2,
            },
            new UserSkill
            {
                UserId = 1,
                SkillId = 28,
                LevelId = 2,
            },
            new UserSkill
            {
                UserId = 1,
                SkillId = 29,
                LevelId = 2,
            },
            new UserSkill
            {
                UserId = 1,
                SkillId = 1,
                LevelId = 3,
            },
            new UserSkill
            {
                UserId = 1,
                SkillId = 2,
                LevelId = 3,
            },
            new UserSkill
            {
                UserId = 1,
                SkillId = 3,
                LevelId = 3,
            },
            new UserSkill
            {
                UserId = 1,
                SkillId = 16,
                LevelId = 3,
            },
            new UserSkill
            {
                UserId = 1,
                SkillId = 19,
                LevelId = 3,
            },
            new UserSkill
            {
                UserId = 1,
                SkillId = 22,
                LevelId = 3,
            },
            new UserSkill
            {
                UserId = 1,
                SkillId = 24,
                LevelId = 3,
            },
            new UserSkill
            {
                UserId = 1,
                SkillId = 20,
                LevelId = 4,
            },
            new UserSkill
            {
                UserId = 1,
                SkillId = 25,
                LevelId = 4,
            },
            new UserSkill
            {
                UserId = 1,
                SkillId = 27,
                LevelId = 4,
            }
        );
    }
}
