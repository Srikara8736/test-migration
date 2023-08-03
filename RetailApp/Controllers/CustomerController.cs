using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Retail.Services.Customers;
using RetailApp.Helpers;

namespace RetailApp.Controllers;


[Authorize]
[Route("api/[controller]")]
[ApiController]
public class CustomerController : BaseController
{

    #region Fields

    private readonly ICustomerService _customerService;
    private readonly IWebHostEnvironment _environment;

    #endregion

    #region Ctor
    public CustomerController(ICustomerService customerService, IWebHostEnvironment environment)
    {
        _customerService = customerService;
        _environment = environment;
    }

    #endregion


    #region Utilities

    private async Task<string> UploadImageAsync(IFormFile source, string projectId)
    {
        // var source = projectModel.Image;
        string Filename = source.FileName;
        string Filepath = GetFilePath(projectId);

        if (!System.IO.Directory.Exists(Filepath))
        {
            System.IO.Directory.CreateDirectory(Filepath);
        }

        string imagepath = Filepath + "\\image.png";

        if (System.IO.File.Exists(imagepath))
        {
            System.IO.File.Delete(imagepath);
        }
        using (FileStream stream = System.IO.File.Create(imagepath))
        {
            await source.CopyToAsync(stream);


        }
        return GetImagebyProduct(projectId);
    }

    private string GetFilePath(string CustomerCode)
    {
        return this._environment.WebRootPath + "\\ClientAssets\\customerlogo\\" + CustomerCode;
    }

    private string GetImagebyProduct(string customerCode)
    {
        string ImageUrl = string.Empty;
        // string HostUrl = "https://localhost:7285/";
        string Filepath = GetFilePath(customerCode);
        string Imagepath = Filepath + "\\image.png";
        if (!System.IO.File.Exists(Imagepath))
        {
            ImageUrl = "/ClientAssets/common/noimage.png";
        }
        else
        {
            ImageUrl = "/ClientAssets/customerlogo/" + customerCode + "/image.png";
        }
        return ImageUrl;

    }

    #endregion


    #region Methods

    /// <summary>
    ///	Get all customer information, optionally with a paged list
    /// </summary>
    /// <param name="parameters">Filter parameter</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Return customers details with optionally with a paged list Response</returns>
    [HttpGet]
    [Route("Customers")]
    public async Task<IActionResult> GetAllCustomers([FromQuery] PagingParam parameters, CancellationToken ct = default)
    {

        return this.Result(await _customerService.GetAllCustomers(parameters.Keyword, parameters.PageIndex, parameters.PageSize, ct));
    }


    /// <summary>
    /// Get a Customer details by Identifier
    /// </summary>
    /// <param name="id">Identifier</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Return Customer Response</returns>
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetCustomerById(string id, CancellationToken ct)
    {

        return this.Result(await _customerService.GetCustomerById(new Guid(id), ct));

    }



    #endregion

}
