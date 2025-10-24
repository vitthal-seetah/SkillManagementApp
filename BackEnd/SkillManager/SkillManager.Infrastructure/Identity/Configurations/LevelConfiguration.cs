using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillManager.Domain.Entities;

namespace SkillManager.Infrastructure.Identity.Configurations;

public class LevelConfiguration : IEntityTypeConfiguration<Level>
{
    public void Configure(EntityTypeBuilder<Level> builder)
    {
        builder.Property(l => l.Name).HasMaxLength(100);

        builder.HasData(
            new Level
            {
                LevelId = 1,
                Name = "Notion",
                Points = 1,
            },
            new Level
            {
                LevelId = 2,
                Name = "Pratique",
                Points = 2,
            },
            new Level
            {
                LevelId = 3,
                Name = "Maitrise",
                Points = 3,
            },
            new Level
            {
                LevelId = 4,
                Name = "Expert",
                Points = 4,
            }
        );
    }
}
