using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TeamTaskManager.Domain.Entities;

namespace TeamTaskManager.Infrastructure.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Content).IsRequired().HasMaxLength(1000);
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");


            builder.HasOne(x => x.Task).WithMany(x => x.Comments).HasForeignKey(x => x.TaskId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.User).WithMany(x => x.Comments).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
