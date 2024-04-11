namespace InventoryManagementSystemApi.API.Domain.Entities;

public class RefreshToken
{
    public int Id { get; set; }
    public ApplicationUser ApplicationUser { get; set; } = null!;
    public string Token { get; set; } = null!;
    public DateTime Expires { get; set; } = DateTime.UtcNow.AddDays(7);
    public bool IsExpired => DateTime.UtcNow >= Expires;
    public DateTime? Revoked { get; set; }
    public bool IsActive => Revoked == null && !IsExpired;
}