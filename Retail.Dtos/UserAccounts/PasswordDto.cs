using System.ComponentModel.DataAnnotations;

namespace Retail.DTOs.UserAccounts;

public class PasswordDto
{
    [Required]
    public string CurrentPassword { get; set; }


    [Required]
    public string NewPassword { get; set; }
  
}
