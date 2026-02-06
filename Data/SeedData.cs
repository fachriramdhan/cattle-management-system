using Microsoft.AspNetCore.Identity;
using SimSapi.Models;

namespace SimSapi.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Create Roles
            string[] roleNames = { "Admin", "User" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Create Admin User
            var adminEmail = "admin@simsapi.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    NamaLengkap = "Administrator",
                    EmailConfirmed = true,
                    IsActive = true
                };

                var result = await userManager.CreateAsync(adminUser, "Admin123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // Create Demo User
            var userEmail = "user@simsapi.com";
            var demoUser = await userManager.FindByEmailAsync(userEmail);

            if (demoUser == null)
            {
                demoUser = new ApplicationUser
                {
                    UserName = userEmail,
                    Email = userEmail,
                    NamaLengkap = "Peternak Demo",
                    EmailConfirmed = true,
                    IsActive = true
                };

                var result = await userManager.CreateAsync(demoUser, "User123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(demoUser, "User");
                }
            }
        }
    }
}