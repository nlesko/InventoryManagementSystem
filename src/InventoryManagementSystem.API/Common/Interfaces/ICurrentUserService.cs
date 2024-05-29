namespace InventoryManagementSystem.API.Common.Interfaces;

public interface ICurrentUserService
{
    string? Username { get; }
    int? UserId { get; }
}