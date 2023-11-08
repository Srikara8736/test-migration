using Retail.DTOs.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retail.DTOs.UserAccounts;

/// <summary>
/// Represents a JWT Token Response
/// </summary>
public class JWTTokenResponseDTO
{
    /// <summary>
    /// Gets or sets the Is AccessToken
    /// </summary>
    public string? AccessToken { get; set; }

    /// <summary>
    /// Gets or sets the Is RefreshToken
    /// </summary>
    public string? RefreshToken { get; set; }

    /// <summary>
    /// Gets or sets the Is Expiration
    /// </summary>
    //public DateTime? Expiration { get; set; }

    public UserInformation User { get; set; }
    public CustomerResponseDto? Customer { get; set; }

}

public class UserInformation
{
    /// <summary>
    /// Gets or sets the Id
    /// </summary>
    public Guid Id { get; set; }

   

    /// <summary>
    /// Gets or sets the Email
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets the Role
    /// </summary>
    public string Role { get; set; }
}

