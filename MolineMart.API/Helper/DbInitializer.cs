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

            var admin = await userManager.FindByEmailAsync(adminEmail);


            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new IdentityRole("Admin"));

            if(admin == null)
            {
                var user = new ApplicationUser { UserName = adminEmail, Email = adminEmail, FullName = "Admin User", 
                    EmailConfirmed = true };
                var result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                    await userManager.AddToRoleAsync(user, "Admin");
            }

            // ---------------------------------------------
            // APPLY ADDRESS DATA TO ALL EXISTING USERS
            // ---------------------------------------------

            var allUsers = userManager.Users.ToList();

            foreach (var u in allUsers)
            {
                bool needsUpdate = false;

                if (string.IsNullOrWhiteSpace(u.AddressLine1))
                {
                    u.AddressLine1 = "123 Main St";
                    needsUpdate = true;
                }

                if (string.IsNullOrWhiteSpace(u.AddressLine2))
                {
                    u.AddressLine2 = "Apt 4B";
                    needsUpdate = true;
                }

                if (string.IsNullOrWhiteSpace(u.City))
                {
                    u.City = "Pune";
                    needsUpdate = true;
                }

                if (string.IsNullOrWhiteSpace(u.State))
                {
                    u.State = "Maharashtra";
                    needsUpdate = true;
                }

                if (string.IsNullOrWhiteSpace(u.Country))
                {
                    u.Country = "India";
                    needsUpdate = true;
                }

                if (string.IsNullOrWhiteSpace(u.ZipCode))
                {
                    u.ZipCode = "400001";
                    needsUpdate = true;
                }

                if (needsUpdate)
                    await userManager.UpdateAsync(u);
            }
        }
    }
}
