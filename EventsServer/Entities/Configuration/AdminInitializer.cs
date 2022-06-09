using Entities.Models;
using Microsoft.AspNetCore.Identity;
namespace Entities
{
    public class AdminInitializer
    {
        private static string UserName => "Admin";
        private static string Password => "124578369Aa";

        public static async Task InitializeAsync(UserManager<User> userManager)
        {
            var admin = new User()
            {
                FirstName = "Admin",
                LastName = "Admin",
                UserName = "Admin",
                Email = "Admin@gmail.com"
            };

            if (await userManager.FindByNameAsync(UserName) == null)
            {
                var result = await userManager.CreateAsync(admin, Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }
        }
    }
}