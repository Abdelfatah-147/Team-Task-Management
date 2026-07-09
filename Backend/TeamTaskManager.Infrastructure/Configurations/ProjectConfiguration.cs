using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TeamTaskManager.Domain.Entities;
using TeamTaskManager.Domain.Enums;

namespace TeamTaskManager.Infrastructure.Configurations
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Description).HasMaxLength(500);
            builder.Property(x => x.Status).HasDefaultValue(ProjectStatus.NotStarted).HasSentinel(ProjectStatus.NotStarted);
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");


            builder.HasOne(x => x.Team).WithMany(x => x.projects).HasForeignKey(x => x.TeamId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.CreatedBy).WithMany().HasForeignKey(x => x.CreatedByUserId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(x => x.Tasks).WithOne(x => x.Project).HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
