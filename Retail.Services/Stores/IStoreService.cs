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



    Task<ResultDto<List<AreaTypeGridDto>>> GetGridData(Guid StoreId, CancellationToken ct = default);
}
