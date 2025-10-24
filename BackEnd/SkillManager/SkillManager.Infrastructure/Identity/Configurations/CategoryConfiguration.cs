using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillManager.Domain.Entities;

namespace SkillManager.Infrastructure.Identity.AppDbContext.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        // 🔹 Table
        builder.ToTable("Categories");

        // 🔹 Primary Key
        builder.HasKey(c => c.CategoryId);

        // 🔹 Properties
        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);

        // 🔹 Relationships
        builder
            .HasOne(c => c.CategoryType)
            .WithMany(ct => ct.Categories)
            .HasForeignKey(c => c.CategoryTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(c => c.Skills)
            .WithOne(s => s.Category)
            .HasForeignKey(s => s.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(c => c.Applications)
            .WithOne(a => a.Category)
            .HasForeignKey(a => a.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(c => c.SubCategories)
            .WithOne(sc => sc.Category)
            .HasForeignKey(sc => sc.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        // 🔹 Index
        builder.HasIndex(c => c.Name);

        // 🔹 Seed Data
        builder.HasData(
            new Category
            {
                CategoryId = 1,
                Name = "CONNAISSANCES METIER",
                CategoryTypeId = 2, // Functional
            },
            new Category
            {
                CategoryId = 2,
                Name = "CONCEPTION SOLUTION SI",
                CategoryTypeId = 1, // Technical
            },
            new Category
            {
                CategoryId = 3,
                Name = "CONNAISSANCE APPLI AMC",
                CategoryTypeId = 1, // Technical
            },
            new Category
            {
                CategoryId = 4,
                Name = "TECHNIQUES DE PROGRAMMATION",
                CategoryTypeId = 1, // Technical
            },
            new Category
            {
                CategoryId = 5,
                Name = "GESTION DE PROJET",
                CategoryTypeId = 2, // Functional
            },
            new Category
            {
                CategoryId = 6,
                Name = "AUTRES COMPETENCES",
                CategoryTypeId = 2, // Functional
            }
        );
    }
}
