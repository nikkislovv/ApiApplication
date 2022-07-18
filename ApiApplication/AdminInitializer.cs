using ApiApplication.Models;
using Contracts;
using Microsoft.AspNetCore.Identity;
using Repository;
using System.Threading.Tasks;

namespace Server
{
    public class AdminInitializer
    {
        const string adminUserName= "Nikita";
        const string adminFullName= "Kislov";
        const string password = "7nikita4";
        const string AdminEmail = "nikitakislov368@gmail.com";
        const string adminPhoneNumber = "+375445628241";
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (await roleManager.FindByNameAsync("administrator") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("administrator"));
            }
            if (await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("user"));
            }

            if (await userManager.FindByNameAsync(adminUserName) == null)
            {
                User admin = new User { UserName = adminUserName, Email=AdminEmail, FullName = adminFullName,PhoneNumber=adminPhoneNumber};
                var result= await userManager.CreateAsync(admin,password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");

                }
            }

        }
    }
}
