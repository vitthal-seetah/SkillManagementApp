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
    public DbSet<Domain.Entities.User> Users { get; set; }

    public DbSet<UserRole> UserRoles { get; set; }

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

        modelBuilder.Entity<Domain.Entities.User>().HasIndex(u => u.UtCode).IsUnique();

        modelBuilder.Entity<Domain.Entities.User>().HasIndex(u => u.RefId).IsUnique();

        modelBuilder.Entity<UserRole>().HasIndex(r => r.Name).IsUnique();

        // Add unique constraint for Eid as well
        modelBuilder.Entity<Domain.Entities.User>().HasIndex(u => u.Eid).IsUnique();
        // Configure Enums
        modelBuilder
            .Entity<Domain.Entities.User>()
            .Property(u => u.Status)
            .HasConversion<string>();

        modelBuilder
            .Entity<Domain.Entities.User>()
            .Property(u => u.DeliveryType)
            .HasConversion<string>();

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

        // Configure relationships

        // User -> UserRole (Many-to-One)
        modelBuilder
            .Entity<Domain.Entities.User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        // User -> UserSkills (One-to-Many)
        modelBuilder
            .Entity<Domain.Entities.User>()
            .HasMany(u => u.UserSkills)
            .WithOne(us => us.User)
            .HasForeignKey(us => us.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // User -> UserSMEs (One-to-Many)
        modelBuilder
            .Entity<Domain.Entities.User>()
            .HasMany(u => u.UserSMEs)
            .WithOne(us => us.User)
            .HasForeignKey(us => us.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // UserSkill -> Skill (Many-to-One)
        modelBuilder
            .Entity<UserSkill>()
            .HasOne(us => us.Skill)
            .WithMany(s => s.UserSkills)
            .HasForeignKey(us => us.SkillId)
            .OnDelete(DeleteBehavior.Cascade);

        // UserSkill -> Level (Many-to-One)
        modelBuilder
            .Entity<UserSkill>()
            .HasOne(us => us.Level)
            .WithMany(l => l.UserSkills)
            .HasForeignKey(us => us.LevelId)
            .OnDelete(DeleteBehavior.Restrict);

        // UserSME -> Skill (Many-to-One)
        modelBuilder
            .Entity<UserSME>()
            .HasOne(us => us.Skill)
            .WithMany(s => s.UserSMEs)
            .HasForeignKey(us => us.SkillId)
            .OnDelete(DeleteBehavior.Cascade);

        // UserSME -> CategoryType (Many-to-One)
        modelBuilder
            .Entity<UserSME>()
            .HasOne(us => us.CategoryType)
            .WithMany()
            .HasForeignKey(us => us.CategoryTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        // CategoryType -> Categories (One-to-Many)
        modelBuilder
            .Entity<CategoryType>()
            .HasMany(ct => ct.Categories)
            .WithOne(c => c.CategoryType)
            .HasForeignKey(c => c.CategoryTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Category -> Skills (One-to-Many)
        modelBuilder
            .Entity<Category>()
            .HasMany(c => c.Skills)
            .WithOne(s => s.Category)
            .HasForeignKey(s => s.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Category -> Applications (One-to-Many)
        modelBuilder
            .Entity<Category>()
            .HasMany(c => c.Applications)
            .WithOne(a => a.Category)
            .HasForeignKey(a => a.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Category -> SubCategories (One-to-Many)
        modelBuilder
            .Entity<Category>()
            .HasMany(c => c.SubCategories)
            .WithOne(sc => sc.Category)
            .HasForeignKey(sc => sc.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        // SubCategory -> Skills (One-to-Many)
        modelBuilder
            .Entity<SubCategory>()
            .HasMany(sc => sc.Skills)
            .WithOne(s => s.SubCategory)
            .HasForeignKey(s => s.SubCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // ApplicationSuite -> Applications (One-to-Many)
        modelBuilder
            .Entity<ApplicationSuite>()
            .HasMany(asu => asu.Applications)
            .WithOne(a => a.ApplicationSuite)
            .HasForeignKey(a => a.SuiteId)
            .OnDelete(DeleteBehavior.Restrict);

        // Application -> ApplicationSkills (One-to-Many)
        modelBuilder
            .Entity<Domain.Entities.Application>()
            .HasMany(a => a.ApplicationSkills)
            .WithOne(appSkill => appSkill.Application)
            .HasForeignKey(appSkill => appSkill.ApplicationId)
            .OnDelete(DeleteBehavior.Cascade);

        // Skill -> ApplicationSkills (One-to-Many)
        modelBuilder
            .Entity<Skill>()
            .HasMany(s => s.ApplicationSkills)
            .WithOne(appSkill => appSkill.Skill)
            .HasForeignKey(appSkill => appSkill.SkillId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure string lengths and constraints
        modelBuilder.Entity<Domain.Entities.User>().Property(u => u.FirstName).HasMaxLength(100);

        modelBuilder.Entity<Domain.Entities.User>().Property(u => u.LastName).HasMaxLength(100);

        modelBuilder.Entity<Domain.Entities.User>().Property(u => u.UtCode).HasMaxLength(50);

        modelBuilder.Entity<Domain.Entities.User>().Property(u => u.RefId).HasMaxLength(100);

        modelBuilder.Entity<Domain.Entities.User>().Property(u => u.Domain).HasMaxLength(100);

        modelBuilder.Entity<Domain.Entities.User>().Property(u => u.Eid).HasMaxLength(50);

        modelBuilder.Entity<UserRole>().Property(r => r.Name).HasMaxLength(50);

        modelBuilder.Entity<Skill>().Property(s => s.Code).HasMaxLength(50);

        modelBuilder.Entity<Skill>().Property(s => s.Label).HasMaxLength(200);

        modelBuilder.Entity<Skill>().Property(s => s.CriticalityLevel).HasMaxLength(50);

        modelBuilder.Entity<Level>().Property(l => l.Name).HasMaxLength(100);

        modelBuilder.Entity<CategoryType>().Property(ct => ct.Name).HasMaxLength(100);

        modelBuilder.Entity<Category>().Property(c => c.Name).HasMaxLength(100);

        modelBuilder.Entity<SubCategory>().Property(sc => sc.Name).HasMaxLength(100);

        modelBuilder.Entity<AppEntity>().Property(a => a.Name).HasMaxLength(200);

        modelBuilder.Entity<ApplicationSuite>().Property(asu => asu.Name).HasMaxLength(200);

        modelBuilder.Entity<ApplicationSuite>().Property(asu => asu.Perimeter).HasMaxLength(500);

        // Indexes for performance
        modelBuilder.Entity<Domain.Entities.User>().HasIndex(u => u.UtCode).IsUnique();

        modelBuilder.Entity<Domain.Entities.User>().HasIndex(u => u.Eid).IsUnique();

        modelBuilder.Entity<UserRole>().HasIndex(r => r.Name).IsUnique();

        modelBuilder.Entity<Skill>().HasIndex(s => s.Code).IsUnique();

        modelBuilder.Entity<UserSkill>().HasIndex(us => us.UserId);

        modelBuilder.Entity<UserSkill>().HasIndex(us => us.SkillId);

        modelBuilder.Entity<UserSME>().HasIndex(us => us.UserId);

        modelBuilder.Entity<UserSME>().HasIndex(us => us.SkillId);

        modelBuilder.Entity<ApplicationSkill>().HasIndex(a => a.ApplicationId);

        modelBuilder.Entity<ApplicationSkill>().HasIndex(a => a.SkillId);

        // Seed initial data
        // SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // Seed UserRoles
        modelBuilder
            .Entity<UserRole>()
            .HasData(
                new UserRole { RoleId = 1, Name = "Admin" },
                new UserRole { RoleId = 2, Name = "TechLead" },
                new UserRole { RoleId = 3, Name = "Manager" },
                new UserRole { RoleId = 4, Name = "Employee" }
            );

        modelBuilder
            .Entity<CategoryType>()
            .HasData(
                new CategoryType { CategoryTypeId = 1, Name = "Technical" },
                new CategoryType { CategoryTypeId = 2, Name = "Functional" }
            );

        modelBuilder
            .Entity<Level>()
            .HasData(
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
