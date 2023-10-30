using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Retail.Data.Entities.Customers;
using Retail.Data.Entities.Stores;
using Retail.DTOs;
using Retail.DTOs.Cad;
using Retail.DTOs.Customers;
using Retail.DTOs.Stores;
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
    private readonly IStoreService _storeService;
    private readonly IWebHostEnvironment _environment;

    #endregion

    #region Ctor
    public CustomerController(ICustomerService customerService, IWebHostEnvironment environment, IStoreService storeService)
    {
        _customerService = customerService;
        _environment = environment;
        _storeService = storeService;
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

        var result = await _customerService.ResizeImage(source.OpenReadStream(), 150, 150, imagepath);        
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
    private async Task<string> UploadImageAsync(IFormFile source, string id,string type)
    {
        // var source = projectModel.Image;
        string Filename = source.FileName;
        string Filepath = string.Empty;

        if (type.ToLower().Trim() == "customer")
            Filepath = GetImagePath(id);
        else

            Filepath = GetStoreImagePath(id);

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

        var mainFileName = Path.GetFileNameWithoutExtension(source.FileName);

        string imageName150 = Filepath + "\\" + mainFileName + "_150.JPG";
        string imageName450 = Filepath + "\\" + mainFileName + "_450.JPG";

        var result_150 = await _customerService.ResizeImage(source.OpenReadStream(),250,150, imageName150);
        var result_450 = await _customerService.ResizeImage(source.OpenReadStream(), 750, 450, imageName450);

        return GetImagebyId(id, Filename, type);
    }    
    private string GetImagePath(string customerId)
    {
        return this._environment.WebRootPath + "\\ClientAssets\\CustomerImage\\" + customerId;
    }
    private string GetStoreImagePath(string storeId)
    {
        return this._environment.WebRootPath + "\\StoreAssets\\StoreImages\\" + storeId;
    }
    private string GetImagebyId(string id, string fileName,string type)
    {
        string ImageUrl = string.Empty;
        string Filepath = string.Empty;
        if (type.ToLower().Trim() == "customer")
             Filepath = GetImagePath(id);
        else

            Filepath = GetStoreImagePath(id);
        string Imagepath = Filepath + "\\" + fileName;
        if (System.IO.File.Exists(Imagepath))
        {
            ImageUrl = id + "/" + fileName;
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
    ///Upload Multiple Images
    /// </summary>
    /// <param name="id">Store Id / Customer Id</param>
    /// <param name="images">Store Images / Customer images</param>
    /// <param name="type">Type Should be either Store / Customer </param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Return Image Uploaded Status</returns>
    [HttpPost]
    [Route("UploadImages/{id}")]
    public async Task<IActionResult> UploadImages( string id, List<IFormFile> images, [BindRequired] string type, CancellationToken ct = default)
    {

        var result = new ResultDto<bool>();

        foreach (var file in images)
        {
            var imgUrl = await UploadImageAsync(file, id, type);

            if (imgUrl != null)
            {
                if (type.ToLower().Trim() == "customer")
                    result = await _customerService.UploadCustomerImage(id, imgUrl, file.ContentType, file.ContentType);
                else
                    result = await _storeService.UploadStoreImage(id, imgUrl, file.ContentType, file.ContentType);


            }
        }

        return this.Result(result);
    }

    /// <summary>
    ///Delete a Image
    /// </summary>
    /// <param name="deleteImageRequest">Object Model</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Return Image deleted Status</returns>

    [HttpDelete]
    [Route("DeleteImage")]
    public async Task<IActionResult> DeleteImage([FromBody] DeleteImageDto deleteImageRequest, CancellationToken ct = default)
    {

        var deleteImage = new ResultDto<bool>();
        if (deleteImageRequest.Type.ToLower().Trim() == "customer")
            deleteImage = await _customerService.DeleteCustomerImage(deleteImageRequest.Id, deleteImageRequest.ImageMidId, deleteImageRequest.ImageId, ct);
        else
            deleteImage = await _storeService.DeleteStoreImage(deleteImageRequest.Id, deleteImageRequest.ImageMidId, deleteImageRequest.ImageId, ct);

        if (!deleteImage.IsSuccess)
            return this.Result(deleteImage);

        string Filepath = string.Empty;

        if (deleteImageRequest.Type.ToLower().Trim() == "customer")
            Filepath = GetImagePath(deleteImageRequest.Id.ToString());
        else

            Filepath = GetStoreImagePath(deleteImageRequest.Id.ToString());


        var fileFullName = Path.GetFileName(deleteImageRequest.ImgUrl);


        string fileName = Path.GetFileNameWithoutExtension(fileFullName);



        DirectoryInfo dir = new DirectoryInfo(Filepath);
        FileInfo[] filesInDir = dir.GetFiles("*" + fileName + "*.*");


        foreach (var item in filesInDir)
        {
            if (System.IO.File.Exists(item.FullName))
            {
                System.IO.File.Delete(item.FullName);
            }           
        }


        return this.Result(deleteImage);

    }


    #endregion

    }
