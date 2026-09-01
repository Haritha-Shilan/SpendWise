using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace SpendWise.Infrastructure.Identity
{
    public static class IdentitySeeder
    {
        public static async Task SeedAsync(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            const string adminRole = "Admin";

            if(!await roleManager.RoleExistsAsync(adminRole))
            {
                await roleManager.CreateAsync(new IdentityRole(adminRole));
            }

            var adminEmail = configuration["Admin:Email"];
            var adminPassword =configuration["Admin:Password"];

            if(string.IsNullOrWhiteSpace(adminEmail))
            {
                throw new InvalidOperationException("Admin email is not configured.");
            }

            if (string.IsNullOrWhiteSpace(adminPassword))
            {
                throw new InvalidOperationException("Admin password is not configured.");
            }

            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if(adminUser==null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    FullName = "System Administrator",
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);

                if(!result.Succeeded)
                {
                    var errors = string.Join(",",
                        result.Errors.Select(x=>x.Description));

                    throw new InvalidOperationException($"Failed to create admin user: {errors}");
                }
            }

            if(!await userManager.IsInRoleAsync(adminUser,adminRole))
            {
                await userManager.AddToRoleAsync(adminUser, adminRole);
            }
        }
    }
}
