﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Retail.DTOs.Master;
using Retail.DTOs.Roles;
using Retail.Services.Master;
using Retail.Services.UserAccounts;
using RetailApp.Helpers;

namespace RetailApp.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class CodeMasterController : BaseController
{

    #region Fields

    private readonly ICodeMasterService _codeMasterService;

    #endregion

    #region Ctor
    public CodeMasterController(ICodeMasterService codeMasterService)
    {
        _codeMasterService = codeMasterService;

    }

    #endregion


    #region Methods

    /// <summary>
    ///Get All GetAllStoreStatus optionally with paged List
    /// </summary>    
    /// <param name="parameters">parameters</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Return Store Status optionally with paged List Response</returns>
    [HttpGet]
    [Route("GetAllStoreStatus")]
    public async Task<IActionResult> GetAllStoreStatus([FromQuery] CustomerParam param, CancellationToken ct = default)
    {

        return this.Result(await _codeMasterService.GetAllStoreStatus(param.Keyword, param.CustomerId, param.PageIndex, param.PageSize, ct));
    }


    /// <summary>
    ///Get a Status details by Identifier
    /// </summary>
    /// <param name="id">Identifier</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Return Status Response</returns>
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetStatusById(Guid id,Guid? customerId, CancellationToken ct)
    {

        return this.Result(await _codeMasterService.GetStatusById(id, customerId, ct));

    }

    /// <summary>
    ///Add a new Status item
    /// </summary>
    /// <param name="codeMaster">codeMasters</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Return newly added status</returns>
    [HttpPost]
    public async Task<IActionResult> InsertStatus([FromBody] CodeMasterDto codeMaster, CancellationToken ct = default)
    {

        return this.Result(await _codeMasterService.InsertStatus(codeMaster, ct));

    }

    /// <summary>
    /// Update a Status Details
    /// </summary>
    /// <param name="codeMaster">codeMaster</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Return updated Status Information</returns>
    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> UpdateStatus(Guid id,[BindRequired]Guid CustomerId, [FromBody] CodeMasterDto codeMaster, CancellationToken ct)
    {
        return this.Result(await _codeMasterService.UpdateStatus(id, codeMaster, ct));
    }


    /// <summary>
    ///Add a new Customer Status item
    /// </summary>
    /// <param name="codeMaster">codeMasters</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Return newly added status</returns>
    [HttpPost]
    [Route("{InsertCustomerStatus}")]
    public async Task<IActionResult> InsertCustomerStatus([FromBody] CustomerCodeMasterDto codeMaster, CancellationToken ct = default)
    {

        return this.Result(await _codeMasterService.InsertCustomerStatus(codeMaster, ct));

    }


    /// <summary>
    /// Delete a Role item
    /// </summary>
    /// <param name="id">id</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Return Deleted Status</returns>
    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteStatus(Guid id, CancellationToken ct)
    {
        return this.Result(await _codeMasterService.DeleteStatus(id, ct));

    }

    #endregion

}
