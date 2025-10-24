using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillManager.Domain.Entities;

namespace SkillManager.Infrastructure.Identity.AppDbContext.Configurations
{
    public class ApplicationSuiteConfiguration : IEntityTypeConfiguration<ApplicationSuite>
    {
        public void Configure(EntityTypeBuilder<ApplicationSuite> builder)
        {
            builder.ToTable("ApplicationSuites");

            builder.HasKey(s => s.SuiteId);

            builder.Property(s => s.Name).IsRequired().HasMaxLength(200);

            builder.Property(s => s.Perimeter).HasMaxLength(500);

            builder
                .HasMany(s => s.Applications)
                .WithOne(a => a.ApplicationSuite)
                .HasForeignKey(a => a.SuiteId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
