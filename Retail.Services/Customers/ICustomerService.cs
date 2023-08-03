using Retail.DTOs.Customers;
using Retail.DTOs;
using Retail.Services.Common;

namespace Retail.Services.Customers;

/// <summary>
/// Customer Interface
/// </summary>
public interface ICustomerService
{
    #region Methods

    /// <summary>
    /// gets all Customers
    /// </summary>
    /// <param name="pageIndex">page Indes</param>
    /// <param name="pageSize">page size</param>
    /// <param name="ct">cancellation token</param>
    /// <returns>Customer List with Pagination</returns>
    Task<PaginationResultDto<PagedList<CustomerResponseDto>>> GetAllCustomers(string keyword = null, int pageIndex = 0, int pageSize = 0, CancellationToken ct = default);



    /// <summary>
    /// gets the Customer details by Id
    /// </summary>
    /// <param name="id">customer Id</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns> User Infromation</returns>
    Task<ResultDto<CustomerResponseDto>> GetCustomerById(Guid id, CancellationToken ct);

    Task<ResultDto<CustomerResponseDto>> UploadLogoByCustomerId(Guid id, string ImgUrl, CancellationToken ct);

    #endregion
}

