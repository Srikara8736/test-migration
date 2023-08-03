namespace Retail.DTOs.UserAccounts;

/// <summary>
/// Represents a JWT Token Response
/// </summary>
public class JWTTokenRequestDTO
{
    /// <summary>
    /// Gets or sets the Is AccessToken
    /// </summary>
    public string? AccessToken { get; set; }

    /// <summary>
    /// Gets or sets the Is RefreshToken
    /// </summary>
    public string? RefreshToken { get; set; }


}
