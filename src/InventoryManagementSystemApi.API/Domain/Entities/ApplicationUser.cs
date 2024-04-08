using Microsoft.AspNetCore.Identity;

namespace InventoryManagementSystemApi.API.Domain.Entities;

public class ApplicationUser : IdentityUser<int>
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
}