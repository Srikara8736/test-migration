using Retail.Data.Entities.FileSystem;
using Retail.Data.Entities.Stores;
using Retail.DTOs;
using Retail.DTOs.Stores;
using Retail.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retail.Services.Stores;

public interface IStoreService
{

    Task<List<StoreImage>> GetStoreImagesByStoreId(Guid storeId);

    Task<Image> GetImageById(Guid id, CancellationToken ct = default);

    Task<Image> InsertImage(Image image);
    Task<StoreImage> InsertStoreImage(StoreImage image);

    Task<ResultDto<bool>> UploadStoreImage(string storeId, string imgUrl, string fileType, string fileExtension);

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
}
