using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ZenithWebSite.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ZenithWebSite.Models
{
    public static class RolesData
    {
        public static async void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<ApplicationDbContext>();
            var roleStore = new RoleStore<ApplicationRole>(context);

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                await roleStore.CreateAsync(new ApplicationRole {
                    Name = "Admin",
                    Description = "Administartor",
                    CreatedDate = DateTime.Now,
                    NormalizedName = "ADMIN"
                });
            }

            if (!context.Roles.Any(r => r.Name == "Member"))
            {
                await roleStore.CreateAsync(new ApplicationRole
                {
                    Name = "Member",
                    Description = "Members",
                    CreatedDate = DateTime.Now,
                    NormalizedName = "MEMBER"
                });
            }

            var admin = new ApplicationUser
            {
                FirstName = "AAA",
                LastName = "AAA",
                Email = "a@a.a",
                NormalizedEmail = "A@A.A",
                UserName = "a",
                NormalizedUserName = "A",
                PhoneNumber = "604-417-1573",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,    
                SecurityStamp = Guid.NewGuid().ToString("D")
            };
            
            //await context.SaveChangesAsync();

            var member = new ApplicationUser
            {
                FirstName = "BBB",
                LastName = "BBB",
                Email = "m@m.m",
                NormalizedEmail = "M@M.M",
                UserName = "m",
                NormalizedUserName = "M",
                PhoneNumber = "604-417-1574",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };

            if (!context.Users.Any(u => u.UserName == admin.UserName))
            {
                var password = new PasswordHasher<ApplicationUser>();
                var hashed = password.HashPassword(admin, "P@$$w0rd");
                admin.PasswordHash = hashed;

                var userStore = new UserStore<ApplicationUser>(context);
                var result = userStore.CreateAsync(admin);
            }

            if (!context.Users.Any(u => u.UserName == member.UserName))
            {
                var password = new PasswordHasher<ApplicationUser>();
                var hashed = password.HashPassword(member, "P@$$w0rd");
                member.PasswordHash = hashed;

                var userStore = new UserStore<ApplicationUser>(context);
                var result = userStore.CreateAsync(member);
                await AssignRoles(serviceProvider, admin.UserName, "Admin");
                await AssignRoles(serviceProvider, member.UserName, "Member");
            }

            await context.SaveChangesAsync();
        }

        public static async Task<IdentityResult> AssignRoles(IServiceProvider services, string username, string role)
        {
            UserManager<ApplicationUser> _userManager = services.GetService<UserManager<ApplicationUser>>();
            ApplicationUser user = await _userManager.FindByNameAsync(username);
            var result = await _userManager.AddToRoleAsync(user, role);

            return result;
        }
    }
}