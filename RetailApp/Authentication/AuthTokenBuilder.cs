using Microsoft.IdentityModel.Tokens;
using Retail.DTOs.UserAccounts;
using Retail.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Retail.Services.UserAccounts;
using Retail.Services.Common;

namespace RetailApp.Authentication;

public class AuthTokenBuilder : IAuthTokenBuilder
{
    #region Fields

    private readonly IConfiguration _configuration;
    private readonly IUserService _userService;

    #endregion

    #region Ctor
    public AuthTokenBuilder(IConfiguration configuration, IUserService userService)
    {
        _configuration = configuration;
        _userService = userService;

    }

    #endregion


    #region  Utilities

    /// <summary>
    /// Creates a User Token
    /// </summary>
    /// <param name="authClaims">Auth Claims</param>
    /// <returns>List Claims</returns>
    public JwtSecurityToken CreateToken(List<Claim> authClaims)
    {

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
        //_ = int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JWT:TokenValidityInMinutes"])),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

        return token;
    }

    /// <summary>
    /// Generates Refresh token
    /// </summary>
    /// <returns>Refresh Token/returns>
    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    /// <summary>
    /// gets principal from expired token
    /// </summary>
    /// <param name="token">Token</param>
    /// <returns>Principal Claim</returns>
    /// <exception cref="SecurityTokenException">Invalid Token</exception>
    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException(StringResources.InvalidToken);

        return principal;

    }

    //For temporary use only
    /// <summary>
    /// gets the User Role
    /// </summary>
    /// <param name="username">User Name</param>
    /// <returns>User Roles</returns>
    public async Task<string> GetUserRoles(Guid userId, CancellationToken ct = default)
    {

        var userRole = "Admin";
        return userRole;
    }
    #endregion

    #region Methods

    /// <summary>
    /// Generates Auth Token
    /// </summary>
    /// <param name="user">Login Request DTO</param>
    /// <returns>JWT Token</returns>
    public async Task<ResultDto<JWTTokenResponseDTO>> AuthTokenGeneration(LoginRequestDTO user, string ipAddress)
    {
        if (user is null)
        {
            var UserResult = new ResultDto<JWTTokenResponseDTO>()
            {
                ErrorMessage = StringResources.NoResultsFound,
                StatusCode = HttpStatusCode.NotFound
            };
            return UserResult;

        }



        UserDto userRequestDto = new UserDto
        {
            UserName = user.UserName,
            PasswordHash = user.Password,
        };

        CancellationToken ct = new CancellationToken();

        var userDetails = await _userService.GetUserAuthByUser(userRequestDto, ct);

        if (userDetails.Data == null)
        {
            var UserResult = new ResultDto<JWTTokenResponseDTO>()
            {
                ErrorMessage = userDetails.ErrorMessage,
                StatusCode = userDetails.StatusCode
            };
            return UserResult;
        }


        var authClaims = new List<Claim>
        {

            new Claim("UserId", userDetails.Data.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, userDetails.Data.RoleName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };


        var token = CreateToken(authClaims);
        var refreshToken = GenerateRefreshToken();

        var userInfo = new UserInformation()
        {
            UserName = userDetails.Data.UserName,
            Email = userDetails.Data.Email,
            Role = userDetails.Data.RoleName,
            Id = userDetails.Data.Id,

        };

        var authDetails = new JWTTokenResponseDTO()
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = refreshToken,
            User = userInfo,
            Customer = userDetails.Data.Customer != null ? userDetails.Data.Customer : null,
        };



        var result = new ResultDto<JWTTokenResponseDTO>()
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Data = authDetails
        };
        return result;

    }


    /// <summary>
    /// Refreshes Auth Token
    /// </summary>
    /// <param name="userToken">User Token</param>
    /// <returns>Refresh Auth Token</returns>
    public ResultDto<JWTTokenResponseDTO> RefreshAuthToken(JWTTokenRequestDTO userToken)
    {
        if (userToken is null)
        {
            var UserResult = new ResultDto<JWTTokenResponseDTO>()
            {
                ErrorMessage = StringResources.InvalidArgument,
                StatusCode = HttpStatusCode.BadRequest
            };
            return UserResult;

        }

        string? accessToken = userToken.AccessToken;
        string? refreshToken = userToken.RefreshToken;

        var principal = GetPrincipalFromExpiredToken(accessToken);

        if (principal == null)
        {
            var UserResult = new ResultDto<JWTTokenResponseDTO>()
            {
                ErrorMessage = StringResources.InvalidToken,
                StatusCode = HttpStatusCode.NotFound
            };
            return UserResult;
        }
        var userId = principal.Claims.Where(x => x.Type == "UserId").FirstOrDefault();

        CancellationToken ct = new CancellationToken();

        var userDetails = _userService.GetUserById(new Guid(userId.Value), ct).Result;


        var newAccessToken = CreateToken(principal.Claims.ToList());
        var newRefreshToken = GenerateRefreshToken();

        var userInfo = new UserInformation()
        {
            Email = userDetails.Data.Email,
            Role = userDetails.Data.RoleName,
            Id = userDetails.Data.Id

        };

        var authDetails = new JWTTokenResponseDTO()
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
            RefreshToken = newRefreshToken,
            User = userInfo,
            Customer = userDetails.Data.Customer != null ? userDetails.Data.Customer : null,
        };
        var result = new ResultDto<JWTTokenResponseDTO>()
        {
            StatusCode = HttpStatusCode.OK,
            Data = authDetails
        };
        return result;
    }
    #endregion
}
