using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Retail.DTOs;
using Retail.Services.Common;
using System.Net;

namespace RetailApp.Controllers;

public class BaseController : ControllerBase
{

    /// <summary>
    /// Generic Result Response
    /// </summary>
    /// <param name="result">result object</param>
    /// <returns>Return Generic Json response</returns>
    public IActionResult Result<T>(ResultDto<T> result)
    {


        if (result.StatusCode == HttpStatusCode.NotFound)
            return NotFound(result);

        if (result.StatusCode == HttpStatusCode.BadRequest)
            return BadRequest(result);

        if (result.StatusCode == HttpStatusCode.Unauthorized)
            return Unauthorized(result);

        return Ok(result);

    }

    /// <summary>
    /// Generic  Paged List Result Response
    /// </summary>
    /// <param name="result">result object</param>
    /// <returns>Return Generic Json Paged List Response</returns>
    public IActionResult Result<T>(PaginationResultDto<PagedList<T>> result)
    {


        if (result.StatusCode == HttpStatusCode.NotFound)
            return NotFound(result);

        if (result.StatusCode == HttpStatusCode.BadRequest)
            return BadRequest(result);

        if (result.StatusCode == HttpStatusCode.Unauthorized)
            return Unauthorized(result);

        return Ok(result);

    }

}

