using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AppEntity = SkillManager.Domain.Entities.Application;

namespace SkillManager.Infrastructure.Identity.AppDbContext.Configurations
{
    public class ApplicationConfiguration : IEntityTypeConfiguration<AppEntity>
    {
        public void Configure(EntityTypeBuilder<AppEntity> builder)
        {
            builder.ToTable("Applications");

            builder.HasKey(a => a.ApplicationId);

            builder.Property(a => a.Name).IsRequired().HasMaxLength(200);

            builder
                .HasOne(a => a.ApplicationSuite)
                .WithMany(s => s.Applications)
                .HasForeignKey(a => a.SuiteId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(a => a.Category)
                .WithMany(c => c.Applications)
                .HasForeignKey(a => a.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasMany(a => a.ApplicationSkills)
                .WithOne(asl => asl.Application)
                .HasForeignKey(asl => asl.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
