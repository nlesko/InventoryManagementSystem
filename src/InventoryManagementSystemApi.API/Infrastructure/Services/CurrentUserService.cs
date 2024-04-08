using System.Security.Claims;

using InventoryManagementSystemApi.API.Common.Interfaces;

namespace InventoryManagementSystemApi.API.Infrastructure.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    public string? Username => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
    public int? UserId => ParseContextStringToInt(_httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier));

    private static int? ParseContextStringToInt(string? claimType)
    {
        bool isParsed = int.TryParse(claimType, out int id);

        if (isParsed)
        {
            return id;
        }

        return null;
    }

}