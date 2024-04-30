using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Retail.DTOs.Stores;
using Retail.Services.Stores;
using RetailApp.Helpers;
using System.ComponentModel.DataAnnotations;

namespace RetailApp.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class StoreController : BaseController
{

    #region Fields

    private readonly IStoreService _storeService;
    private readonly IWebHostEnvironment _environment;


    #endregion

    #region Ctor
    public StoreController(IStoreService storeService, IWebHostEnvironment environment)
    {
        _storeService = storeService;
        _environment = environment;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Get All Store details optionally with paged List
    /// </summary>
    /// <param name="param">Store model parameters</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Return Main Project details optionally with paged List</returns>
    [HttpGet]
    [Route("Stores")]
    public async Task<IActionResult> GetAllStores([FromQuery] CustomerParam param, CancellationToken ct = default)
    {

        return this.Result(await _storeService.GetAllStores(param.CustomerId,param.Keyword, param.PageIndex, param.PageSize, ct));
    }


    /// <summary>
    ///Get a Store details by Identifier
    /// </summary>
    /// <param name="id">Identifier</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Return Store Response</returns>
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetStoreById(Guid id, CancellationToken ct)
    {

        return this.Result(await _storeService.GetStoreById(id, ct));

    }

    /// <summary>
    ///Add a new Store item
    /// </summary>
    /// <param name="storeModel">Store Model</param>
    /// <returns>Return newly added Store</returns>
    [HttpPost]
    public async Task<IActionResult> AddStore([FromBody] StoreDto storeModel, CancellationToken ct = default)
    {

        return this.Result(await _storeService.InsertStore(storeModel, ct));

    }

    /// <summary>
    /// Update a Store Details
    /// </summary>
    /// <param name="id">Store Id</param>
    /// <param name="storeModel">Store Model</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Return updated Store Information</returns>
    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> UpdateStore(string id, [FromBody] StoreDto storeModel, CancellationToken ct)
    {
        return this.Result(await _storeService.UpdateStore(id, storeModel, ct));
    }


    /// <summary>
    /// Update a Store Data Details
    /// </summary>
    /// <param name="id">Store Data Id</param>
    /// <param name="storeStatusModel">Store Model</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Return updated Store Information</returns>
    [HttpPut]
    [Route("UpdateStoreData/{id}")]
    public async Task<IActionResult> UpdateStoreData(Guid id, [FromBody] StoreDataStatusDto storeStatusModel, CancellationToken ct)
    {
        return this.Result(await _storeService.UpdateStoreData(id, storeStatusModel,ct));
    }


    /// <summary>
    /// Delete a Store item
    /// </summary>
    /// <param name="id">id</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Return Deleted Status</returns>
    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteStore(string id, CancellationToken ct)
    {
        return this.Result(await _storeService.DeleteStore(id, ct));

    }


    /// <summary>
    /// Gets all Grid Data of Store
    /// </summary>
    /// <param name="StoreId">Store Identifier</param>
    /// <param name="StoreDataId">Store Data Identifier</param>
    /// <param name="IsGroup">Group Identifier</param>
    /// <param name="ct">cancellation token</param>
    /// <returns>Store Grid Data</returns>
    [HttpGet]
    [Route("GetGridData")]
    public async Task<IActionResult> GetGridData([Required]Guid StoreId,Guid? StoreDataId, bool IsGroup = false, CancellationToken ct = default)
    {

        return this.Result(await _storeService.GetGridData(StoreId, StoreDataId, IsGroup, ct));
    }

    /// <summary>
    /// Gets all Chart Data of Store
    /// </summary>
    /// <param name="StoreId">Store Identifier</param>
    /// <param name="StoreDataId">Store Data Identifier</param>
    /// <param name="type">Space / Department / Grouping</param>
    /// <param name="ct">cancellation token</param>
    /// <returns>Store Chart Data</returns>
    [HttpGet]
    [Route("GetChartData")]
    public async Task<IActionResult> GetChartData([Required] Guid StoreId, Guid? StoreDataId, string? type = null, CancellationToken ct = default)
    {

        return this.Result(await _storeService.GetChartData(StoreId, StoreDataId, type, ct));
    }



    /// <summary>
    /// Gets all Chart Data of Store
    /// </summary>
    /// <param name="StoreId">Store Identifier</param>
    /// <param name="ct">cancellation token</param>
    /// <returns>Store Chart Data</returns>
    [HttpGet]
    [Route("GetChartDataByStore")]
    public async Task<IActionResult> GetChartDataByStore([Required] Guid StoreId, Guid? StoreDataId, CancellationToken ct = default)
    {
        return this.Result(await _storeService.GetStoreChartData(StoreId, StoreDataId, ct));
    }


    /// <summary>
    /// Gets Drawing Grid Data of Store
    /// </summary>
    /// <param name="StoreId">Store Identifier</param>
    /// <param name="ct">cancellation token</param>
    /// <returns>Store Grid Data</returns>
    [HttpGet]
    [Route("GetDrawingGridData")]
    public async Task<IActionResult> GetDrawingGridData([Required] Guid StoreId, Guid? StoreDataId, CancellationToken ct = default)
    {

        return this.Result(await _storeService.GetDrawingGridData(StoreId,StoreDataId, ct));
    }




    /// <summary>
    /// Gets Order Grid Data of Store
    /// </summary>
    /// <param name="StoreId">Store Identifier</param>
    /// <param name="ct">cancellation token</param>
    /// <returns>Store Grid Data</returns>
    [HttpGet]
    [Route("GetOrderListData")]
    public async Task<IActionResult> GetOrderListData([Required] Guid StoreId, CancellationToken ct = default)
    {

        return this.Result(await _storeService.GetOrderListGridData(StoreId, ct));
    }



    /// <summary>
    /// Gets General List Grid Data of Store
    /// </summary>
    /// <param name="StoreId">Store Identifier</param>
    /// <param name="ct">cancellation token</param>
    /// <returns>Store Grid Data</returns>
    [HttpGet]
    [Route("GetGeneralListTypeGridData")]
    public async Task<IActionResult> GetGeneralListTypeGridData([Required] Guid StoreId, CancellationToken ct = default)
    {

        return this.Result(await _storeService.GetGeneralListTypeGridData(StoreId, ct));
    }


    [HttpGet]
    [Route("GetDepartmentListGridData")]
    public async Task<IActionResult> GetDepartmentListGridData([Required] Guid StoreId, Guid? StoreDataId, CancellationToken ct = default)
    {

        return this.Result(await _storeService.GetDepartmentGridData(StoreId, StoreDataId, ct));
    }



    /// <summary>
    /// Get all StoreStatus
    /// </summary>
    /// <param name="parameters">Search parameters</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Return StoreStatus optionally with paged List Response</returns>
    [HttpGet]
    [Route("GetStoreStatus")]
    public async Task<IActionResult> GetStoreStatus([FromQuery] PagingParam parameters, CancellationToken ct = default)
    {

        return this.Result(await _storeService.GetAllStoreStatus(parameters.Keyword, parameters.PageIndex, parameters.PageSize, ct));
    }



    /// <summary>
    /// Get all Store Data Status
    /// </summary>
    /// <param name="parameters">Search parameters</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Return StoreStatus optionally with paged List Response</returns>
    [HttpGet]
    [Route("GetStoreDataStatus")]
    public async Task<IActionResult> GetStoreDataStatus([FromQuery] PagingParam parameters, CancellationToken ct = default)
    {

        return this.Result(await _storeService.GetAllStoreDataStatus(parameters.Keyword, parameters.PageIndex, parameters.PageSize, ct));
    }



    [HttpPost]
    [Route("StoreComparision")]
    public async Task<IActionResult> StoreComparision([FromBody] StoreComparisionRequestDto storeComparision, CancellationToken ct = default)
    {

        return this.Result(await _storeService.CompareStoreVersionData(storeComparision.FirstStoreId, storeComparision.FirstVersionId, storeComparision.SecondStoreId,storeComparision.SecondVersionId, storeComparision.Type, ct));
    }


    /// <summary>
    /// Gets Store List By Customer
    /// </summary>
    /// <param name="CustomerId">Customer Identifier</param>
    /// <param name="type">Space / Department / Grouping</param>
    /// <param name="ct">cancellation token</param>
    /// <returns>Store List By Customer</returns>
    [HttpGet]
    [Route("GetStoreListByCustomer")]
    public async Task<IActionResult> GetStoreListByCustomer([Required] Guid CustomerId, string? type = null, CancellationToken ct = default)
    {

        return this.Result(await _storeService.StoreDataByCustomerId(CustomerId, type, ct));
    }

    #endregion


    #region Country & Region

    /// <summary>
    /// Get all Countries
    /// </summary>
    /// <param name="keyword">keyword</param>
    /// <param name="ct">cancellation token</param>
    /// <returns>Country List</returns>
    [HttpGet]
    [Route("GetAllCountries")]
    public async Task<IActionResult> GetAllCountries(string? keyword = null, CancellationToken ct = default)
    {
        return this.Result(await _storeService.GetAllCountries(keyword, ct));
    }



    /// <summary>
    /// Get all Region
    /// </summary>
    /// <param name="country">country</param>
    /// <param name="keyword">keyword</param>
    /// <param name="ct">cancellation token</param>
    /// <returns>Country List</returns>
    [HttpGet]
    [Route("GetAllRegionByCountry")]
    public async Task<IActionResult> GetAllRegionByCountry([BindRequired] string country, string? keyword = null, CancellationToken ct = default)
    {
        return this.Result(await _storeService.GetAllRegionByCountry(country, keyword, ct));
    }
    #endregion

}
