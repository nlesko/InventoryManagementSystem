using Microsoft.AspNetCore.Identity;

namespace InventoryManagementSystem.API.Domain.Entities;

public class ApplicationUser : IdentityUser<int>
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    #region Navigation Properties
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    #endregion
}