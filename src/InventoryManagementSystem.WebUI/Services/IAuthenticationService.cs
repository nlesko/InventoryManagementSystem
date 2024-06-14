using InventoryManagementSystem.WebUI.Entities.DTO;

namespace InventoryManagementSystem.WebUI.Services;

public interface IAuthenticationService
{
    Task<RegistrationResponseDto> RegisterUser(UserForRegistrationDto userForRegistration);
    Task<AuthResponseDto> Login(UserForAuthenticationDto userForAuthentication);
    Task Logout();
}