using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TeamTaskManager.Domain.Entities;
using TeamTaskManager.Domain.Enums;

namespace TeamTaskManager.Infrastructure.Configurations
{
    public class TaskConfiguration : IEntityTypeConfiguration<Tasks>
    {
        public void Configure(EntityTypeBuilder<Tasks> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Description).HasMaxLength(1000);
            builder.Property(x => x.Status).HasDefaultValue(TasksStatus.Todo).HasSentinel(TasksStatus.Todo);
            builder.Property(x => x.Priority).HasDefaultValue(TaskPriority.Medium).HasSentinel(TaskPriority.Medium);
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            
            builder.HasOne(x => x.Project).WithMany(x => x.Tasks).HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.CreatedBy).WithMany().HasForeignKey(x => x.CreatedByUserId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.AssignedTo).WithMany(x => x.AssignedTasks).HasForeignKey(x => x.AssignedToUserId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(x => x.Comments).WithOne(x => x.Task).HasForeignKey(x => x.TaskId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
