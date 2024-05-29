using InventoryManagementSystem.API.Domain.Entities;
using InventoryManagementSystem.API.Infrastructure.Services;

using Microsoft.AspNetCore.Identity;

namespace InventoryManagementSystem.API.Features.Auth;

public class TokensResult
{
    public int Id { get; set; }
    public string? Email { get; set; }
    public string? Name { get; set; }
    public string AccessToken { get; set; } = null!;
    
    public string RefreshToken { get; set; } = null!;

    public async Task<TokensResult> GenerateTokens(TokenService tokenService, UserManager<ApplicationUser> userManager, ApplicationUser user)
    {
        var refreshToken = tokenService.GenerateRefreshToken();

        user.RefreshTokens.Add(refreshToken);
        await userManager.UpdateAsync(user);

        return new TokensResult
        {
            Id = user.Id,
            Email = user.Email,
            Name = $"{user.FirstName} {user.LastName}",
            AccessToken = tokenService.CreateToken(user),
            RefreshToken = refreshToken.Token
        };
    }
}