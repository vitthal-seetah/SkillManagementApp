using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SkillManager.Domain.Entities;
using SkillManager.Infrastructure.Abstractions.Identity;
using SkillManager.Infrastructure.Identity.Models;
using AppEntity = SkillManager.Domain.Entities.Application;

namespace SkillManager.Infrastructure.Identity.DbContext;

public class ApplicationIdentityDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options)
        : base(options) { }

    // DbSets
    public DbSet<CategoryType> CategoryTypes { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<SubCategory> SubCategories { get; set; }
    public DbSet<Skill> Skills { get; set; }
    public DbSet<Level> Levels { get; set; }
    public DbSet<UserSkill> UserSkills { get; set; }
    public DbSet<UserSME> UserSMEs { get; set; }
    public DbSet<ApplicationSuite> ApplicationSuites { get; set; }
    public DbSet<AppEntity> Applications { get; set; }
    public DbSet<ApplicationSkill> ApplicationSkills { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure composite keys
        modelBuilder.Entity<UserSkill>().HasKey(us => new { us.UserId, us.SkillId });

        modelBuilder
            .Entity<UserSME>()
            .HasKey(us => new
            {
                us.UserId,
                us.SkillId,
                us.CategoryTypeId,
            });

        modelBuilder.Entity<ApplicationSkill>().HasKey(a => new { a.ApplicationId, a.SkillId });

        // Configure relationships WITHOUT User navigation

        // UserSkill -> Skill (One-to-Many)
        modelBuilder
            .Entity<UserSkill>()
            .HasOne(us => us.Skill)
            .WithMany(s => s.UserSkills)
            .HasForeignKey(us => us.SkillId)
            .OnDelete(DeleteBehavior.Cascade);

        // UserSkill -> Level (One-to-Many)
        modelBuilder
            .Entity<UserSkill>()
            .HasOne(us => us.Level)
            .WithMany(l => l.UserSkills)
            .HasForeignKey(us => us.LevelId)
            .OnDelete(DeleteBehavior.Restrict);

        // UserSME -> Skill (One-to-Many)
        modelBuilder
            .Entity<UserSME>()
            .HasOne(us => us.Skill)
            .WithMany(s => s.UserSMEs)
            .HasForeignKey(us => us.SkillId)
            .OnDelete(DeleteBehavior.Cascade);

        // UserSME -> CategoryType (One-to-Many)
        modelBuilder
            .Entity<UserSME>()
            .HasOne(us => us.CategoryType)
            .WithMany()
            .HasForeignKey(us => us.CategoryTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure ApplicationUser relationships (one-way)
        modelBuilder
            .Entity<Models.ApplicationUser>()
            .HasMany<UserSkill>()
            .WithOne()
            .HasForeignKey(us => us.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder
            .Entity<Models.ApplicationUser>()
            .HasMany<UserSME>()
            .WithOne()
            .HasForeignKey(us => us.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Rest of your configurations remain the same
        modelBuilder
            .Entity<CategoryType>()
            .HasMany(ct => ct.Categories)
            .WithOne(c => c.CategoryType)
            .HasForeignKey(c => c.CategoryTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder
            .Entity<Category>()
            .HasMany(c => c.Skills)
            .WithOne(s => s.Category)
            .HasForeignKey(s => s.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder
            .Entity<Category>()
            .HasMany(c => c.Applications)
            .WithOne(a => a.Category)
            .HasForeignKey(a => a.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder
            .Entity<Category>()
            .HasMany(c => c.SubCategories)
            .WithOne(sc => sc.Category)
            .HasForeignKey(sc => sc.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder
            .Entity<SubCategory>()
            .HasMany(sc => sc.Skills)
            .WithOne(s => s.SubCategory)
            .HasForeignKey(s => s.SubCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder
            .Entity<ApplicationSuite>()
            .HasMany(asu => asu.Applications)
            .WithOne(a => a.ApplicationSuite)
            .HasForeignKey(a => a.SuiteId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder
            .Entity<Domain.Entities.Application>()
            .HasMany(a => a.ApplicationSkills)
            .WithOne(appSkill => appSkill.Application)
            .HasForeignKey(appSkill => appSkill.ApplicationId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder
            .Entity<Skill>()
            .HasMany(s => s.ApplicationSkills)
            .WithOne(appSkill => appSkill.Skill)
            .HasForeignKey(appSkill => appSkill.SkillId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure string lengths and constraints
        modelBuilder.Entity<Skill>().Property(s => s.Name).HasMaxLength(200);

        modelBuilder.Entity<Skill>().Property(s => s.Code).HasMaxLength(50);

        modelBuilder.Entity<Skill>().Property(s => s.Label).HasMaxLength(200);

        modelBuilder.Entity<Skill>().Property(s => s.CriticalityLevel).HasMaxLength(50);

        modelBuilder.Entity<Level>().Property(l => l.Name).HasMaxLength(100);

        modelBuilder.Entity<CategoryType>().Property(ct => ct.Name).HasMaxLength(100);

        modelBuilder.Entity<Category>().Property(c => c.Name).HasMaxLength(100);

        modelBuilder.Entity<SubCategory>().Property(sc => sc.Name).HasMaxLength(100);

        modelBuilder.Entity<Domain.Entities.Application>().Property(a => a.Name).HasMaxLength(200);

        modelBuilder.Entity<ApplicationSuite>().Property(asu => asu.Name).HasMaxLength(200);

        modelBuilder.Entity<ApplicationSuite>().Property(asu => asu.Perimeter).HasMaxLength(500);

        // Indexes for performance
        modelBuilder.Entity<Skill>().HasIndex(s => s.Code).IsUnique();

        modelBuilder.Entity<UserSkill>().HasIndex(us => us.UserId);

        modelBuilder.Entity<UserSkill>().HasIndex(us => us.SkillId);

        modelBuilder.Entity<UserSME>().HasIndex(us => us.UserId);

        modelBuilder.Entity<UserSME>().HasIndex(us => us.SkillId);

        modelBuilder.Entity<ApplicationSkill>().HasIndex(a => a.ApplicationId);

        modelBuilder.Entity<ApplicationSkill>().HasIndex(a => a.SkillId);
    }
}
