using InventoryManagementSystemApi.API.Domain.Entities;

using Microsoft.AspNetCore.Identity;

namespace InventoryManagementSystemApi.API.Infrastructure.Persistence;

public static class ApplicationDbContextSeed
{
    public static async Task SeedData(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
    }
}