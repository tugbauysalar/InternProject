using InternProject.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternProject.Persistence
{
    public class AppDbInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            string[] roleNames = { "Admin", "Team Lead" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new Role { Name = roleName });
                }
            }

            string adminPassword = "Admin.123";

            var admin = new User
            {
                NameSurname = "Tuğba Uysalar",
                UserName = "admin",
                Email = "admin@gmail.com",
                Password = adminPassword
            };

            
            var user = await userManager.FindByEmailAsync("admin@gmail.com");

            if (user == null)
            {
                var createAdmin = await userManager.CreateAsync(admin, adminPassword);
                if (createAdmin.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }
        }
    }
}
