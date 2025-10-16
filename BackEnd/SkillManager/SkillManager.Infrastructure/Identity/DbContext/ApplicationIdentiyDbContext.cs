using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SkillManager.Domain.Entities;
using SkillManager.Infrastructure.Identity.Models;

namespace SkillManager.Infrastructure.Identity.DbContext;

public class ApplicationIdentityDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options)
        : base(options) { }

    public DbSet<Category> Categories { get; set; }
    public DbSet<SkillSection> SkillSections { get; set; }
    public DbSet<Skill> Skills { get; set; }
    public DbSet<UserSkill> UserSkills { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationIdentityDbContext).Assembly);

        // --- SkillSection → Category (many-to-one) ---
        builder
            .Entity<SkillSection>()
            .HasOne(ss => ss.Category)
            .WithMany(c => c.Sections)
            .HasForeignKey(ss => ss.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        // --- Skill → SkillSection (many-to-one) ---
        builder
            .Entity<Skill>()
            .HasOne(s => s.SkillSection)
            .WithMany(ss => ss.Skills)
            .HasForeignKey(s => s.SkillSectionId)
            .OnDelete(DeleteBehavior.Cascade);

        // --- UserSkill configuration ---
        builder.Entity<UserSkill>().HasKey(us => us.Id); // Primary key

        builder
            .Entity<UserSkill>()
            .HasOne(us => us.Skill)
            .WithMany(s => s.UserSkills)
            .HasForeignKey(us => us.SkillId)
            .OnDelete(DeleteBehavior.Cascade);

        // Optional: Navigation to ApplicationUser (Infrastructure)
        builder
            .Entity<UserSkill>()
            .HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(us => us.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
