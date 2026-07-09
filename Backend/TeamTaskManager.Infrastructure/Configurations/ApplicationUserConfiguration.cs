using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TeamTaskManager.Domain.Entities;

namespace TeamTaskManager.Infrastructure.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(x => x.FullName).IsRequired().HasMaxLength(120);
            builder.Property(x => x.IsActive).HasDefaultValue(true);

            builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
