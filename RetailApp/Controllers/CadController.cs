using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Retail.DTOs.Cad;
using Retail.DTOs.UserAccounts;
using Retail.Services.Cad;
using RetailApp.Authentication;
using System.IO.Compression;
using System.Net;

namespace RetailApp.Controllers
{
    //[Authorize]
    [Route("api/")]
    [ApiController]
    public class CadController : ControllerBase
    {
        #region Fields

        private readonly ICadService _cadService;
        private readonly IAuthTokenBuilder _authTokenBuilder;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _environment;

        #endregion

        #region Ctor
        public CadController(ICadService cadService, IAuthTokenBuilder authTokenBuilder, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment environment)
        {
            _cadService = cadService;
            _authTokenBuilder = authTokenBuilder;
            _httpContextAccessor = httpContextAccessor;
            _environment = environment;
        }

        #endregion


        #region Utilities

        private async void UploadCadFileAsync(IFormFile source)
        {
            string Filepath = GetFilePath(source.FileName);
            string Filename = source.FileName;


            if (!System.IO.Directory.Exists(Filepath))
            {
                System.IO.Directory.CreateDirectory(Filepath);
            }

            if (System.IO.File.Exists(Filepath))
            {
                System.IO.File.Delete(Filepath);
            }
            using (FileStream stream = System.IO.File.Create(Filepath + "\\" + Filename))
            {
                await source.CopyToAsync(stream);

            }
            var result = Path.GetFileNameWithoutExtension(Filepath + "\\" + Filename);
            ZipFile.ExtractToDirectory(Filepath + "\\" + Filename, Filepath + "\\" + result);
        }

        private string GetFilePath(string fileName)
        {
            return this._environment.WebRootPath + "\\StoreAssets\\StoreFiles\\" + fileName;
        }


        #endregion




        [AllowAnonymous]
        [HttpPost]
        [Route("External/Interior/AuthenticateUser")]
        public object AuthenticateUser([FromBody] Login model)
        {
            if (!string.IsNullOrEmpty(model.UserName) && !string.IsNullOrEmpty(model.Password))
            {
                var login = new LoginRequestDTO()
                {
                    Email= model.UserName,
                    Password= model.Password
                };

                var ip = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();

                var Item =  _authTokenBuilder.AuthTokenGeneration(login, ip).Result;

                if (!Item.IsSuccess)
                    return Unauthorized();

               
                return Item.Data.AccessToken;


            }
            else
            {
                return Unauthorized();
            }

        }


        [HttpGet]
        [Route("External/Customers/GetAllcustomers")]
        public List<CustomerItem> GetAllCustomers()
        {
            return _cadService.GetAllCustomer();
        }


        [HttpGet]
        [Route("External/Customers/GetStoresByCustomerNo")]
        public List<Store> GetStoresByCustomerNo(string customerNo)
        {

            return _cadService.GetStoresByCustomerNo(customerNo);
        }


        [HttpPost]
        [Route("Cad/UploadCad")]
        public object UploadCad([BindRequired] IFormFile cadFile)
        {
            if (cadFile.ContentType != "application/x-zip-compressed")
            {        

                return HttpStatusCode.UnsupportedMediaType;
            }


            try
            {
                UploadCadFileAsync(cadFile);

                return this.Ok();
            }
            catch (Exception ex)
            {
             
                return HttpStatusCode.InternalServerError;
            }

        }



    }
}
