using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Retail.DTOs.UserAccounts;
using Retail.Services.UserAccounts;
using RetailApp.Helpers;

namespace RetailApp.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UserController : BaseController
{

    #region Fields

    private readonly IUserService _userService;

    #endregion

    #region Ctor
    public UserController(IUserService userService)
    {
        _userService = userService;

    }

    #endregion


    #region Methods

    /// <summary>
    /// Get All User optionally with paged List
    /// </summary>
    ///  <param name="parameters">Filter and Pageing Parameters</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Return User optionally with paged List Response</returns>
    [HttpGet]
    [Route("Users")]
    public async Task<IActionResult> GetAllUsers([FromQuery] CustomerParam parameters, CancellationToken ct = default)
    {
        return this.Result(await _userService.GetAllUsers(parameters.CustomerId, parameters.Keyword, parameters.PageIndex, parameters.PageSize, ct));
    }

    /// <summary>
    /// Get a User by Identifier
    /// </summary>
    /// <param name="id">Identifier</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Return User Response</returns>
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetUserById(string id, CancellationToken ct)
    {
        return this.Result(await _userService.GetUserById(new Guid(id), ct));

    }

    /// <summary>
    /// Add a new User item
    /// </summary>
    /// <param name="userModel">User Information</param>  
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Return newly added User</returns>
    [HttpPost]
    public async Task<IActionResult> AddUser([FromBody] UserDto userModel, CancellationToken ct = default)
    {

        return this.Result(await _userService.RegisterUser(userModel, ct));

    }

    /// <summary>
    /// Update a User item
    /// </summary>
    /// <param name="id">id</param>
    /// <param name="userModel">User infromation</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Return updated User Information</returns>
    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> UpdateUser(string id, [FromBody] UserDto userModel, CancellationToken ct)
    {
        return this.Result(await _userService.UpdateUser(new Guid(id), userModel, ct));
    }

    /// <summary>
    /// Update a User Password
    /// </summary>
    /// <param name="id">id</param>
    /// <param name="Password">User Password Details</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Return Success Statement</returns>
    [HttpPost]
    [Route("UpdatePassword/{id}")]
    public async Task<IActionResult> UpdateApprovedStatus(string id, [FromBody] PasswordDto Password, CancellationToken ct = default)
    {

        return this.Result(await _userService.UpdateUserPassword(new Guid(id), Password, ct));

    }


    /// <summary>
    /// Delete a User item
    /// </summary>
    /// <param name="id">id</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Return Deleted Status</returns>
    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteUser(string id, CancellationToken ct)
    {
        return this.Result(await _userService.DeleteUser(new Guid(id), ct));

    }

    #endregion

}

