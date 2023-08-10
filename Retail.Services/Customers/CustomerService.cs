using AutoMapper;
using Retail.Data.Entities.Customers;
using Retail.Data.Repository;
using Retail.DTOs.Customers;
using Retail.DTOs;
using Retail.Services.Common;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Retail.Data.Entities.UserAccount;
using Retail.DTOs.UserAccounts;

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


    #region Utilities

    /// <summary>
    /// Validate User Is Already Exists
    /// </summary>
    /// <param name="emailAdrress">emailAdrress</param>
    /// <returns>Flag Value</returns>
    public bool ValidateCustomerEmail(string emailAdrress)
    {
        bool isExists = false;

        if (emailAdrress != "" && emailAdrress != null)
            isExists = _repositoryContext.Customers.Any(x => x.Email.ToLower().Trim() == emailAdrress.ToLower().Trim());

        return isExists;

    }


    public async Task<Address> InsertCustomerAddress(AddressDto addressDto, CancellationToken ct)
    {
        try
        {
            var address = _mapper.Map<Address>(addressDto);
            await _repositoryContext.Addresses.AddAsync(address, ct);
            await _repositoryContext.SaveChangesAsync(ct);
            return address;
        }
        catch (Exception ex)
        {
            return null;
        }

    }


    public async Task<Address> UpdateCustomerAddress(Guid addressId, AddressDto addressDto, CancellationToken ct)
    {
        try
        {
            var address = await _repositoryContext.Addresses.Where(x => x.Id == addressId).FirstOrDefaultAsync();
            if (address == null)
                return null;

            address.City = addressDto.City;
            address.Street = addressDto.Street;
            address.ZipCode = addressDto.ZipCode;
            address.Country = addressDto.Country;


            await _repositoryContext.SaveChangesAsync(ct);
            return address;
        }
        catch (Exception ex)
        {
            return null;
        }

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
                    where p.IsDeleted == false
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



    /// <summary>
    /// Add the Customer details 
    /// </summary>
    /// <param name="customerRequestDto">Customer Request DTO</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Customer Information</returns>
    public async Task<ResultDto<CustomerResponseDto>> InsertCustomer(CustomerDto customerRequestDto, CancellationToken ct)
    {
        if (customerRequestDto == null)
        {
            var UserResult = new ResultDto<CustomerResponseDto>()
            {
                ErrorMessage = StringResources.InvalidArgument,
                StatusCode = HttpStatusCode.BadRequest
            };
            return UserResult;

        }


        var resultCusEmail = ValidateCustomerEmail(customerRequestDto.Email);

        if (resultCusEmail)
        {
            var errorResponse = new ResultDto<CustomerResponseDto>
            {
                ErrorMessage = StringResources.UserRecordExists,
                StatusCode = HttpStatusCode.Conflict

            };
            return errorResponse;
        }

        var address = await InsertCustomerAddress(customerRequestDto.Address, ct);

        var customer = _mapper.Map<Customer>(customerRequestDto);

        customer.AddressId = address.Id;
        customer.CreatedBy = customerRequestDto.CreatedBy;

        customer.CreatedOn = DateTime.UtcNow;
     
        await _repositoryContext.Customers.AddAsync(customer, ct);
        await _repositoryContext.SaveChangesAsync(ct);

        var customerResponse = _mapper.Map<CustomerResponseDto>(customer);

        
       var customerAddress = _mapper.Map<AddressDto>(address);
       customerResponse.Address = customerAddress;


        var resultResponse = new ResultDto<CustomerResponseDto>
        {
            Data = customerResponse,
            IsSuccess = true
        };
        return resultResponse;

    }



    /// <summary>
    /// Update the Customer details 
    /// </summary>
    /// <param name="id">Customer id</param>
    /// <param name="customerDto">Customer Request DTO</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Customer Information</returns>
    public virtual async Task<ResultDto<CustomerResponseDto>> UpdateCustomer(Guid id, CustomerDto customerDto, CancellationToken ct = default)
    {
        if (customerDto == null)
        {
            var response = new ResultDto<CustomerResponseDto>
            {
                ErrorMessage = StringResources.NoResultsFound,
                StatusCode = HttpStatusCode.NotFound
            };
            return response;

        }

        var result = await _repositoryContext.Customers.FirstOrDefaultAsync(x => x.Id == id, ct);



        if (result == null)
        {
            var response = new ResultDto<CustomerResponseDto>
            {
                ErrorMessage = StringResources.RecordNotFound,
                StatusCode = HttpStatusCode.NotFound
            };
            return response;

        }

       
        result.Name = customerDto.Name;
        result.PhoneNumber = customerDto.PhoneNumber;
        result.Email = customerDto.Email;

        result.UpdatedOn = DateTime.UtcNow;
        result.UpdatedBy = customerDto.UpdatedBy;
       
        await _repositoryContext.SaveChangesAsync(ct);
        
        var customerResponse = _mapper.Map<CustomerResponseDto>(result);

        var address = await UpdateCustomerAddress(result.AddressId, customerDto.Address, ct);
        if(address != null)
        {
            var customerAddress = _mapper.Map<AddressDto>(address);
            customerResponse.Address = customerAddress;

        }


        var successResponse = new ResultDto<CustomerResponseDto>
        {
            IsSuccess = true,
            Data = customerResponse,
            StatusCode = HttpStatusCode.OK
        };
        return successResponse;
    }



    /// <summary>
    /// Delete Customer
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Response Customer Delete Status</returns>
    public virtual async Task<ResultDto<CustomerResponseDto>> DeleteCustomer(Guid id, CancellationToken ct = default)
    {
        var customer = await _repositoryContext.Customers.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (customer == null)
        {
            var response = new ResultDto<CustomerResponseDto>
            {
                ErrorMessage = StringResources.RecordNotFound,
                StatusCode = HttpStatusCode.NotFound
            };
            return response;

        }

        customer.IsDeleted = true;
        await _repositoryContext.SaveChangesAsync(ct);

        var successResponse = new ResultDto<CustomerResponseDto>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK
        };
        return successResponse;



    }


    #endregion

}
