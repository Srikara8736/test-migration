using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Retail.Data.Entities.Customers;
using Retail.DTOs.Customers;
using Retail.Services.Customers;
using Retail.Services.Stores;
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

    private async Task<string> UploadImageAsync(IFormFile source, string customerId)
    {
        // var source = projectModel.Image;
        string Filename = source.FileName;
        string Filepath = GetFilePath(customerId);

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
        return GetImagebyCustomerId(customerId);
    }

    private string GetFilePath(string customerId)
    {
        return this._environment.WebRootPath + "\\ClientAssets\\CustomerLogo\\" + customerId;
    }

    private string GetImagebyCustomerId(string customerId)
    {
        string ImageUrl = string.Empty;
        string Filepath = GetFilePath(customerId);
        string Imagepath = Filepath + "\\image.png";
        if (System.IO.File.Exists(Imagepath))
        {
            ImageUrl = customerId + "/image.png";
    
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


    /// <summary>
    /// Add a new Customer item
    /// </summary>
    /// <param name="customerModel">Customer Information</param>  
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Return newly added Customer</returns>
    [HttpPost]
    public async Task<IActionResult> AddCustomer([FromForm] CustomerDto customerModel, CancellationToken ct = default)
    {
        var customer = await _customerService.InsertCustomer(customerModel, ct);

        if (customerModel.CustomerLogo != null & customer.Data != null)
        {
            var imgpath = await UploadImageAsync(customerModel.CustomerLogo, customer.Data.Id.ToString());

            if(imgpath != null)
                customer = await _customerService.UploadLogoByCustomerId(customer.Data.Id, imgpath, customerModel.CustomerLogo.ContentType, customerModel.CustomerLogo.ContentType, ct);
        }
        return this.Result(customer);

    }

    /// <summary>
    /// Update a Customer item
    /// </summary>
    /// <param name="id">Customer Identifier</param>
    /// <param name="customerModel">Customer infromation</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Return updated Customer Information</returns>
    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> UpdateCustomer(Guid id, [FromForm] CustomerDto customerModel, CancellationToken ct)
    {
        if (customerModel.CustomerLogo != null )
        {
            var imgpath = await UploadImageAsync(customerModel.CustomerLogo, id.ToString());

            if(customerModel.LogoImageId == null)
            {
                if (imgpath != null)
                    await _customerService.UploadLogoByCustomerId(id, imgpath, customerModel.CustomerLogo.ContentType, customerModel.CustomerLogo.ContentType, ct);
            }

          
        }
        return this.Result(await _customerService.UpdateCustomer(id, customerModel, ct));
    }




    /// <summary>
    /// Delete a Customer item
    /// </summary>
    /// <param name="id">id</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Return Customer Deleted Status</returns>
    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteCustomer(string id, CancellationToken ct)
    {
        return this.Result(await _customerService.DeleteCustomer(new Guid(id), ct));

    }




    #endregion

}
