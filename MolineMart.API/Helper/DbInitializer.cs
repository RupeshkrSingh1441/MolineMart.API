using Microsoft.AspNetCore.Identity;
using MolineMart.API.Models;

namespace MolineMart.API.Helper
{
    public class DbInitializer
    {
        public static async Task SeedAdmin(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string adminEmail = "rs.rupesh105@gmail.com";//rs.rupesh105@gmail.com
            string password = "Admin@123"; //Admin@123

            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new IdentityRole("Admin"));

            if(await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var user = new ApplicationUser { UserName = adminEmail, Email = adminEmail, FullName = "Admin User", EmailConfirmed = true };
                var result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                    await userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }
}
