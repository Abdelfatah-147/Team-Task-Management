using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TeamTaskManager.Domain.Entities;

namespace TeamTaskManager.Infrastructure.Configurations
{
    public class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(120);
            builder.Property(x => x.Description).HasMaxLength(700);
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");


            builder.HasOne(x => x.CreatedBy).WithMany().HasForeignKey(x => x.CreatedByUserId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(x => x.Members).WithOne(x => x.Team).HasForeignKey(x => x.TeamId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.projects).WithOne(x => x.Team).HasForeignKey(x => x.TeamId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
