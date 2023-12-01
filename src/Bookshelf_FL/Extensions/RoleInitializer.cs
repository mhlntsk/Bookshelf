using Bookshelf_TL.Models;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Bookshelf_SL
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(IConfiguration configuration, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }

            if (await userManager.FindByEmailAsync(configuration["AdminDefaultEmail"]) == null)
            {
                User admin = new User { Id = configuration["AdminDefaultId"], Email = configuration["AdminDefaultEmail"], UserName = configuration["AdminDefaultLogin"] };
                IdentityResult result = await userManager.CreateAsync(admin, configuration["AdminDefaultPass"]);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
        }
    }
}
