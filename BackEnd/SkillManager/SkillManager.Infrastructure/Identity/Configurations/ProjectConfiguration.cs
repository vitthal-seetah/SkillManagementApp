using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillManager.Domain.Entities;

namespace SkillManager.Infrastructure.Identity.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("Projects");
        builder.HasKey(p => p.ProjectId);

        // Project has many ProjectTeams
        builder
            .HasMany(p => p.ProjectTeams)
            .WithOne(pt => pt.Project)
            .HasForeignKey(pt => pt.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasData(
            new Project
            {
                ProjectId = 1,
                ProjectName = "SMAC",
                ProjectDescription = "Doing Things",
            }
        );
    }
}
