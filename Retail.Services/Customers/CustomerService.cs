using AutoMapper;
using Retail.Data.Entities.Customers;
using Retail.Data.Repository;
using Retail.DTOs.Customers;
using Retail.DTOs;
using Retail.Services.Common;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace Retail.Services.Customers;

/// <summary>
/// Customer Service
/// </summary>
public class CustomerService : ICustomerService
{
    #region Fields

    private readonly RepositoryContext _repositoryContext;
    private readonly IMapper _mapper;

    #endregion


    #region Ctor

    public CustomerService(RepositoryContext repositoryContext, IMapper mapper)
    {
        _repositoryContext = repositoryContext;
        _mapper = mapper;
    }

    #endregion


    #region Methods

    /// <summary>
    /// gets all Customers
    /// </summary>
    /// <param name="pageIndex">page Indes</param>
    /// <param name="pageSize">page size</param>
    /// <param name="ct">cancellation token</param>
    /// <returns>Customer List with Pagination</returns>
    public async Task<PaginationResultDto<PagedList<CustomerResponseDto>>> GetAllCustomers(string keyword = null, int pageIndex = 0, int pageSize = 0, CancellationToken ct = default)
    {

        var query = from p in _repositoryContext.Customers
                    select p;

        if (keyword != null)
        {
            query = query.Where(x => x.Name.Contains(keyword));
        }

        var customers = await PagedList<Customer>.ToPagedList(query.OrderBy(on => on.Name), pageIndex, pageSize);

        if (customers == null)
        {
            var errorResponse = new PaginationResultDto<PagedList<CustomerResponseDto>>
            {
                IsSuccess = false,
                ErrorMessage = StringResources.NoResultsFound,
                StatusCode = HttpStatusCode.NoContent
            };
        }

        var customerResponse = _mapper.Map<PagedList<CustomerResponseDto>>(customers);

      
        foreach( var customer in customerResponse)
        {
            var result = _repositoryContext.Addresses.Where( x => x.Id  == customer.AddressId).FirstOrDefault();
            if(result != null)
            {
                var customerAddress = _mapper.Map<AddressDto>(result);
                customer.Address = customerAddress;
            }
            


        }

        var response = new PaginationResultDto<PagedList<CustomerResponseDto>>
        {
            IsSuccess = true,
            Data = customerResponse,
            TotalCount = customers.TotalCount,
            TotalPages = customers.TotalPages
        };
        return response;

    }



    /// <summary>
    /// gets the customer details by User Id
    /// </summary>
    /// <param name="id">customer Id</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Customer Details</returns>
    public async Task<ResultDto<CustomerResponseDto>> GetCustomerById(Guid id, CancellationToken ct)
    {
        if (id == null)
        {
            var customerResult = new ResultDto<CustomerResponseDto>
            {
                ErrorMessage = StringResources.InvalidArgument,
                StatusCode = HttpStatusCode.BadRequest
            };
            return customerResult;
        }
        var user = await _repositoryContext.Customers.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (user == null)
        {
            var customerResult = new ResultDto<CustomerResponseDto>()
            {
                ErrorMessage = StringResources.NoResultsFound,
                StatusCode = HttpStatusCode.NotFound
            };
            return customerResult;
        }

        var customerResponse = _mapper.Map<CustomerResponseDto>(user);

        var result = _repositoryContext.Addresses.Where(x => x.Id == customerResponse.AddressId).FirstOrDefault();
        if (result != null)
        {
            var customerAddress = _mapper.Map<AddressDto>(result);
            customerResponse.Address = customerAddress;
        }



        var response = new ResultDto<CustomerResponseDto>
        {
            IsSuccess = true,
            Data = customerResponse
        };
        return response;
    }



    public async Task<ResultDto<CustomerResponseDto>> UploadLogoByCustomerId(Guid id, string ImgUrl, CancellationToken ct)
    {
        if (id == null)
        {
            var customerResult = new ResultDto<CustomerResponseDto>
            {
                ErrorMessage = StringResources.InvalidArgument,
                StatusCode = HttpStatusCode.BadRequest
            };
            return customerResult;
        }
        var customer = await _repositoryContext.Customers.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (customer == null)
        {
            var customerResult = new ResultDto<CustomerResponseDto>()
            {
                ErrorMessage = StringResources.NoResultsFound,
                StatusCode = HttpStatusCode.NotFound
            };
            return customerResult;
        }

        //user.ImageUrl = ImgUrl;

        await _repositoryContext.SaveChangesAsync(ct);
        var customerResponse = _mapper.Map<CustomerResponseDto>(customer);


        var response = new ResultDto<CustomerResponseDto>
        {
            IsSuccess = true,
            Data = customerResponse
        };
        return response;
    }

    #endregion

}
