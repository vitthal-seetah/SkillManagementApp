using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillManager.Domain.Entities;

namespace SkillManager.Infrastructure.Identity.Configurations;

public class CategoryTypeConfiguration : IEntityTypeConfiguration<CategoryType>
{
    public void Configure(EntityTypeBuilder<CategoryType> builder)
    {
        builder.Property(ct => ct.Name).HasMaxLength(100);

        builder
            .HasMany(ct => ct.Categories)
            .WithOne(c => c.CategoryType)
            .HasForeignKey(c => c.CategoryTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(
            new CategoryType { CategoryTypeId = 1, Name = "Technical" },
            new CategoryType { CategoryTypeId = 2, Name = "Functional" }
        );
    }
}
