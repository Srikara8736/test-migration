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
    /// <summary>
    /// Get Store Image items by Store Identifiers
    /// </summary>
    /// <param name="storeId">Store Id</param>
    /// <returns> Image items</returns>
    Task<List<StoreImage>> GetStoreImagesByStoreId(Guid storeId);


    /// <summary>
    /// Get Image item by Identifiers
    /// </summary>
    /// <param name="id">id</param>
    /// <param name="ct">Cancellation Charages</param>
    /// <returns>Image Information</returns>
    Task<Data.Entities.FileSystem.Image> GetImageById(Guid id, CancellationToken ct = default);


    /// <summary>
    /// Insert Image 
    /// </summary>
    /// <param name="image">image</param>
    /// <param name="ct">Cancellation Charages</param>
    /// <returns>Image Information</returns>
    Task<Data.Entities.FileSystem.Image> InsertImage(Data.Entities.FileSystem.Image image);


    /// <summary>
    /// Insert store Image
    /// </summary>
    /// <param name="image">image</param>
    /// <param name="ct">Cancellation Charages</param>
    /// <returns>Store Image Information</returns>
    Task<StoreImage> InsertStoreImage(StoreImage image);


    /// <summary>
    /// Insert customer Image
    /// </summary>
    /// <param name="image">Image</param>
    /// <param name="ct">Cancellation Charages</param>
    /// <returns>Insert customer Image</returns>
    Task<CustomerImage> InsertCustomerImage(CustomerImage image);


    /// <summary>
    /// Delete Store Images
    /// </summary>
    /// <param name="storeId">Store Identifier</param>
    /// <param name="storeImageId">Store Image Id</param>
    /// <param name="ImageId">Image  Id</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>True / False of Store Image Deletion</returns>
    Task<ResultDto<bool>> DeleteStoreImage(Guid storeId, Guid storeImageId, Guid ImageId, CancellationToken ct = default);


    /// <summary>
    /// Delete Image
    /// </summary>
    /// <param name="image">image</param>
    /// <param name="ct">Cancellation Charages</param>
    /// <returns>True / False Image Delete Status</returns>
    Task<bool> DeleteImage(Guid ImageId);


    /// <summary>
    /// Update Store Images
    /// </summary>
    /// <param name="storeId">Store Identifier</param>
    /// <param name="imgUrl">Store Image URL</param>
    /// <param name="fileType">File Type</param>
    /// <param name="fileExtension">File Extension</param>
    /// <returns>True / False of Store Image Updation</returns>
    Task<ResultDto<bool>> UploadStoreImage(string storeId, string imgUrl, string fileType, string fileExtension);

    #region Address

    /// <summary>
    /// Insert customer Address
    /// </summary>
    /// <param name="addressDto">Address</param>
    /// <param name="ct">Cancellation Charages</param>
    /// <returns>Insert customer Address</returns>
    Task<Address> InsertCustomerAddress(AddressDto addressDto, CancellationToken ct);


    /// <summary>
    /// Update Customer Address
    /// </summary>
    /// <param name="addressId">addressId</param>
    /// <param name="addressDto">Address</param>
    /// <param name="ct">Cancellation Charages</param>
    /// <returns>Update Customer Image</returns>
    Task<Address> UpdateCustomerAddress(Guid addressId, AddressDto addressDto, CancellationToken ct);


    /// <summary>
    /// Delete Customer Address
    /// </summary>
    /// <param name="addressId">addressId</param>
    /// <param name="ct">Cancellation Charages</param>
    /// <returns>Delete Customer Address</returns>
    Task<bool> DeleteAddress(Guid addressId, CancellationToken ct);

    #endregion



    /// <summary>
    /// Get Store By Id
    /// </summary>
    /// <param name="storeId">customerId</param>
    /// <param name="ct">cancellation token</param>
    /// <returns>Store Information</returns>
    Task<ResultDto<StoreResponseDto>> GetStoreById(Guid storeId, CancellationToken ct = default);


    /// <summary>
    /// Insert Store Information
    /// </summary>
    /// <param name="storeDto">store</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Store Infromation</returns>
    Task<ResultDto<StoreResponseDto>> InsertStore(StoreDto storeDto, CancellationToken ct = default);

    /// <summary>
    /// Update Store Information
    /// </summary>
    /// <param name="id">Store Id</param>
    /// <param name="storeDto">Store</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Updated Store Information</returns>
    Task<ResultDto<StoreResponseDto>> UpdateStore(string storeId, StoreDto storeDto, CancellationToken ct = default);


    /// <summary>
    /// Delete Store
    /// </summary>
    /// <param name="id">Store Id</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>True / False status of Store Deletion</returns>
    Task<ResultDto<bool>> DeleteStore(string id, CancellationToken ct = default);


    /// <summary>
    /// Update Data Status
    /// </summary>
    /// <param name="id">Store Data Id</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>True / False status of Store Data Update</returns>
    Task<ResultDto<bool>> UpdateStoreData(Guid id,StoreDataStatusDto storeData, CancellationToken ct = default);


    /// <summary>
    /// Get all Stores By Customer
    /// </summary>
    /// <param name="customerId">customerId</param>
    /// <param name="ct">cancellation token</param>
    /// <returns>Store List By customer</returns>
    Task<List<StoreResponseDto>> GetStoresByCustomerId(Guid customerId , CancellationToken ct = default);



    /// <summary>
    /// Get all Stores
    /// </summary>
    /// <param name="customerId">customerId</param>
    /// <param name="keyword">keyword</param>
    /// <param name="pageIndex">page Index</param>
    /// <param name="pageSize">page size</param>
    /// <param name="ct">cancellation token</param>
    /// <returns>Store List with Pagination</returns>
    Task<PaginationResultDto<PagedList<StoreResponseDto>>> GetAllStores(Guid? customerId = null, string? keyword = null, int pageIndex = 0, int pageSize = 0, CancellationToken ct = default);


    /// <summary>
    /// Get all Grid Data
    /// </summary>
    /// <param name="StoreId">Store Identifier</param>
    /// <param name="ct">cancellation token</param>
    /// <returns>Store Grid Data</returns>
    Task<ResultDto<List<ChartGridDto>>> GetGridData(Guid StoreId, Guid? StoreDataId, bool IsGroup = false, CancellationToken ct = default);

    /// <summary>
    /// Get all Chart Data
    /// </summary>
    /// <param name="StoreId">Store Identifier</param>
    /// <param name="ct">cancellation token</param>
    /// <returns>Store Chart Data</returns>
    Task<ResultDto<List<ChartGraphDto>>> GetChartData(Guid StoreId, Guid? StoreDataId, string? type = null, CancellationToken ct = default);


    /// <summary>
    /// Get all Drawing Grid Data By store
    /// </summary>
    /// <param name="StoreId">Store Identifier</param>
    /// <param name="ct">cancellation token</param>
    /// <returns>Store Drawing Tye Grid Data</returns>
    Task<ResultDto<List<DrawingListResponseDto>>> GetDrawingGridData(Guid StoreId, Guid? StoreDataId, CancellationToken ct = default);



    /// <summary>
    /// Get all StoreStatus
    /// </summary>
    /// <param name="pageIndex">PageIndex</param>
    /// <param name="pageSize">PageSize</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Get Store status with Pagination</returns>
    Task<PaginationResultDto<PagedList<StoreStatusResponseDto>>> GetAllStoreStatus(string keyword = null, int pageIndex = 0, int pageSize = 0, CancellationToken ct = default);



    /// <summary>
    /// Get all StoreStatus
    /// </summary>
    /// <param name="pageIndex">PageIndex</param>
    /// <param name="pageSize">PageSize</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Get Store status with Pagination</returns>
    Task<PaginationResultDto<PagedList<StoreStatusResponseDto>>> GetAllStoreDataStatus(string keyword = null, int pageIndex = 0, int pageSize = 0, CancellationToken ct = default);

    /// <summary>
    /// Get Comparision chart data of Two versions
    /// </summary>
    /// <param name="FirstStoreId">First Store ID</param>
    /// <param name="FirstVersionId">First Version ID</param>
    /// <param name="SecondStoreId">Second Store ID</param>
    /// <param name="SecondVersionId">Second Version ID</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Comparision chart data</returns>
    Task<ResultDto<List<ComparisionChartGraphDto>>> CompareStoreVersionData(Guid FirstStoreId, Guid FirstVersionId, Guid SecondStoreId, Guid SecondVersionId, string? type = null,CancellationToken ct = default);

    /// <summary>
    /// Get all Order Grid Data
    /// </summary>
    /// <param name="StoreId">Store Identifier</param>
    /// <param name="ct">cancellation token</param>
    /// <returns>Store Grid Data</returns>
    Task<ResultDto<PackageDataDto>> GetOrderListGridData(Guid StoreId, CancellationToken ct = default);


    /// <summary>
    /// Get all General List Grid Data
    /// </summary>
    /// <param name="StoreId">Store Identifier</param>
    /// <param name="ct">cancellation token</param>
    /// <returns>Store Grid Data</returns>
    Task<ResultDto<List<GeneralListTypeDataDto>>> GetGeneralListTypeGridData(Guid StoreId, CancellationToken ct = default);

    /// <summary>
    /// Get Store Chart Data
    /// </summary>
    /// <param name="StoreId">Store Identifier</param>
    /// <param name="ct">cancellation token</param>
    /// <returns>Store Chart Data</returns>
    Task<ResultDto<List<StoreChartGraphDto>>> GetStoreChartData(Guid StoreId, Guid? StoreDataId, CancellationToken ct = default);


    /// <summary>
    /// Get all Grid Data of Department List
    /// </summary>
    /// <param name="StoreId">Store Identifier</param>
    /// <param name="ct">cancellation token</param>
    /// <returns>Store Department List Grid Data</returns>
    Task<ResultDto<DaparmentListDto>> GetDepartmentGridData(Guid StoreId, Guid? StoreDataId, CancellationToken ct = default);


    Task<ResultDto<CustomerStoresDto>> StoreDataByCustomerId(Guid CustomerId, string? type = null, CancellationToken ct = default);


    /// <summary>
    /// Get all Countries
    /// </summary>
    /// <param name="keyword">keyword</param>
    /// <param name="ct">cancellation token</param>
    /// <returns>Country List</returns>
    Task<ResultDto<List<string>>> GetAllCountries(string? keyword = null, Guid? customerId = null, CancellationToken ct = default);


    /// <summary>
    /// Get all Region by country
    /// </summary>
    /// <param name="keyword">keyword</param>
    /// <returns>Region List</returns>
    Task<ResultDto<List<string>>> GetAllRegionByCountry(string country, string? keyword = null, Guid? customerId =null, CancellationToken ct = default);
}
