using Retail.DTOs.Customers;

namespace Retail.DTOs.UserAccounts;

public class UserResponseDto : UserDto
{
    /// <summary>
    /// Gets or sets the User ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the Role Name 
    /// </summary>
    public string RoleName { get; set; }

    /// <summary>
    /// Gets or sets the Customer Name 
    /// </summary>
    public CustomerResponseDto? Customer { get; set; }

}

