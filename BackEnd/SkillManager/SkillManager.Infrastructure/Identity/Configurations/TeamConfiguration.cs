using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillManager.Domain.Entities;

namespace SkillManager.Infrastructure.Identity.Configurations
{
    public class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.ToTable("Teams");

            builder.HasKey(t => t.TeamId);

            builder.Property(t => t.TeamName).IsRequired().HasMaxLength(100);

            builder.Property(t => t.TeamDescription).HasMaxLength(500);

            // Relationship: Team has one TeamLead (optional)
            builder
                .HasOne(t => t.TeamLead)
                .WithMany() // No inverse navigation - user doesn't know they lead teams
                .HasForeignKey(t => t.TeamLeadId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            // Relationship: Team has many Members (Users) - One-to-Many
            builder
                .HasMany(t => t.Members)
                .WithOne(u => u.Team)
                .HasForeignKey(u => u.TeamId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            // Relationship: Team has many ProjectTeams (Many-to-Many with Projects)
            builder
                .HasMany(t => t.ProjectTeams)
                .WithOne(pt => pt.Team)
                .HasForeignKey(pt => pt.TeamId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
