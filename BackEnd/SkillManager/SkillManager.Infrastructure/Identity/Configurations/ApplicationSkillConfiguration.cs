using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillManager.Domain.Entities;

namespace SkillManager.Infrastructure.Identity.AppDbContext.Configurations
{
    public class ApplicationSkillConfiguration : IEntityTypeConfiguration<ApplicationSkill>
    {
        public void Configure(EntityTypeBuilder<ApplicationSkill> builder)
        {
            builder.ToTable("ApplicationSkills");

            builder.HasKey(a => new { a.ApplicationId, a.SkillId });

            builder
                .HasOne(a => a.Application)
                .WithMany(app => app.ApplicationSkills)
                .HasForeignKey(a => a.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(a => a.Skill)
                .WithMany(s => s.ApplicationSkills)
                .HasForeignKey(a => a.SkillId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(a => a.ApplicationId);
            builder.HasIndex(a => a.SkillId);
        }
    }
}
