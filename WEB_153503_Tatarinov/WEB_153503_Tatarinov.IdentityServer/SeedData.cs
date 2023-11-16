using System.Security.Claims;
using IdentityModel;
using WEB_153503_Tatarinov.IdentityServer.Data;
using WEB_153503_Tatarinov.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace WEB_153503_Tatarinov.IdentityServer;

public class SeedData
{
    public static async Task EnsureSeedData(WebApplication app)
    {
        using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            context.Database.Migrate();

            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            
            var adminRoleExists = await roleMgr.RoleExistsAsync(Config.Admin);
            if (!adminRoleExists)
            {
                var adminRole = new IdentityRole(Config.Admin);
                await roleMgr.CreateAsync(adminRole);
            }

            var userRoleExists = await roleMgr.RoleExistsAsync(Config.User);
            if (!userRoleExists)
            {
                var userRole = new IdentityRole(Config.User);
                await roleMgr.CreateAsync(userRole);
            }

            
            var user = await userMgr.FindByNameAsync(Config.User);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = "user",
                    Email = "",
                    EmailConfirmed = true,
                };
                var resultUser = await userMgr.CreateAsync(user, "User123$");
                if (!resultUser.Succeeded)
                {
                    throw new Exception(resultUser.Errors.First().Description);
                }

                resultUser = await userMgr.AddClaimsAsync(user, new Claim[]
                {
                    new Claim(JwtClaimTypes.Name, Config.User),
                    new Claim(JwtClaimTypes.GivenName, "user"),
                    new Claim(JwtClaimTypes.FamilyName, "userov"),
                    new Claim(JwtClaimTypes.WebSite, Config.User),
                });
                if (!resultUser.Succeeded)
                {
                    throw new Exception(resultUser.Errors.First().Description);
                }

                Log.Debug("user created");
            }
            else
            {
                Log.Debug("user already exists");
            }

            var admin = await userMgr.FindByNameAsync(Config.Admin);
            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@gmail.com",
                    EmailConfirmed = true
                };
                var resultAdmin = await userMgr.CreateAsync(admin, "Admin123$");
                if (!resultAdmin.Succeeded)
                {
                    throw new Exception(resultAdmin.Errors.First().Description);
                }

                resultAdmin = await userMgr.AddClaimsAsync(admin, new Claim[]
                {
                    new Claim(JwtClaimTypes.Name, Config.Admin),
                    new Claim(JwtClaimTypes.GivenName, "admin"),
                    new Claim(JwtClaimTypes.FamilyName, "adminov"),
                    new Claim(JwtClaimTypes.WebSite, Config.Admin),
                    new Claim("location", "somewhere")
                });
                if (!resultAdmin.Succeeded)
                {
                    throw new Exception(resultAdmin.Errors.First().Description);
                }

                Log.Debug("admin created");
            }
            else
            {
                Log.Debug("admin already exists");
            }
        }
    }
}