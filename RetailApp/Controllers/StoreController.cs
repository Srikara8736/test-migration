﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Retail.DTOs.Roles;
using Retail.Services.Stores;
using Retail.Services.UserAccounts;
using RetailApp.Helpers;

namespace RetailApp.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class StoreController : BaseController
{

    #region Fields

    private readonly IStoreService _storeService;

    #endregion

    #region Ctor
    public StoreController(IStoreService storeService)
    {
        _storeService = storeService;

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





    #endregion

}
