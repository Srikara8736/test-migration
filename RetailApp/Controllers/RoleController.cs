using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Retail.DTOs.Roles;
using Retail.Services.UserAccounts;
using RetailApp.Helpers;

namespace RetailApp.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class RoleController : BaseController
{

    #region Fields

    private readonly IRoleService _roleService;

    #endregion

    #region Ctor
    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;

    }

    #endregion


    #region Methods

    /// <summary>
    ///Get All Role optionally with paged List
    /// </summary>
    /// <param name="Keyword">Search keyword by Role Name</param>
    /// <param name="PageIndex">PageIndex</param>
    /// <param name="PageSize">PageSize</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Return Role optionally with paged List Response</returns>
    [HttpGet]
    [Route("Roles")]
    public async Task<IActionResult> getAllRoles([FromQuery] PagingParam parameters, CancellationToken ct = default)
    {

        return this.Result(await _roleService.GetAllRoles(parameters.Keyword, parameters.PageIndex, parameters.PageSize, ct));
    }


    /// <summary>
    ///Get a Role details by Identifier
    /// </summary>
    /// <param name="id">Identifier</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Return Role Response</returns>
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetRoleById(string id, CancellationToken ct)
    {

        return this.Result(await _roleService.GetRoleById(id, ct));

    }

    /// <summary>
    ///Add a new Role item
    /// </summary>
    /// <param name="name">Role Name</param>
    /// <param name="description">Role Description</param>
    /// <param name="isActive">Active Status</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Return newly added Role</returns>
    [HttpPost]
    public async Task<IActionResult> AddRole([FromBody] RoleDto roleModel, CancellationToken ct = default)
    {

        return this.Result(await _roleService.InsertRole(roleModel, ct));

    }

    /// <summary>
    /// Update a Role Details
    /// </summary>
    /// <param name="name">Role Name</param>
    /// <param name="description">Role Description</param>
    /// <param name="isActive">Active Status</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Return updated Role Information</returns>
    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> UpdateRole(string id, [FromBody] RoleDto roleModel, CancellationToken ct)
    {
        return this.Result(await _roleService.UpdateRole(id, roleModel, ct));
    }


    /// <summary>
    /// Delete a Role item
    /// </summary>
    /// <param name="id">id</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Return Deleted Status</returns>
    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteRole(string id, CancellationToken ct)
    {
        return this.Result(await _roleService.DeleteRole(id, ct));

    }

    #endregion

}
