using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Identity
{
    public static class AppIdentityDbContextSeed
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "Admin", "User" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var User = new AppUser()
                {
                    DisplayName = "Christine Adib",
                    Email = "christadib@gmail.com",
                    UserName = "christadib",
                    PhoneNumber = "01234567891"
                };
                var result = await userManager.CreateAsync(User, "Pa$$w0rd");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(User, "Admin"); 
                }
            }
        }
    }
}
