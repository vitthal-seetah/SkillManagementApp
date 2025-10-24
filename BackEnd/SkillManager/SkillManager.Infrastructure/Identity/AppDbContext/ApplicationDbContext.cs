using Microsoft.EntityFrameworkCore;
using SkillManager.Domain.Entities;
using AppEntity = SkillManager.Domain.Entities.Application;

namespace SkillManager.Infrastructure.Identity.AppDbContext;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<User> Users { get; set; }
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
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
