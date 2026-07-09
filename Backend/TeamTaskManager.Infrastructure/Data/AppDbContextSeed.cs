using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace TeamTaskManager.Infrastructure.Data
{
    public static class AppDbContextSeed
    {
        public static async System.Threading.Tasks.Task SeedRolesAsync(RoleManager<IdentityRole<Guid>> roleManager)
        {
            var roles = new[] { "Admin", "Manager", "Member" };
            foreach(var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole<Guid>(role));
            }
        }
    }
}
