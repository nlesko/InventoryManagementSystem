using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.WebUI.Entities.DTO;

public class UserForAuthenticationDto
{
    [Required(ErrorMessage = "Email is required.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; } = string.Empty;
}