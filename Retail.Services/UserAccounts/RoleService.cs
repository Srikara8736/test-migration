using Retail.DTOs.Roles;
using Retail.DTOs;
using Retail.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Retail.Data.Repository;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Retail.Data.Entities.UserAccount;

namespace Retail.Services.UserAccounts;

public class RoleService : IRoleService
{
    #region Fields

    private readonly RepositoryContext _repositoryContext;
    private readonly IMapper _mapper;

    #endregion

    #region Ctor
    public RoleService(RepositoryContext repositoryContext, IMapper mapper)
    {
        _repositoryContext = repositoryContext;
        _mapper = mapper;
    }
    #endregion


    #region Utilities

    /// <summary>
    /// Validate Role Is Already Exists
    /// </summary>
    /// <param name="role">role</param>
    /// <returns>Return Frue / False</returns>
    public bool ValidateRole(string role)
    {
        bool isExists = false;

        if (role != null && role != "")
            isExists = _repositoryContext.Roles.Any(x => x.Name.ToLower().Trim() == role.ToLower().Trim());

        return isExists;


    }

    #endregion

    #region Methods

    /// <summary>
    /// gets the role details by User Id
    /// </summary>
    /// <param name="roleId">Role Id</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Role Details</returns>
    public async Task<ResultDto<RoleResponseDto>> GetRoleById(string id, CancellationToken ct)
    {
        if (id == null)
        {
            var UserResult = new ResultDto<RoleResponseDto>
            {
                ErrorMessage = StringResources.InvalidArgument,
                StatusCode = HttpStatusCode.BadRequest
            };
            return UserResult;
        }
        var role = await _repositoryContext.Roles.FirstOrDefaultAsync(x => x.Id == new Guid(id), ct);
        if (role == null)
        {
            var UserResult = new ResultDto<RoleResponseDto>()
            {
                ErrorMessage = StringResources.NoResultsFound,
                StatusCode = HttpStatusCode.NotFound
            };
            return UserResult;
        }

        var roleResponse = _mapper.Map<RoleResponseDto>(role);
        var response = new ResultDto<RoleResponseDto>
        {
            IsSuccess = true,
            Data = roleResponse
        };
        return response;
    }



    /// <summary>
    /// Inserts Role 
    /// </summary>
    /// <param name="roleDto">Role</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns></returns>
    public async Task<ResultDto<RoleResponseDto>> InsertRole(RoleDto roleDto, CancellationToken ct = default)
    {
        if (roleDto == null)
        {
            var errorResponse = new ResultDto<RoleResponseDto>
            {
                ErrorMessage = StringResources.RecordNotFound,
                StatusCode = HttpStatusCode.NotFound
            };
            return errorResponse;
        }

        var isExists = ValidateRole(roleDto.Name);

        if (isExists)
        {
            var errorResponse = new ResultDto<RoleResponseDto>
            {
                ErrorMessage = StringResources.RecordExists,
                StatusCode = HttpStatusCode.Conflict

            };
            return errorResponse;
        }

        var role = _mapper.Map<Role>(roleDto);

        await _repositoryContext.Roles.AddAsync(role, ct);
        await _repositoryContext.SaveChangesAsync(ct);


        var roleResponse = _mapper.Map<RoleResponseDto>(role);


        var result = new ResultDto<RoleResponseDto>
        {
            Data = roleResponse,
            IsSuccess = true
        };

        return result;

    }

    /// <summary>
    /// Updates Role
    /// </summary>
    /// <param name="id">RoleId</param>
    /// <param name="roleDto">Role</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns></returns>
    public async Task<ResultDto<RoleResponseDto>> UpdateRole(string roleId, RoleDto roleDto, CancellationToken ct = default)
    {
        if (roleDto == null)
        {
            var errorResponse = new ResultDto<RoleResponseDto>
            {
                ErrorMessage = StringResources.RecordNotFound,
                StatusCode = HttpStatusCode.NotFound
            };
            return errorResponse;
        }


        var isRoleExists = _repositoryContext.Roles.Any(x => x.Name == roleDto.Name && x.Id != new Guid(roleId));
        if (isRoleExists == true)
        {
            var errorResponse = new ResultDto<RoleResponseDto>
            {
                ErrorMessage = StringResources.BadRequest,
                StatusCode = HttpStatusCode.BadRequest
            };
            return errorResponse;
        }
        var roleResult = await _repositoryContext.Roles.FirstOrDefaultAsync(x => x.Id == new Guid(roleId), ct);

        var role = _mapper.Map(roleDto, roleResult);
        await _repositoryContext.SaveChangesAsync(ct);


        var roleResponse = _mapper.Map<RoleResponseDto>(role);


        var result = new ResultDto<RoleResponseDto>
        {
            Data = roleResponse,
            IsSuccess = true
        };
        return result;

    }

    /// <summary>
    /// Delete Role
    /// </summary>
    /// <param name="id">RoleID</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns></returns>
    public async Task<ResultDto<bool>> DeleteRole(string id, CancellationToken ct = default)
    {

        var role = await _repositoryContext.Roles.FirstOrDefaultAsync(x => x.Id == new Guid(id), ct);

        if (role == null)
        {
            var response = new ResultDto<bool>
            {
                ErrorMessage = StringResources.RecordNotFound,
                StatusCode = HttpStatusCode.NotFound
            };
            return response;

        }

        _repositoryContext.Roles.Remove(role);
        await _repositoryContext.SaveChangesAsync(ct);


        var successResponse = new ResultDto<bool>
        {
            IsSuccess = true,
            Data = true
        };

        return successResponse;

    }

    /// <summary>
    /// Get all Roles
    /// </summary>
    /// <param name="pageIndex">PageIndex</param>
    /// <param name="pageSize">PageSize</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns></returns>
    public virtual async Task<PaginationResultDto<PagedList<RoleResponseDto>>> GetAllRoles(string keyword = null, int pageIndex = 0, int pageSize = 0, CancellationToken ct = default)
    {

        var query = from p in _repositoryContext.Roles
                    where p.Name != "Super Admin"
                    select p ;

        if (keyword != null)
        {
            query = query.Where(x => x.Name.Contains(keyword));
        }

        var roles = await PagedList<Role>.ToPagedList(query.OrderBy(on => on.Name), pageIndex, pageSize);
        if (roles == null)
        {
            var errorResponse = new PaginationResultDto<PagedList<RoleResponseDto>>
            {
                ErrorMessage = StringResources.NoResultsFound,
                StatusCode = HttpStatusCode.NoContent
            };
            return errorResponse;
        }

        var rolesResponse = _mapper.Map<PagedList<RoleResponseDto>>(roles);
        var response = new PaginationResultDto<PagedList<RoleResponseDto>>
        {
            IsSuccess = true,
            Data = rolesResponse,
            TotalCount = roles.TotalCount,
            TotalPages = roles.TotalPages
        };
        return response;
    }

    #endregion
}


