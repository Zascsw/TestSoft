using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using TestSoft;
using static System.Console;
using Microsoft.Identity.Client;
using TestSoft.Areas.Identity.Pages.Account;

namespace TestSoft.Controllers
{
    public class RolesController : Controller
    {
        private string AdminRole = "Administrators";
        private string UserRole = "user";
       // private string UserEmail = "test@example.com";
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;
        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        public async Task<IActionResult> Index(RegisterModel model)
        {
            if (!(await roleManager.RoleExistsAsync(AdminRole)))
            {
                await roleManager.CreateAsync(new IdentityRole(AdminRole));
            }
            if (!(await roleManager.RoleExistsAsync(UserRole)))
            {
                await roleManager.CreateAsync(new IdentityRole(UserRole));
            }
            IdentityUser user = await userManager.FindByEmailAsync(model.Input.Email);
         /*   if(ModelState.IsValid)
            {
                bool isAdmin = Request.Form.ContainsKey("isAdminChecbox");
                // Добавляем пользователя к нужной роли
                if (isAdmin)
                {
                    await userManager.AddToRoleAsync(user, "Administrators");
                }
                else
                {
                    await userManager.AddToRoleAsync(user, "User");
                }
            }*/
         /*   if (user == null)
            {
                user = new();
                user.UserName = UserEmail;
                user.Email = UserEmail;
                IdentityResult result = await userManager.CreateAsync(user, "Pa$$w0rd");
                if (result.Succeeded)
                {
                    WriteLine($"User {user.UserName} created successfully");
                }
                else foreach(IdentityError error in result.Errors)
                    {
                        WriteLine(error.Description);
                    }
            }*/
            if (!user.EmailConfirmed)
            {
                string token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                IdentityResult result = await userManager.ConfirmEmailAsync(user, token);
                    if (result.Succeeded)
                    {
                    WriteLine($"User {user.UserName} email confirmed succesefully");
                    }
                    else foreach (IdentityError error in result.Errors)
                    {
                        WriteLine($"Error: {error.Description}");
                    }
            }
            
                if (model.Input.isAdmin)
                {
                    IdentityResult result = await userManager.AddToRoleAsync(user, AdminRole);
                    if (result.Succeeded)
                    {
                        WriteLine($"User {user.UserName} added to {AdminRole}");
                    }
                    else
                    {
                        foreach (IdentityError error in result.Errors)
                        {
                            WriteLine(error.Description);
                        }
                    }
                }
                else
                {
                    IdentityResult result = await userManager.AddToRoleAsync(user, UserRole);
                    if (result.Succeeded)
                    {
                        WriteLine($"User {user.UserName} added to {UserRole}");
                    }
                    else
                    {
                        foreach (IdentityError error in result.Errors)
                        {
                            WriteLine(error.Description);
                        }
                    }
                }
            

            return Redirect("/");
        }
        
    }
}
