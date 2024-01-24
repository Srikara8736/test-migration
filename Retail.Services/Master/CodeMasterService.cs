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
    /// <param name="status">Status</param>
    /// <param name="type">type</param>
    /// <returns>Return Frue / False</returns>
    public bool ValidateStatus(string status, string type)
    {
        bool isExists = false;

        if (status != null && status != "")
            isExists = _repositoryContext.CodeMasters.Any(x => (x.Value.ToLower().Trim() == status.ToLower().Trim()) && (x.Type.ToLower().Trim() == type.ToLower().Trim()));

        return isExists;


    }


    /// <summary>
    /// Validate Customer Status Is Already Exists
    /// </summary>
    /// <param name="status">Status</param>
    /// <param name="customerId">Customer Id</param>
    /// <param name="statusId">Status Id</param>
    /// <returns>Return Frue / False</returns>
    public bool ValidateCustomerStatus(string status, Guid customerId, Guid statusId)
    {
        bool isExists = false;

        if (status != null && status != "")
            isExists = _repositoryContext.customerCodemasters.Any(x => (x.StatusName.ToLower().Trim() == status.ToLower().Trim()) && (x.CustomerId == customerId) && (x.StatusId == statusId));

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
    public async Task<ResultDto<CodeMasterResponseDto>> GetStatusById(Guid id,Guid? customerId, CancellationToken ct)
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

        var codeMasterResponse = _mapper.Map<CodeMasterResponseDto>(role);

        if(customerId != null)
        {
            var customerCodeMaster = await GetCustomerStatus(codeMasterResponse.Id, (Guid)customerId, ct);
            if (customerCodeMaster != null)
                codeMasterResponse.customerCodeMaster = customerCodeMaster;

        }
       

        var response = new ResultDto<CodeMasterResponseDto>
        {
            IsSuccess = true,
            Data = codeMasterResponse
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


        var isStatusExists = _repositoryContext.CodeMasters.Any(x => x.Value == StatusDto.Value && x.Id != StatusId);
        if (isStatusExists == true)
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
    public virtual async Task<PaginationResultDto<PagedList<CodeMasterResponseDto>>> GetAllStoreStatus(string keyword = null, Guid? customerId = null, int pageIndex = 0, int pageSize = 0, CancellationToken ct = default)
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

        var statusResponse = _mapper.Map<PagedList<CodeMasterResponseDto>>(codeMaster);

        foreach(var item in statusResponse)
        {
            if (customerId != null)
            {
                var customerCodeMaster = await GetCustomerStatus(item.Id, (Guid)customerId, ct);
                if (customerCodeMaster != null)
                    item.customerCodeMaster = customerCodeMaster;

            }
        }

        var response = new PaginationResultDto<PagedList<CodeMasterResponseDto>>
        {
            IsSuccess = true,
            Data = statusResponse,
            TotalCount = codeMaster.TotalCount,
            TotalPages = codeMaster.TotalPages
        };
        return response;
    }

    #endregion



    #region Customer Code Master

    /// <summary>
    /// gets the Status details by Id
    /// </summary>
    /// <param name="id">Status Id</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns> Status Infromation</returns>
    public async Task<ResultDto<CustomerCodeMasterResponseDto>> GetCustomerStatusById(Guid id, CancellationToken ct)
    {
        if (id == null)
        {
            var UserResult = new ResultDto<CustomerCodeMasterResponseDto>
            {
                ErrorMessage = StringResources.InvalidArgument,
                IsSuccess = false
            };
            return UserResult;
        }
        var customerCodemaster = await _repositoryContext.customerCodemasters.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (customerCodemaster == null)
        {
            var UserResult = new ResultDto<CustomerCodeMasterResponseDto>()
            {
                ErrorMessage = StringResources.NoResultsFound,
                IsSuccess = false
            };
            return UserResult;
        }

        var customerCodemasterResponse = _mapper.Map<CustomerCodeMasterResponseDto>(customerCodemaster);
        var response = new ResultDto<CustomerCodeMasterResponseDto>
        {
            IsSuccess = true,
            Data = customerCodemasterResponse
        };
        return response;
    }


    /// <summary>
    /// gets the Status details by Id
    /// </summary>
    /// <param name="id">Status Id</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns> Status Infromation</returns>
    public async Task<CustomerCodeMasterResponseDto?> GetCustomerStatus(Guid statusId,Guid customerId, CancellationToken ct)
    {
      
        var customerCodemaster = await _repositoryContext.customerCodemasters.FirstOrDefaultAsync(x => x.StatusId == statusId && x.CustomerId == customerId, ct);

        if (customerCodemaster == null)
            return null;       

       return _mapper.Map<CustomerCodeMasterResponseDto>(customerCodemaster);
    }



    /// <summary>
    /// Creates a new Status
    /// </summary>
    /// <param name="StatusDto">Status</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public async Task<ResultDto<CodeMasterResponseDto>> InsertCustomerStatus(CustomerCodeMasterDto StatusDto, CancellationToken ct = default)
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

        var isExists = ValidateCustomerStatus(StatusDto.StatusName, StatusDto.CustomerId, StatusDto.StatusId);

        if (isExists)
        {
            var errorResponse = new ResultDto<CodeMasterResponseDto>
            {
                ErrorMessage = StringResources.RecordExists,
                IsSuccess = false

            };
            return errorResponse;
        }

        var customerCodeMaster = _mapper.Map<CustomerCodemaster>(StatusDto);

        await _repositoryContext.customerCodemasters.AddAsync(customerCodeMaster, ct);
        await _repositoryContext.SaveChangesAsync(ct);

        var customerMaster = await GetStatusById(customerCodeMaster.Id, customerCodeMaster.CustomerId,new CancellationToken());

        var result = new ResultDto<CodeMasterResponseDto>
        {
            Data = customerMaster?.Data,
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
    public async Task<ResultDto<CodeMasterResponseDto>> UpdateCustomerStatus(Guid customerStatusId, CustomerCodeMasterDto StatusDto, CancellationToken ct = default)
    {
        if (StatusDto == null)
        {
            var errorResponse = new ResultDto<CodeMasterResponseDto>
            {
                ErrorMessage = StringResources.BadRequest,
                IsSuccess = false
            };
            return errorResponse;
        }


        var isStatusExists = _repositoryContext.customerCodemasters.Any(x => x.StatusName == StatusDto.StatusName && x.CustomerId == StatusDto.CustomerId && x.StatusId == StatusDto.StatusId && x.Id != customerStatusId);
        if (isStatusExists == true)
        {
            var errorResponse = new ResultDto<CodeMasterResponseDto>
            {
                ErrorMessage = StringResources.BadRequest,IsSuccess= false
            };
            return errorResponse;
        }
        var customerCodeMasterResult = await _repositoryContext.customerCodemasters.FirstOrDefaultAsync(x => x.Id == customerStatusId, ct);

        if(customerCodeMasterResult == null)
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

        }


        customerCodeMasterResult.StatusName = StatusDto.StatusName;


        await _repositoryContext.SaveChangesAsync(ct);


        
        var codeMasterResponse = await GetStatusById(customerCodeMasterResult.StatusId, customerCodeMasterResult.CustomerId,new CancellationToken());

        var result = new ResultDto<CodeMasterResponseDto>
        {
            Data = codeMasterResponse?.Data,
            IsSuccess = true
        };
        return result;

    }



    #endregion
}


