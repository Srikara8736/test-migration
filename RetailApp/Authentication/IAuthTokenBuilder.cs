using Retail.DTOs;
using Retail.DTOs.UserAccounts;

namespace RetailApp.Authentication;

/// <summary>
/// AuthTokenBuilder Interface
/// </summary>
public interface IAuthTokenBuilder
{
    /// <summary>
    /// Generates AuthToken
    /// </summary>
    /// <param name="user">Login Request DTO</param>
    /// <returns>JWT Token</returns>
    Task<ResultDto<JWTTokenResponseDTO>> AuthTokenGeneration(LoginRequestDTO user, string ipAddress);

    /// <summary>
    ///generates Refresh Auth Token
    /// </summary>
    /// <param name="userToken">JWT Token</param>
    /// <returns>JWT Refresh Token</returns>
    ResultDto<JWTTokenResponseDTO> RefreshAuthToken(JWTTokenRequestDTO userToken);
}

