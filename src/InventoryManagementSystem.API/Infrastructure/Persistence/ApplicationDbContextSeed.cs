using InventoryManagementSystem.API.Domain.Entities;

using Microsoft.AspNetCore.Identity;

namespace InventoryManagementSystem.API.Infrastructure.Persistence;

public static class ApplicationDbContextSeed
{
    public static async Task SeedData(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        if (!userManager.Users.Any())
        {
            var users = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    FirstName = "Bob",
                    LastName = "Smith",
                    UserName = "bob",
                    Email = "bob@test.com"
                },
                new ApplicationUser
                {
                    FirstName = "Jane",
                    LastName = "Doe",
                    UserName = "jane",
                    Email = "jane@test.com"
                }
            };

            foreach (var user in users)
            {
                await userManager.CreateAsync(user, "Pa$$w0rd");
            }

            await context.SaveChangesAsync();
        }
    }
}