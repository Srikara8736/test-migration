using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Retail.DTOs.UserAccounts;
using RetailApp.Authentication;

namespace RetailApp.Controllers;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : BaseController
{
    #region Fields

    private readonly IAuthTokenBuilder _authTokenBuilder;
    private readonly IHttpContextAccessor _httpContextAccessor;

    #endregion

    #region Ctor
    public AuthenticationController(IAuthTokenBuilder authTokenBuilder, IHttpContextAccessor httpContextAccessor)
    {
        _authTokenBuilder = authTokenBuilder;
        _httpContextAccessor = httpContextAccessor;
    }

    #endregion

    #region Methods

    /// <summary>
    /// User provides email and password for the authentication
    /// </summary>
    /// <param name="userLogin">User Login Model</param>
    /// <returns>Return Json with JWT response when login success</returns>

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO userLogin)
    {
        var ip = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();

        return this.Result(await _authTokenBuilder.AuthTokenGeneration(userLogin, ip));
    }

    /// <summary>
    /// Refresh the access token for Keep alive the login Session
    /// </summary>
    /// <param name="tokenModel">Token Object Model</param>
    /// <returns>Return Json with Regenerated JWT response with Newly Generated Token</returns>

    [HttpPost]
    [Route("refresh-token")]
    public IActionResult RefreshToken(JWTTokenRequestDTO tokenModel)
    {
        return this.Result(_authTokenBuilder.RefreshAuthToken(tokenModel));

    }

    #endregion
}
