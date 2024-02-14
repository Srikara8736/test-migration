﻿using AutoMapper;
using Retail.Data.Entities.Customers;
using Retail.Data.Repository;
using Retail.DTOs.Customers;
using Retail.DTOs;
using Retail.Services.Common;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Retail.Data.Entities.UserAccount;
using Retail.DTOs.UserAccounts;
using Retail.Services.Stores;
using Retail.DTOs.Stores;
using Microsoft.Extensions.Configuration;
using Retail.Data.Entities.FileSystem;
using Retail.DTOs.Cad;
using Retail.Data.Entities.Stores;
using System.IO;
using System.Collections.Generic;
using SixLabors.ImageSharp.Formats.Jpeg;
using System.Data;

namespace Retail.Services.Customers;

/// <summary>
/// Customer Service
/// </summary>
public class CustomerService : ICustomerService
{
    #region Fields

    private readonly RepositoryContext _repositoryContext;
    private readonly IMapper _mapper;
    private readonly IStoreService _storeService;
    private readonly IConfiguration _configuration;

    #endregion


    #region Ctor

    public CustomerService(RepositoryContext repositoryContext, IMapper mapper, IStoreService storeService, IConfiguration configuration)
    {
        _repositoryContext = repositoryContext;
        _mapper = mapper;
        _storeService = storeService;
        _configuration = configuration;
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


    #endregion

    #region Methods

    /// <summary>
    ///Rezise the Image
    /// </summary>
    /// <param name="imageStream">Image Stream</param>
    /// <param name="width">Image Width</param>
    /// <param name="height">Image Height</param>
    /// <param name="outPath">Image Location</param>
    /// <returns>Resize Image</returns>
    public async Task<bool> ResizeImage(Stream imageStream, int width, int height, string outPath)
    {
        try
        {

            using (SixLabors.ImageSharp.Image imageItem = SixLabors.ImageSharp.Image.Load(imageStream))
            {               
                imageItem.Mutate(x => x.Resize(width, height));

                imageItem.Save(outPath);
            }

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }

    }


    /// <summary>
    /// Get all Customer Images
    /// </summary>
    /// <param name="customerId">Customer Id</param>
    /// <returns>Customer Image</returns>
    public async Task<List<CustomerImage>> GetCustomerImagesByCustomerId(Guid customerId)
    {
        if (customerId == null)
            return null;

        return await _repositoryContext.CustomerImages.Where(x => x.CustomerId == customerId).ToListAsync();
    }



    /// <summary>
    /// Get all Customers
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

        var path = _configuration["AssetLocations:ClientLogo"];
        var backgroundImagepath = _configuration["AssetLocations:BackgroundImages"];

        foreach (var customer in customerResponse)
        {
            if (customer.LogoImageId != null)
            {
                var image = await _storeService.GetImageById((Guid)customer.LogoImageId);
                if (image != null)
                    customer.Logo = backgroundImagepath + image.FileName;
            }

            if (customer.BackgroundImageId != null)
            {
                var codeMasterItem = await _repositoryContext.CodeMasters.FirstOrDefaultAsync(x => x.Id == customer.BackgroundImageId && x.Type == "BackgroundImage");
                if (codeMasterItem != null)
                {
                    customer.BackgroundImage = backgroundImagepath + codeMasterItem.Value;
                }
            }
            else
            {
                customer.Logo = backgroundImagepath + "Flower.png";
            }

            var result = _repositoryContext.Addresses.Where(x => x.Id == customer.AddressId).FirstOrDefault();
            if (result != null)
            {
                var customerAddress = _mapper.Map<AddressDto>(result);
                customer.Address = customerAddress;
            }


            var stores = await _storeService.GetStoresByCustomerId(customer.Id);

            if (stores != null)
            {
                var customerStore = new CustomerStoreDto
                {
                    NumberOfStore = stores.Count,
                    Store = stores
                };

                foreach (var store in stores)
                {
                    var areaDetails = await _storeService.GetGridData(store.Id,null);
                    if (areaDetails.Data != null)
                    {
                        //customerStore.TotalStoreArea = customerStore.TotalStoreArea + areaDetails.Data.Sum(x => x.TotalArea);
                        customerStore.TotalSalesArea = customerStore.TotalSalesArea + areaDetails.Data.Where(y => y.AreaType == "SalesArea").Sum(x => x.TotalArea);
                    }

                }

                customerStore.TotalStoreArea = stores.Sum(x => x.TotalArea);

                var storeStatus = _repositoryContext.CodeMasters.Where(x => x.Type.ToLower().Trim() == "storestatus").ToList();

                var storeStatusItems = new List<StoreStatusDto>();

                foreach (var store in stores.GroupBy(x => x.StoreStatus).Select(group => new {
                    Status = group.Key,
                    Count = group.Count()
                }))
                {

                    var storeStatusDto = new StoreStatusDto
                    {
                        Status = store.Status,
                        NumberOfStore = store.Count
                    };


                    var storeStatusItem = storeStatus.Where(x => x.Value.ToLower().Trim() == store.Status.ToLower().Trim()).FirstOrDefault();
                    if (storeStatusItem != null)
                    {
                        storeStatusDto.Property = storeStatusItem.Property;
                        storeStatus.Remove(storeStatusItem);
                    }




                    storeStatusItems.Add(storeStatusDto);

                }

                foreach (var item in storeStatus)
                {
                    var storeStatusDto = new StoreStatusDto
                    {
                        Status = item.Value,
                        NumberOfStore = 0,
                        Property = item.Property
                    };

                    storeStatusItems.Add(storeStatusDto);
                }



                customerStore.StoreStatus = storeStatusItems;


                customer.CustomerStores = customerStore;

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
    /// Get the customer details by User Id
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

        if (customerResponse.LogoImageId != null)
        {
            var path = _configuration["AssetLocations:ClientLogo"];

            var image = await _storeService.GetImageById((Guid)customerResponse.LogoImageId);
            if (image != null)
                customerResponse.Logo = path + image.FileName;

        }

        var customerImageList = await GetCustomerImagesByCustomerId(customerResponse.Id);
        var customerImagepath = _configuration["AssetLocations:CustomerImages"];

        foreach (var img in customerImageList)
        {
            var image = await _storeService.GetImageById((Guid)img.ImageId);
            if (image != null)
            {
                var storeImageItem = new ImageDto()
                {
                    Id = img.Id,
                    ImageId = image.Id,
                    ImageUrl = customerImagepath + image.FileName
                };


                customerResponse.CustomerImages.Add(storeImageItem);
            }

        }

        var result = _repositoryContext.Addresses.Where(x => x.Id == customerResponse.AddressId).FirstOrDefault();
        if (result != null)
        {
            var customerAddress = _mapper.Map<AddressDto>(result);
            customerResponse.Address = customerAddress;
        }


        var stores = await _storeService.GetStoresByCustomerId(customerResponse.Id);
        var storeStatus = _repositoryContext.CodeMasters.Where(x => x.Type.ToLower().Trim() == "storestatus").ToList();

        if (stores != null)
        {
            var customerStore = new CustomerStoreDto
            {
                NumberOfStore = stores.Count,
                Store = stores
            };

            foreach (var store in stores)
            {
                var areaDetails = await _storeService.GetGridData(store.Id,null);
                if (areaDetails.Data != null)
                {
                   // customerStore.TotalStoreArea = customerStore.TotalStoreArea + areaDetails.Data.Sum(x => x.TotalArea);
                    customerStore.TotalSalesArea = customerStore.TotalSalesArea + areaDetails.Data.Where(y => y.AreaType == "SalesArea").Sum(x => x.TotalArea);
                }

            }
            customerStore.TotalStoreArea = stores.Sum(x => x.TotalArea);
            var storeStatusItems = new List<StoreStatusDto>();

            foreach (var store in stores.GroupBy(x => x.StoreStatus).Select(group => new {
                Status = group.Key,
                Count = group.Count()
            }))
            {
                var storeStatusDto = new StoreStatusDto
                {
                    Status = store.Status,
                    NumberOfStore = store.Count
                };

                var storeStatusItem = storeStatus.Where(x => x.Value.ToLower().Trim() == store.Status.ToLower().Trim()).FirstOrDefault();
                if (storeStatusItem != null)
                {
                    storeStatusDto.Property = storeStatusItem.Property;
                    storeStatus.Remove(storeStatusItem);
                }

                storeStatusItems.Add(storeStatusDto);

            }

            foreach (var item in storeStatus)
            {
                var storeStatusDto = new StoreStatusDto
                {
                    Status = item.Value,
                    NumberOfStore = 0,
                    Property = item.Property
                };

                storeStatusItems.Add(storeStatusDto);
            }


            customerStore.StoreStatus = storeStatusItems;


            customerResponse.CustomerStores = customerStore;

        }

        var backgroundImagepath = _configuration["AssetLocations:BackgroundImages"];
        if (customerResponse.BackgroundImageId != null)
        {
            var codeMasterItem = await _repositoryContext.CodeMasters.FirstOrDefaultAsync(x => x.Id == customerResponse.BackgroundImageId && x.Type == "BackgroundImage");
            if (codeMasterItem != null)
            {
                customerResponse.BackgroundImage = backgroundImagepath + codeMasterItem.Value;
            }

        }
        else
        {
            customerResponse.Logo = backgroundImagepath + "Flower.png";
        }


        var response = new ResultDto<CustomerResponseDto>
        {
            IsSuccess = true,
            Data = customerResponse
        };
        return response;
    }


    /// <summary>
    /// Upload Logo for the customer
    /// </summary>
    /// <param name="id">customer Id</param>
    /// <param name="ImgUrl">Image URl</param>
    /// <param name="fileType">File Type</param>
    /// <param name="fileExtension">File Extension</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns> Customer Information</returns>.
    public async Task<ResultDto<CustomerResponseDto>> UploadLogoByCustomerId(Guid id, string imageUrl, string fileType, string fileExtension, CancellationToken ct)
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

        var imgItem = new Data.Entities.FileSystem.Image()
        {
            FileName = imageUrl,
            UploadedOn = DateTime.UtcNow,
            FileExtension = fileExtension,
            FileType = fileType,
            UploadedBy = "Retail"

        };

        var img = await _storeService.InsertImage(imgItem);

        if (img != null)
            customer.LogoImageId = img.Id;



        await _repositoryContext.SaveChangesAsync(ct);
        var customerResponse = _mapper.Map<CustomerResponseDto>(customer);

        var customerItem = await GetCustomerById(customerResponse.Id, ct);
        if (customerItem.Data != null)
            customerResponse = customerItem.Data;


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

        var address = await _storeService.InsertCustomerAddress(customerRequestDto.Address, ct);

        var customer = _mapper.Map<Customer>(customerRequestDto);

        customer.AddressId = address.Id;
        customer.CreatedBy = customerRequestDto.CreatedBy;

        customer.CreatedOn = DateTime.UtcNow;

        await _repositoryContext.Customers.AddAsync(customer, ct);
        await _repositoryContext.SaveChangesAsync(ct);

        var customerResponse = _mapper.Map<CustomerResponseDto>(customer);

        var customerItem = await GetCustomerById(customerResponse.Id, ct);
        if (customerItem.Data != null)
            customerResponse = customerItem.Data;

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

        var customerItem = await GetCustomerById(customerResponse.Id, ct);
        if (customerItem.Data != null)
            customerResponse = customerItem.Data;


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
    public virtual async Task<ResultDto<bool>> DeleteCustomer(Guid id, CancellationToken ct = default)
    {
        var customer = await _repositoryContext.Customers.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (customer == null)
        {
            var response = new ResultDto<bool>
            {
                ErrorMessage = StringResources.RecordNotFound,
                StatusCode = HttpStatusCode.NotFound
            };
            return response;

        }

        customer.IsDeleted = true;
        await _repositoryContext.SaveChangesAsync(ct);

        var successResponse = new ResultDto<bool>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK
        };
        return successResponse;



    }


    /// <summary>
    /// Upload Customer Image
    /// </summary>
    /// <param name="id">customer Id</param>
    /// <param name="imgUrl">imgUrl</param>
    /// <param name="fileType">File Type</param>
    /// <param name="fileExtension">File Extension</param>
    /// <returns>Upload Customer Image</returns>
    public async Task<ResultDto<bool>> UploadCustomerImage(string customerId, string imgUrl, string fileType, string fileExtension)
    {
        if (customerId == null || imgUrl == null)
        {
            var customerResult = new ResultDto<bool>
            {
                ErrorMessage = StringResources.InvalidArgument,
                StatusCode = HttpStatusCode.BadRequest,
                Data = false

            };


            return customerResult;
        }

        var imageItem = new Data.Entities.FileSystem.Image()
        {
            FileName = imgUrl,
            UploadedOn = DateTime.UtcNow,
            FileExtension = fileExtension,
            FileType = fileType,
            UploadedBy = "Retail"

        };

        var img = await _storeService.InsertImage(imageItem);
        if (img == null)
        {
            var customerResult = new ResultDto<bool>
            {
                ErrorMessage = StringResources.InvalidArgument,
                StatusCode = HttpStatusCode.BadRequest,
                Data = false

            };

            return customerResult;
        }

        var customerImage = new CustomerImage()
        {
            ImageId = img.Id,
            CustomerId = Guid.Parse(customerId)
        };

        var storeResult = await _storeService.InsertCustomerImage(customerImage);

        if (storeResult == null)
        {
            var customerResult = new ResultDto<bool>
            {
                ErrorMessage = StringResources.InvalidArgument,
                StatusCode = HttpStatusCode.BadRequest,
                Data = false

            };
        }

        var result = new ResultDto<bool>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Data = true

        };


        return result;
    }


    /// <summary>
    /// Delete Customer
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Response Customer Delete Status</returns>
    public virtual async Task<ResultDto<bool>> DeleteCustomerImage(Guid customerId, Guid customerImageId, Guid ImageId, CancellationToken ct = default)
    {
        var customerImage = await _repositoryContext.CustomerImages.FirstOrDefaultAsync(x => x.Id == customerImageId && x.CustomerId == customerId, ct);
        if (customerImage == null)
        {
            var response = new ResultDto<bool>
            {
                IsSuccess = false,
                ErrorMessage = StringResources.RecordNotFound,
                StatusCode = HttpStatusCode.NotFound
            };
            return response;

        }

        _repositoryContext.CustomerImages.Remove(customerImage);
        await _repositoryContext.SaveChangesAsync(ct);

        var ImageItem = await _repositoryContext.Images.FirstOrDefaultAsync(x => x.Id == ImageId, ct);
        if (ImageItem == null)
        {
            var response = new ResultDto<bool>
            {
                IsSuccess = false,
                ErrorMessage = StringResources.RecordNotFound,
                StatusCode = HttpStatusCode.NotFound
            };
            return response;

        }
        _repositoryContext.Images.Remove(ImageItem);
        await _repositoryContext.SaveChangesAsync(ct);

        var successResponse = new ResultDto<bool>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK
        };
        return successResponse;



    }



    #endregion

}
