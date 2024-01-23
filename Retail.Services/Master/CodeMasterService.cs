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
using Retail.DTOs.Master;
using Retail.Data.Entities.Common;
using Retail.DTOs.Customers;

namespace Retail.Services.Master;

public class CodeMasterService : ICodeMasterService
{
    #region Fields

    private readonly RepositoryContext _repositoryContext;
    private readonly IMapper _mapper;

    #endregion

    #region Ctor
    public CodeMasterService(RepositoryContext repositoryContext, IMapper mapper)
    {
        _repositoryContext = repositoryContext;
        _mapper = mapper;
    }
    #endregion

    #region Utilities

    /// <summary>
    /// Validate Status Is Already Exists
    /// </summary>
    /// <param name="role">Status</param>
    /// <returns>Return Frue / False</returns>
    public bool ValidateStatus(string status, string type)
    {
        bool isExists = false;

        if (status != null && status != "")
            isExists = _repositoryContext.CodeMasters.Any(x => (x.Value.ToLower().Trim() == status.ToLower().Trim()) && (x.Type.ToLower().Trim() == type.ToLower().Trim()));

        return isExists;


    }

    #endregion

    #region Methods

    /// <summary>
    /// gets the Status details by Id
    /// </summary>
    /// <param name="id">Status Id</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns> Status Infromation</returns>
    public async Task<ResultDto<CodeMasterResponseDto>> GetStatusById(Guid id, CancellationToken ct)
    {
        if (id == null)
        {
            var UserResult = new ResultDto<CodeMasterResponseDto>
            {
                ErrorMessage = StringResources.InvalidArgument,
                IsSuccess = false
            };
            return UserResult;
        }
        var role = await _repositoryContext.CodeMasters.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (role == null)
        {
            var UserResult = new ResultDto<CodeMasterResponseDto>()
            {
                ErrorMessage = StringResources.NoResultsFound,
                IsSuccess = false
            };
            return UserResult;
        }

        var roleResponse = _mapper.Map<CodeMasterResponseDto>(role);
        var response = new ResultDto<CodeMasterResponseDto>
        {
            IsSuccess = true,
            Data = roleResponse
        };
        return response;
    }



    /// <summary>
    /// Creates a new Status
    /// </summary>
    /// <param name="StatusDto">Status</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public async Task<ResultDto<CodeMasterResponseDto>> InsertStatus(CodeMasterDto StatusDto, CancellationToken ct = default)
    {
        if (StatusDto == null)
        {
            var errorResponse = new ResultDto<CodeMasterResponseDto>
            {
                ErrorMessage = StringResources.RecordNotFound,
                IsSuccess = false
            };
            return errorResponse;
        }

        var isExists = ValidateStatus(StatusDto.Value, StatusDto.Type);

        if (isExists)
        {
            var errorResponse = new ResultDto<CodeMasterResponseDto>
            {
                ErrorMessage = StringResources.RecordExists,
                IsSuccess= false

            };
            return errorResponse;
        }

        var codeMaster = _mapper.Map<CodeMaster>(StatusDto);

        await _repositoryContext.CodeMasters.AddAsync(codeMaster, ct);
        await _repositoryContext.SaveChangesAsync(ct);


        var codeMasterResponse = _mapper.Map<CodeMasterResponseDto>(codeMaster);


        var result = new ResultDto<CodeMasterResponseDto>
        {
            Data = codeMasterResponse,
            IsSuccess = true
        };

        return result;

    }

    /// <summary>
    /// Updates the existing Status
    /// </summary>
    /// <param name="id">StatusId</param>
    /// <param name="StatusDto">Status</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public async Task<ResultDto<CodeMasterResponseDto>> UpdateStatus(Guid StatusId, CodeMasterDto StatusDto, CancellationToken ct = default)
    {
        if (StatusDto == null)
        {
            var errorResponse = new ResultDto<CodeMasterResponseDto>
            {
                ErrorMessage = StringResources.RecordNotFound,
                IsSuccess= false
            };
            return errorResponse;
        }


        var isRoleExists = _repositoryContext.CodeMasters.Any(x => x.Value == StatusDto.Value && x.Id != StatusId);
        if (isRoleExists == true)
        {
            var errorResponse = new ResultDto<CodeMasterResponseDto>
            {
                ErrorMessage = StringResources.BadRequest,
                StatusCode = HttpStatusCode.BadRequest
            };
            return errorResponse;
        }
        var roleResult = await _repositoryContext.CodeMasters.FirstOrDefaultAsync(x => x.Id == StatusId, ct);

        var role = _mapper.Map(StatusDto, roleResult);
        await _repositoryContext.SaveChangesAsync(ct);


        var roleResponse = _mapper.Map<CodeMasterResponseDto>(role);


        var result = new ResultDto<CodeMasterResponseDto>
        {
            Data = roleResponse,
            IsSuccess = true
        };
        return result;

    }

    /// <summary>
    /// Delete an existing Status
    /// </summary>
    /// <param name="id">StatusID</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns></returns>
    public async Task<ResultDto<bool>> DeleteStatus(Guid id, CancellationToken ct = default)
    {

        var codeMaster = await _repositoryContext.CodeMasters.FirstOrDefaultAsync(x => x.Id == id, ct);

        if (codeMaster == null)
        {
            var response = new ResultDto<bool>
            {
                ErrorMessage = StringResources.RecordNotFound,
                IsSuccess= false
            };
            return response;

        }

        _repositoryContext.CodeMasters.Remove(codeMaster);
        await _repositoryContext.SaveChangesAsync(ct);


        var successResponse = new ResultDto<bool>
        {
            IsSuccess = true,
            Data = true
        };

        return successResponse;

    }

    /// <summary>
    /// gets all the Status
    /// </summary>.
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public virtual async Task<PaginationResultDto<PagedList<CodeMasterResponseDto>>> GetAllStatus(string keyword = null, int pageIndex = 0, int pageSize = 0, CancellationToken ct = default)
    {

        var query = from p in _repositoryContext.CodeMasters
                    select p ;

        if (keyword != null)
        {
            query = query.Where(x => x.Value.Contains(keyword));
        }

        var codeMaster = await PagedList<CodeMaster>.ToPagedList(query.OrderBy(on => on.Order), pageIndex, pageSize);
        if (codeMaster == null)
        {
            var errorResponse = new PaginationResultDto<PagedList<CodeMasterResponseDto>>
            {
                ErrorMessage = StringResources.NoResultsFound,
                StatusCode = HttpStatusCode.NoContent
            };
            return errorResponse;
        }

        var rolesResponse = _mapper.Map<PagedList<CodeMasterResponseDto>>(codeMaster);
        var response = new PaginationResultDto<PagedList<CodeMasterResponseDto>>
        {
            IsSuccess = true,
            Data = rolesResponse,
            TotalCount = codeMaster.TotalCount,
            TotalPages = codeMaster.TotalPages
        };
        return response;
    }





    /// <summary>
    /// gets all the Status
    /// </summary>.
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public virtual async Task<PaginationResultDto<PagedList<CodeMasterResponseDto>>> GetAllStoreStatus(string keyword = null, int pageIndex = 0, int pageSize = 0, CancellationToken ct = default)
    {

        var query = from p in _repositoryContext.CodeMasters
                    where p.Type.ToLower().Trim() =="storestatus"
                    select p;

        if (keyword != null)
        {
            query = query.Where(x => x.Value.Contains(keyword));
        }

        var codeMaster = await PagedList<CodeMaster>.ToPagedList(query.OrderBy(on => on.Order), pageIndex, pageSize);
        if (codeMaster == null)
        {
            var errorResponse = new PaginationResultDto<PagedList<CodeMasterResponseDto>>
            {
                ErrorMessage = StringResources.NoResultsFound,
                StatusCode = HttpStatusCode.NoContent
            };
            return errorResponse;
        }

        var rolesResponse = _mapper.Map<PagedList<CodeMasterResponseDto>>(codeMaster);
        var response = new PaginationResultDto<PagedList<CodeMasterResponseDto>>
        {
            IsSuccess = true,
            Data = rolesResponse,
            TotalCount = codeMaster.TotalCount,
            TotalPages = codeMaster.TotalPages
        };
        return response;
    }

    #endregion
}


