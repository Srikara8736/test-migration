using AutoMapper;
using Retail.Data.Entities.Stores;
using Retail.Data.Repository;
using Retail.DTOs;
using Retail.DTOs.Customers;
using Retail.DTOs.Stores;
using Retail.Services.Common;
using System.Net;

namespace Retail.Services.Stores;

public class StoreService : IStoreService
{
    #region Fields

    private readonly RepositoryContext _repositoryContext;
    private readonly IMapper _mapper;

    #endregion

    #region Ctor
    public StoreService(RepositoryContext repositoryContext, IMapper mapper)
    {
        _repositoryContext = repositoryContext;
        _mapper = mapper;
    }
    #endregion

    /// <summary>
    /// gets all Stores
    /// </summary>
    /// <param name="customerId">customerId</param>
    /// <param name="keyword">keyword</param>
    /// <param name="pageIndex">page Index</param>
    /// <param name="pageSize">page size</param>
    /// <param name="ct">cancellation token</param>
    /// <returns>Store List with Pagination</returns>
    public async Task<PaginationResultDto<PagedList<StoreResponseDto>>> GetAllStores(Guid? customerId = null, string? keyword = null, int pageIndex = 0, int pageSize = 0, CancellationToken ct = default)
    {

        var query = from p in _repositoryContext.Stores
                    select p;



        if (customerId != null)
        {
            query = query.Where(x => x.CustomerId == customerId);
        }


        if (keyword != null)
        {
            query = query.Where(x => x.Name.Contains(keyword));
        }

        var stores = await PagedList<Store>.ToPagedList(query.OrderByDescending(on => on.Name), pageIndex, pageSize);

        if (stores == null)
        {
            var errorResponse = new PaginationResultDto<PagedList<StoreResponseDto>>
            {
                IsSuccess = false,
                ErrorMessage = StringResources.NoResultsFound,
                StatusCode = HttpStatusCode.NoContent
            };
        }

        var storeResponse = _mapper.Map<PagedList<StoreResponseDto>>(stores);

        foreach (var store in storeResponse)
        {
            if (store.AddressId != null)
            {
               
                var result = _repositoryContext.Addresses.Where(x => x.Id == store.AddressId).FirstOrDefault();
                if (result != null)
                {
                    var customerAddress = _mapper.Map<AddressDto>(result);
                    store.Address = customerAddress;
                }
;
            }

            if (store.CustomerId != null)
            {

                var result = _repositoryContext.Customers.Where(x => x.Id == store.CustomerId).FirstOrDefault();
                if (result != null)
                {
                    var customerAddress = _mapper.Map<CustomerDto>(result);
                    store.customer = customerAddress;
                }
;
            }




        }





        var response = new PaginationResultDto<PagedList<StoreResponseDto>>
        {
            IsSuccess = true,
            Data = storeResponse,
            TotalCount = stores.TotalCount,
            TotalPages = stores.TotalPages
        };
        return response;

    }


}
