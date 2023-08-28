using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Retail.DTOs;
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


    #region Utilities

    private async Task<string> UploadImageAsync(IFormFile source, string storeId)
    {
        // var source = projectModel.Image;
        string Filename = source.FileName;
        string Filepath = GetFilePath(storeId);

        if (!System.IO.Directory.Exists(Filepath))
        {
            System.IO.Directory.CreateDirectory(Filepath);
        }

        string imagepath = Filepath + "\\"+Filename;

        if (System.IO.File.Exists(imagepath))
        {
            System.IO.File.Delete(imagepath);
        }
        using (FileStream stream = System.IO.File.Create(imagepath))
        {
            await source.CopyToAsync(stream);


        }
        return GetImagebyStoreId(storeId, Filename);
    }

    private string GetFilePath(string storeId)
    {
        return this._environment.WebRootPath + "\\StoreAssets\\StoreImages\\" + storeId;
    }

    private string GetImagebyStoreId(string storeId,string fileName)
    {
        string ImageUrl = string.Empty;
        string Filepath = GetFilePath(storeId);
        string Imagepath = Filepath + "\\"+ fileName;
        if (System.IO.File.Exists(Imagepath))
        {
            ImageUrl = storeId + "/" + fileName ;
        }
      
        return ImageUrl;

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
    ///Update Store Images
    /// </summary>
    /// <param name="id">id</param>
    /// <param name="storeImage">Upload Store Images</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Return Customer infoormation</returns>
    [HttpPut]
    [Route("UploadStoreLogo/{id}")]
    public async Task<IActionResult> UploadStoreLogos(string id, List<IFormFile> storeImage, CancellationToken ct = default)
    {

        var result = new ResultDto<bool>();

        foreach (var file in storeImage)
        {
            var imgUrl = await UploadImageAsync(file, id);

            if (imgUrl != null)
            {
                result = await _storeService.UploadStoreImage(id, imgUrl, file.ContentType, file.ContentType);
            }
        }
        
        return this.Result(result);
    }



    #endregion

}
