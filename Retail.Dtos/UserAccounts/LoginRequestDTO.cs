using System.ComponentModel.DataAnnotations;

namespace Retail.DTOs.UserAccounts;


/// <summary>
/// Represents a Login Request DTO
/// </summary>
public class LoginRequestDTO
{
    /// <summary>
    /// Gets or sets the Is UserName
    /// </summary>
    [Required(ErrorMessage = "User Name is required")]
    public string Username { get; set; }

    /// <summary>
    /// Gets or sets the Is Password
    /// </summary>

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
}
