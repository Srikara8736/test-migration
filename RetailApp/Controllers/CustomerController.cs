using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Retail.Data.Entities.Customers;
using Retail.DTOs;
using Retail.DTOs.Customers;
using Retail.Services.Customers;
using Retail.Services.Stores;
using RetailApp.Helpers;
using System.IO;

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

    private async Task<string> UploadCustomerLogo(IFormFile source, string customerId)
    {
        // var source = projectModel.Image;
        string Filename = source.FileName;
        string Filepath = GetLogoPath(customerId);

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
        return GetLogobyCustomerId(customerId);
    }



    private string GetLogobyCustomerId(string customerId)
    {
        string ImageUrl = string.Empty;
        string Filepath = GetLogoPath(customerId);
        string Imagepath = Filepath + "\\image.png";
        if (System.IO.File.Exists(Imagepath))
        {
            ImageUrl = customerId + "/image.png";

        }
        return ImageUrl;

    }


    private string GetLogoPath(string customerId)
    {
        return this._environment.WebRootPath + "\\ClientAssets\\CustomerLogo\\" + customerId;
    }


    private async Task<string> UploadImageAsync(IFormFile source, string storeId)
    {
        // var source = projectModel.Image;
        string Filename = source.FileName;
        string Filepath = GetImagePath(storeId);

        if (!System.IO.Directory.Exists(Filepath))
        {
            System.IO.Directory.CreateDirectory(Filepath);
        }

        string imagepath = Filepath + "\\" + Filename;

        if (System.IO.File.Exists(imagepath))
        {
            System.IO.File.Delete(imagepath);
        }
        using (FileStream stream = System.IO.File.Create(imagepath))
        {
            await source.CopyToAsync(stream);


        }
        return GetImagebyCustomerId(storeId, Filename);
    }
    

    private string GetImagePath(string customerId)
    {
        return this._environment.WebRootPath + "\\ClientAssets\\CustomerImage\\" + customerId;
    }
    private string GetImagebyCustomerId(string customerId, string fileName)
    {
        string ImageUrl = string.Empty;
        string Filepath = GetImagePath(customerId);
        string Imagepath = Filepath + "\\" + fileName;
        if (System.IO.File.Exists(Imagepath))
        {
            ImageUrl = customerId + "/" + fileName;
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
            var imgpath = await UploadCustomerLogo(customerModel.CustomerLogo, customer.Data.Id.ToString());

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
            var imgpath = await UploadCustomerLogo(customerModel.CustomerLogo, id.ToString());

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


    /// <summary>
    ///Update Customer Images
    /// </summary>
    /// <param name="customerId">customerId</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Return Customer information</returns>
    [HttpPut]
    [Route("UploadCustomerImage/{id}")]
    public async Task<IActionResult> UploadCustomerImage(string customerId, List<IFormFile> customerImage, CancellationToken ct = default)
    {

        var result = new ResultDto<bool>();

        foreach (var file in customerImage)
        {
            var imgUrl = await UploadImageAsync(file, customerId);

            if (imgUrl != null)
            {
                result = await _customerService.UploadCustomerImage(customerId, imgUrl, file.ContentType, file.ContentType);
            }
        }

        return this.Result(result);
    }

    [HttpDelete]
    [Route("DeleteCustomerImage")]
    public async Task<IActionResult> DeleteCustomerImage([BindRequired]Guid customerId, [BindRequired] Guid customerImageId,[BindRequired]Guid ImageId ,[BindRequired] string ImgUrl, CancellationToken ct = default)
    {

        var customerImage = await _customerService.DeleteCustomerImage(customerId, customerImageId, ImageId, ct);

        if (!customerImage.IsSuccess)
            return this.Result(customerImage);

        string Filepath = GetImagePath(customerId.ToString());
        var filename = Path.GetFileName(ImgUrl);
        string imagepath = Filepath + "\\" + filename;

        if (System.IO.File.Exists(imagepath))
        {
            System.IO.File.Delete(imagepath);
        }


        return this.Result(customerImage);

    }


    #endregion

    }
