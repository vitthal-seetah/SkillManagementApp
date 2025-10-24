using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillManager.Domain.Entities;

namespace SkillManager.Infrastructure.Identity.AppDbContext.Configurations;

public class SubCategoryConfiguration : IEntityTypeConfiguration<SubCategory>
{
    public void Configure(EntityTypeBuilder<SubCategory> builder)
    {
        builder.ToTable("SubCategories");

        builder.HasKey(sc => sc.SubCategoryId);

        builder.Property(sc => sc.Name).IsRequired().HasMaxLength(100);

        builder
            .HasOne(sc => sc.Category)
            .WithMany(c => c.SubCategories)
            .HasForeignKey(sc => sc.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasData(
            // 01 - CONNAISSANCES METIER
            new SubCategory
            {
                SubCategoryId = 1,
                Name = "Finance",
                CategoryId = 1,
            },
            // 02 - CONCEPTION SOLUTION SI
            new SubCategory
            {
                SubCategoryId = 2,
                Name = "Cadrage",
                CategoryId = 2,
            },
            new SubCategory
            {
                SubCategoryId = 3,
                Name = "Design",
                CategoryId = 2,
            },
            new SubCategory
            {
                SubCategoryId = 4,
                Name = "Design fonctionnel",
                CategoryId = 2,
            },
            new SubCategory
            {
                SubCategoryId = 5,
                Name = "Design technique",
                CategoryId = 2,
            },
            new SubCategory
            {
                SubCategoryId = 6,
                Name = "Data",
                CategoryId = 2,
            },
            new SubCategory
            {
                SubCategoryId = 7,
                Name = "Analyse",
                CategoryId = 2,
            },
            new SubCategory
            {
                SubCategoryId = 8,
                Name = "Testing",
                CategoryId = 2,
            },
            new SubCategory
            {
                SubCategoryId = 9,
                Name = "Sécurité",
                CategoryId = 2,
            },
            // 03 - CONNAISSANCE APPLI AMC
            new SubCategory
            {
                SubCategoryId = 10,
                Name = "Outil Compta",
                CategoryId = 3,
            },
            new SubCategory
            {
                SubCategoryId = 11,
                Name = "Outil CdG",
                CategoryId = 3,
            },
            new SubCategory
            {
                SubCategoryId = 12,
                Name = "Outil Compta reglementaire",
                CategoryId = 3,
            },
            new SubCategory
            {
                SubCategoryId = 13,
                Name = "Outil Conso",
                CategoryId = 3,
            },
            new SubCategory
            {
                SubCategoryId = 14,
                Name = "Interprétation comptable",
                CategoryId = 3,
            },
            new SubCategory
            {
                SubCategoryId = 15,
                Name = "Rapprochements",
                CategoryId = 3,
            },
            new SubCategory
            {
                SubCategoryId = 16,
                Name = "Technique",
                CategoryId = 3,
            },
            // 04 - TECHNIQUES DE PROGRAMMATION
            new SubCategory
            {
                SubCategoryId = 17,
                Name = "Langage de programmation",
                CategoryId = 4,
            },
            new SubCategory
            {
                SubCategoryId = 18,
                Name = "Transfert de fichiers",
                CategoryId = 4,
            },
            new SubCategory
            {
                SubCategoryId = 19,
                Name = "CI/CD",
                CategoryId = 4,
            },
            new SubCategory
            {
                SubCategoryId = 20,
                Name = "Frameworks .NET",
                CategoryId = 4,
            },
            new SubCategory
            {
                SubCategoryId = 21,
                Name = "Conso",
                CategoryId = 4,
            },
            new SubCategory
            {
                SubCategoryId = 22,
                Name = "Scripting",
                CategoryId = 4,
            },
            new SubCategory
            {
                SubCategoryId = 23,
                Name = "Front-End",
                CategoryId = 4,
            },
            new SubCategory
            {
                SubCategoryId = 24,
                Name = "Middleware",
                CategoryId = 4,
            },
            new SubCategory
            {
                SubCategoryId = 25,
                Name = "Gestion du code",
                CategoryId = 4,
            },
            new SubCategory
            {
                SubCategoryId = 26,
                Name = "Base de données",
                CategoryId = 4,
            },
            new SubCategory
            {
                SubCategoryId = 27,
                Name = "Testing",
                CategoryId = 4,
            },
            new SubCategory
            {
                SubCategoryId = 28,
                Name = "Sécurité",
                CategoryId = 4,
            },
            new SubCategory
            {
                SubCategoryId = 29,
                Name = "OS",
                CategoryId = 4,
            },
            new SubCategory
            {
                SubCategoryId = 30,
                Name = "ETL",
                CategoryId = 4,
            },
            new SubCategory
            {
                SubCategoryId = 31,
                Name = "Supervision / Monitoring",
                CategoryId = 4,
            },
            new SubCategory
            {
                SubCategoryId = 32,
                Name = "Paramétrage progiciel",
                CategoryId = 4,
            },
            // 05 - GESTION DE PROJET → none

            // 06 - AUTRES COMPETENCES
            new SubCategory
            {
                SubCategoryId = 33,
                Name = "Communication",
                CategoryId = 6,
            }
        );
    }
}
