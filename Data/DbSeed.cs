using EmployeePerformance_AttendenceTracking.Models;
using Microsoft.AspNetCore.Identity;

namespace EmployeePerformance_AttendenceTracking.Data
{
    public static class DbSeed
    {
        public static async Task SeedDataAsync(UserManager<ApplicationUser> userManager,RoleManager<IdentityRole>roleManager)
        {
            string[] roles = { "Admin", "Manager", "Employee" };
            foreach(var r in roles)
            {
                if(!await roleManager.RoleExistsAsync(r))
                    await roleManager.CreateAsync(new IdentityRole(r));
            }

            string adminEmail = "Kumarhk890L@gmail.com";
            string adminPassword = "Kumar@12345";

            var adminuser = await userManager.FindByEmailAsync(adminEmail);
            if(adminuser==null)
            {
                adminuser = new ApplicationUser
                {
                    EmployeeId=1,
                    DepartmentId=4,
                    Email = adminEmail,
                    UserName=adminEmail,
                    FullName = "System Admin",
                    JoinDate = DateTime.Now
                };
                var result = await userManager.CreateAsync(adminuser, adminPassword);
                if(result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminuser, "Admin");
                }
            }

            var managerEmail = "Manager123@gmail.com";
            var managerPass = "Manager@123";

            var manager = await userManager.FindByEmailAsync(managerEmail);
            if(manager==null)
            {
                manager = new ApplicationUser
                {
                    EmployeeId=2,
                    DepartmentId=4,
                    Email = managerEmail,
                    UserName=managerEmail,
                    FullName = "Defaul Manager",
                    Salary=35000,
                    JoinDate=DateTime.UtcNow
                };
                var result = await userManager.CreateAsync(manager, managerPass);
                if(result.Succeeded)
                {
                    await userManager.AddToRoleAsync(manager, "Manager");
                }
            }

        }
    }
}
