using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Retail.DTOs.Roles;
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
    /// <param name="ct">cancellation token</param>
    /// <returns>Store Grid Data</returns>
    [HttpGet]
    [Route("GetGridData")]
    public async Task<IActionResult> GetGridData([Required]Guid StoreId, CancellationToken ct = default)
    {

        return this.Result(await _storeService.GetGridData(StoreId, ct));
    }

    /// <summary>
    /// Gets all Chart Data of Store
    /// </summary>
    /// <param name="StoreId">Store Identifier</param>
    /// <param name="ct">cancellation token</param>
    /// <returns>Store Chart Data</returns>
    [HttpGet]
    [Route("GetChartData")]
    public async Task<IActionResult> GetChartData([Required] Guid StoreId, CancellationToken ct = default)
    {

        return this.Result(await _storeService.GetChartData(StoreId, ct));
    }


    /// <summary>
    /// Gets Drawing Grid Data of Store
    /// </summary>
    /// <param name="StoreId">Store Identifier</param>
    /// <param name="ct">cancellation token</param>
    /// <returns>Store Grid Data</returns>
    [HttpGet]
    [Route("GetDrawingGridData")]
    public async Task<IActionResult> GetDrawingGridData([Required] Guid StoreId, CancellationToken ct = default)
    {

        return this.Result(await _storeService.GetDrawingGridData(StoreId, ct));
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


    [HttpPost]
    [Route("StoreComparision")]
    public async Task<IActionResult> StoreComparision([FromBody] StoreComparisionRequestDto storeComparision, CancellationToken ct = default)
    {

        return this.Result(await _storeService.CompareStoreVersionData(storeComparision.FirstStoreId, storeComparision.FirstVersionId, storeComparision.SecondStoreId,storeComparision.SecondVersionId, ct));
    }

    #endregion

}
