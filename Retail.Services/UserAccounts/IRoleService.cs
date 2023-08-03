using Retail.DTOs;
using Retail.DTOs.Roles;
using Retail.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retail.Services.UserAccounts;


public interface IRoleService
{

    /// <summary>
    /// gets the Role details by Id
    /// </summary>
    /// <param name="id">Role Id</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns> Role Infromation</returns>
    Task<ResultDto<RoleResponseDto>> GetRoleById(string id, CancellationToken ct);


    /// <summary>
    /// Creates a new role
    /// </summary>
    /// <param name="roleDto">role</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<ResultDto<RoleResponseDto>> InsertRole(RoleDto roleDto, CancellationToken ct = default);

    /// <summary>
    /// Updates the existing role
    /// </summary>
    /// <param name="id">roleId</param>
    /// <param name="roleDto">role</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<ResultDto<RoleResponseDto>> UpdateRole(string roleId, RoleDto roleDto, CancellationToken ct = default);

    /// <summary>
    /// Deletes an existing role
    /// </summary>
    /// <param name="id">roleID</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns></returns>
    Task<ResultDto<bool>> DeleteRole(string id, CancellationToken ct = default);

    /// <summary>
    /// gets all the roles
    /// </summary>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task<PaginationResultDto<PagedList<RoleResponseDto>>> GetAllRoles(string keyword = null, int pageIndex = 0, int pageSize = 0, CancellationToken ct = default);


}

