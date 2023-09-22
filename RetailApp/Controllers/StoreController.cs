using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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


    #endregion

}
