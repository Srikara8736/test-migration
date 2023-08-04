using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Retail.Data.Entities.Stores;
using Retail.Data.Repository;
using Retail.DTOs;
using Retail.DTOs.Customers;
using Retail.DTOs.Roles;
using Retail.DTOs.Stores;
using Retail.Services.Common;
using System.Collections.Generic;
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



    public async Task<ResultDto<List<AreaTypeGridDto>>> GetGridData(Guid StoreId, CancellationToken ct = default)
    {

        var store = await _repositoryContext.Stores.FirstOrDefaultAsync(x => x.Id == StoreId, ct);
        if (store == null)
        {
            var storeResult = new ResultDto<List < AreaTypeGridDto>> ()
            {
                ErrorMessage = StringResources.NoResultsFound,
                StatusCode = HttpStatusCode.NotFound
            };
            return storeResult;
        }

        try
        {
            var query = (from at in _repositoryContext.AreaTypes
                         join cat in _repositoryContext.Categories on at.Id equals cat.AreaTypeId
                         join sp in _repositoryContext.Spaces on cat.Id equals sp.CategoryId
                         join stsp in _repositoryContext.StoreSpaces on sp.Id equals stsp.SpaceId
                         where stsp.StoreId == StoreId
                         select new
                         {
                             CategoryId = cat.Id,
                             CategoryName = cat.Name,
                             AreaTypeId = at.Id,
                             AreaTypeName = at.Name,
                             SpaceName = sp.Name,
                             SpaceUnit = stsp.Unit,
                             SpaceAtricles = stsp.Articles,
                             SpaceArea = stsp.Area,
                             SpacePieces = stsp.Pieces

                         }).ToList();



            var areaTypeGrid = new List<AreaTypeGridDto>();

            var areaTypeGroupResult = query.GroupBy(x => x.AreaTypeId).ToList();

            foreach (var item in areaTypeGroupResult)
            {
                var areaType = new AreaTypeGridDto();

                var categoryGrid = new List<CategoryGridDto>();

                var categoryGroup = item.GroupBy(x => x.CategoryId).ToList();
                foreach (var categoryResult in categoryGroup)
                {
                    var category = new CategoryGridDto();

                    var spaceGrid = new List<SpaceGridDto>();

                    foreach (var result in categoryResult)
                    {
                        areaType.AreaType = result.AreaTypeName.Trim();
                        areaType.AreaTypeId = result.AreaTypeId;

                        category.CategoryId = result.CategoryId;
                        category.Category = result.CategoryName.Trim();

                        var space = new SpaceGridDto
                        {
                            Space = result.SpaceName.Trim(),
                            Unit = result.SpaceUnit,
                            Area = (decimal)result.SpaceArea,
                            Pieces = (decimal)result.SpacePieces,
                            Atricles = (decimal)result.SpaceAtricles
                        };
                        spaceGrid.Add(space);

                    }
                    category.Spaces = spaceGrid;
                    category.TotalArea = spaceGrid.Sum(x => x.Area);
                    categoryGrid.Add(category);

                };


                areaType.Categories = categoryGrid;
                areaType.TotalArea = categoryGrid.Sum(x => x.TotalArea);
                areaTypeGrid.Add(areaType);
            }


            var successResponse = new ResultDto<List<AreaTypeGridDto>>
            {
                IsSuccess = true,
                Data = areaTypeGrid
            };

            return successResponse;

        }
        catch(Exception ex)
        {
            var storeResult = new ResultDto<List<AreaTypeGridDto>>()
            {
                ErrorMessage = StringResources.InvalidArgument,
                StatusCode = HttpStatusCode.InternalServerError
            };
            return storeResult;
        }
        

    }


   
}
