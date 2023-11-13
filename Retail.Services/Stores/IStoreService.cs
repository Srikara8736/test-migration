using Retail.Data.Entities.Customers;
using Retail.Data.Entities.FileSystem;
using Retail.Data.Entities.Stores;
using Retail.Data.Entities.UserAccount;
using Retail.DTOs;
using Retail.DTOs.Customers;
using Retail.DTOs.Stores;
using Retail.Services.Common;

namespace Retail.Services.Stores;

public interface IStoreService
{

    Task<List<StoreImage>> GetStoreImagesByStoreId(Guid storeId);

    Task<Data.Entities.FileSystem.Image> GetImageById(Guid id, CancellationToken ct = default);

    Task<Data.Entities.FileSystem.Image> InsertImage(Data.Entities.FileSystem.Image image);
    Task<StoreImage> InsertStoreImage(StoreImage image);

    Task<CustomerImage> InsertCustomerImage(CustomerImage image);

    Task<ResultDto<bool>> DeleteStoreImage(Guid storeId, Guid storeImageId, Guid ImageId, CancellationToken ct = default);

    Task<bool> DeleteImage(Guid ImageId);


    Task<ResultDto<bool>> UploadStoreImage(string storeId, string imgUrl, string fileType, string fileExtension);

    #region Address

    Task<Address> InsertCustomerAddress(AddressDto addressDto, CancellationToken ct);

    Task<Address> UpdateCustomerAddress(Guid addressId, AddressDto addressDto, CancellationToken ct);

    Task<bool> DeleteAddress(Guid addressId, CancellationToken ct);

    #endregion



    /// <summary>
    /// Get Store By Id
    /// </summary>
    /// <param name="storeId">customerId</param>
    /// <param name="ct">cancellation token</param>
    /// <returns>Store List with Pagination</returns>
    Task<ResultDto<StoreResponseDto>> GetStoreById(Guid storeId, CancellationToken ct = default);


    /// <summary>
    /// Insert Store 
    /// </summary>
    /// <param name="storeDto">store</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns></returns>
    Task<ResultDto<StoreResponseDto>> InsertStore(StoreDto storeDto, CancellationToken ct = default);

    /// <summary>
    /// Updates Store
    /// </summary>
    /// <param name="id">Store Id</param>
    /// <param name="storeDto">Store</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns></returns>
    Task<ResultDto<StoreResponseDto>> UpdateStore(string storeId, StoreDto storeDto, CancellationToken ct = default);


    /// <summary>
    /// Delete Role
    /// </summary>
    /// <param name="id">Store Id</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns></returns>
    Task<ResultDto<bool>> DeleteStore(string id, CancellationToken ct = default);


    /// <summary>
    /// Gets all Stores
    /// </summary>
    /// <param name="customerId">customerId</param>
    /// <param name="ct">cancellation token</param>
    /// <returns>Store List By customer</returns>
    Task<List<StoreResponseDto>> GetStoresByCustomerId(Guid customerId , CancellationToken ct = default);



    /// <summary>
    /// gets all Stores
    /// </summary>
    /// <param name="customerId">customerId</param>
    /// <param name="keyword">keyword</param>
    /// <param name="pageIndex">page Index</param>
    /// <param name="pageSize">page size</param>
    /// <param name="ct">cancellation token</param>
    /// <returns>Store List with Pagination</returns>
    Task<PaginationResultDto<PagedList<StoreResponseDto>>> GetAllStores(Guid? customerId = null, string? keyword = null, int pageIndex = 0, int pageSize = 0, CancellationToken ct = default);


    /// <summary>
    /// gets all Grid Data
    /// </summary>
    /// <param name="StoreId">Store Identifier</param>
    /// <param name="ct">cancellation token</param>
    /// <returns>Store Grid Data</returns>
    Task<ResultDto<List<ChartGridDto>>> GetGridData(Guid StoreId, CancellationToken ct = default);

    /// <summary>
    /// gets all Chart Data
    /// </summary>
    /// <param name="StoreId">Store Identifier</param>
    /// <param name="ct">cancellation token</param>
    /// <returns>Store Chart Data</returns>
    Task<ResultDto<List<ChartGraphDto>>> GetChartData(Guid StoreId, CancellationToken ct = default);


    /// <summary>
    /// gets all Drawing Grid Data
    /// </summary>
    /// <param name="StoreId">Store Identifier</param>
    /// <param name="ct">cancellation token</param>
    /// <returns>Store Grid Data</returns>
    Task<ResultDto<List<DrawingListResponseDto>>> GetDrawingGridData(Guid StoreId, CancellationToken ct = default);



    /// <summary>
    /// Get all StoreStatus
    /// </summary>
    /// <param name="keyword">keyword</param>
    /// <param name="pageIndex">PageIndex</param>
    /// <param name="pageSize">PageSize</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns></returns>
    Task<PaginationResultDto<PagedList<StoreStatusResponseDto>>> GetAllStoreStatus(string keyword = null, int pageIndex = 0, int pageSize = 0, CancellationToken ct = default);

    Task<ResultDto<List<ComparisionChartGraphDto>>> CompareStoreVersionData(Guid FirstStoreId, Guid FirstVersionId, Guid SecondStoreId, Guid SecondVersionId, CancellationToken ct = default);
}
