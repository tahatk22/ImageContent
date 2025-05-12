using ImageContent.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageContent.Common.SeedingHelper
{
    public class IdentitySeeder
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        List<string> roles = new()
        {
            "Admin","User"
        };

        public IdentitySeeder(UserManager<ApplicationUser> userManager , RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task SeedRoles()
        {
            await EnsureRolesExists(roles);

            if (!userManager.Users.Any())
            {
                var Admin = new ApplicationUser() {UserName = "admin",Email = "admin@mail.com",FirstName = "Admin",LastName = "Admin"};
                var User = new ApplicationUser() {UserName = "user",Email = "user@mail.com",FirstName = "user",LastName = "user"};
                await userManager.CreateAsync(Admin, "Admin@123#");
                await userManager.CreateAsync(User, "User@123#");

                await userManager.AddToRoleAsync(Admin, "Admin");
                await userManager.AddToRoleAsync(User, "User");
            }
        }

        private async Task EnsureRolesExists(List<string> roles)
        {
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
