using Retail.DTOs.Customers;
using Retail.DTOs;
using Retail.Services.Common;
using Retail.DTOs.UserAccounts;
using Retail.Data.Entities.Customers;
using Retail.Data.Entities.UserAccount;

namespace Retail.Services.Customers;

/// <summary>
/// Customer Interface
/// </summary>
public interface ICustomerService
{
    #region Methods

    /// <summary>
    ///Rezise the Image
    /// </summary>
    /// <param name="imageStream">Image Stream</param>
    /// <param name="width">Image Width</param>
    /// <param name="height">Image Height</param>
    /// <param name="outPath">Image Location</param>
    /// <returns>Resize Image</returns>
    Task<bool> ResizeImage(Stream imageStream, int width, int height, string outPath);



    /// <summary>
    /// Get all Customer Images
    /// </summary>
    /// <param name="customerId">Customer Id</param>
    /// <returns>Customer Image</returns>
    Task<List<CustomerImage>> GetCustomerImagesByCustomerId(Guid customerId);

    /// <summary>
    /// Get all Customers
    /// </summary>
    /// <param name="pageIndex">page Indes</param>
    /// <param name="pageSize">page size</param>
    /// <param name="ct">cancellation token</param>
    /// <returns>Customer List with Pagination</returns>
    Task<PaginationResultDto<PagedList<CustomerResponseDto>>> GetAllCustomers(string keyword = null, int pageIndex = 0, int pageSize = 0, CancellationToken ct = default);



    /// <summary>
    /// Get the Customer details by Id
    /// </summary>
    /// <param name="id">customer Id</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns> Customer Information</returns>
    Task<ResultDto<CustomerResponseDto>> GetCustomerById(Guid id, CancellationToken ct);


    /// <summary>
    /// Upload Logo for the customer
    /// </summary>
    /// <param name="id">customer Id</param>
    /// <param name="ImgUrl">Image URl</param>
    /// <param name="fileType">File Type</param>
    /// <param name="fileExtension">File Extension</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns> Customer Information</returns>
    Task<ResultDto<CustomerResponseDto>> UploadLogoByCustomerId(Guid id, string ImgUrl,string fileType,string fileExtension, CancellationToken ct);


    /// <summary>
    /// Add the Customer details 
    /// </summary>
    /// <param name="customerRequestDto">Customer Request DTO</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Customer Information</returns>
    Task<ResultDto<CustomerResponseDto>> InsertCustomer(CustomerDto customerRequestDto, CancellationToken ct);



    /// <summary>
    /// Update the Customer details 
    /// </summary>
    /// <param name="id">Customer id</param>
    /// <param name="customerDto">Customer Request DTO</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Customer Information</returns>
    Task<ResultDto<CustomerResponseDto>> UpdateCustomer(Guid id, CustomerDto customerDto, CancellationToken ct = default);




    /// <summary>
    /// Delete Customer
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Response Customer Delete Status</returns>
    Task<ResultDto<bool>> DeleteCustomer(Guid id, CancellationToken ct = default);


    /// <summary>
    /// Upload Customer Image
    /// </summary>
    /// <param name="id">customer Id</param>
    /// <param name="imgUrl">imgUrl</param>
    /// <param name="fileType">File Type</param>
    /// <param name="fileExtension">File Extension</param>
    /// <returns>Upload Customer Image</returns>
    Task<ResultDto<bool>> UploadCustomerImage(string customerId, string imgUrl, string fileType, string fileExtension);


    /// <summary>
    /// Delete Customer Image
    /// </summary>
    /// <param name="id">customer Id</param>
    /// <param name="customerImageId">customer Image Id</param>
    /// <param name="ImageId">Image Id</param>
    /// <returns>Delete Customer Image</returns>
    Task<ResultDto<bool>> DeleteCustomerImage(Guid customerId, Guid customerImageId, Guid ImageId, CancellationToken ct = default);

  

    #endregion
}

