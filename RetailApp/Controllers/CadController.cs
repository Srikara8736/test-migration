using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Retail.Data.Entities.UserAccount;
using Retail.DTOs.Cad;
using Retail.DTOs.UserAccounts;
using Retail.DTOs.XML;
using Retail.Services.Cad;
using RetailApp.Authentication;
using RetailApp.Configuration;
using System.Collections;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace RetailApp.Controllers
{
   [Authorize]
    [Route("api/")]
    [ApiController]
    public class CadController : BaseController
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

        private async Task<(string filepath, string strem)> UploadCadFileAsync(IFormFile source, string storeId)
        {
            string Filepath = GetFilePath(storeId);
           // string Filename = source.FileName;

            string FilenameGen = Path.GetFileNameWithoutExtension(source.FileName);
            string Exten = Path.GetExtension(source.FileName);
            long time = DateTime.Now.Ticks;

            var Filename = FilenameGen + Convert.ToString(time) + Exten;


            if (!System.IO.Directory.Exists(Filepath))
            {
                System.IO.Directory.CreateDirectory(Filepath);
            }

            if (System.IO.File.Exists(Filepath + "\\" + Filename))
            {
                System.IO.File.Delete(Filepath + "\\" + Filename);
            }

          

            using (FileStream stream = System.IO.File.Create(Filepath + "\\" + Filename))
            {
                await source.CopyToAsync(stream);
               
            }

            var result = Path.GetFileNameWithoutExtension(Filepath + "\\" + Filename);

            var extractedpath = (Filepath + "\\Extracted\\");

            if (!System.IO.Directory.Exists(extractedpath))
            {
                System.IO.Directory.CreateDirectory(extractedpath);
            }

            ZipFile.ExtractToDirectory(Filepath + "\\" + Filename, extractedpath + result);

            var fileLocpath = GetFilePathWithoutWebRoot(storeId, result);

            return (fileLocpath, Filepath + "\\" + Filename);
        }

        private string GetFilePath(string storeId)
        {
            return this._environment.WebRootPath + "\\StoreAssets\\StoreFiles\\" + storeId;
        }

        private string GetFilePathWithoutWebRoot(string storeId,string fileName)
        {
            return "\\StoreAssets\\StoreFiles\\" + storeId+ "\\Extracted"+"\\"+ fileName;
        }

        private List<string> ManifestReader(string manifestContent, string xPath)
        {
            XmlDocument xmlManifest = new XmlDocument();
            xmlManifest.LoadXml(manifestContent);
            XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(xmlManifest.NameTable);
            xmlNamespaceManager.AddNamespace("cad", "urn:ixicadpackage-schema");
            XmlNodeList xmlNode = xmlManifest.DocumentElement.SelectNodes(xPath, xmlNamespaceManager);
            return xmlNode.Cast<XmlNode>()
                           .Select(node => node.InnerText).ToList();
        }

        private byte[] ZipStreamReader(ZipArchive zip, string fileName)
        {
            MemoryStream ms = null;
            var zipArchiveEntry = zip.GetEntry(fileName);
            if (zipArchiveEntry != null)
            {
                using (var unzippedEntryStream = zipArchiveEntry.Open())
                {
                    using (ms = new MemoryStream())
                    {
                        unzippedEntryStream.CopyTo(ms);
                    }
                }
            }
            return ms?.ToArray();
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
        public List<Retail.DTOs.Cad.Store> GetStoresByCustomerNo(string customerNo)
        {

            return _cadService.GetStoresByCustomerNo(customerNo);
        }


        [HttpPost]
        [Route("Cad/UploadCad")]
        public async Task<object> UploadCad([FromForm]CadUploadDto cadUpload)
        {
            if (cadUpload.CadFile.ContentType != "application/x-zip-compressed")
            {        

                return HttpStatusCode.UnsupportedMediaType;
            }


            //try
            //{

            var fileStream = await UploadCadFileAsync(cadUpload.CadFile, cadUpload.StoreId.ToString());

           // string manifestConten1t = Encoding.UTF8.GetString(ZipStreamReader(fileStream.strem, "Manifest.xml"));
            var stream = cadUpload.CadFile.OpenReadStream();
            using (var zip = new ZipArchive(stream, ZipArchiveMode.Read))
            {
                //Fetch Manifest.xml
                //Extract the Manifest file
                string manifestContent = Encoding.UTF8.GetString(ZipStreamReader(zip, "Manifest.xml"));


                List<string> cadFileNames = null;
                List<string> cadPDFFileNames = null;

                if (manifestContent != string.Empty)
                {
                    string xPath = "//cad:CADXML/@name";
                    cadFileNames = ManifestReader(manifestContent, xPath);

                    string xpdfPath = "//cad:CADPDF/@name";
                    cadPDFFileNames = ManifestReader(manifestContent, xpdfPath);
                }


                foreach (string cadFileName in cadFileNames)
                {


                    byte[] CADContent = ZipStreamReader(zip, cadFileName);

                    XmlSerializer serializer = new XmlSerializer(typeof(CADData));

                    CADData cadData = (CADData)serializer.Deserialize(new MemoryStream(CADContent));

                   if(cadData != null)
                    {
                      // var loadXml = await _cadService.LoadXMLData(items);
                    }

                    var document = await _cadService.InsertDocument(cadFileName, fileStream.filepath + "\\" + cadFileName, Guid.Parse("bdfb90b9-3dd3-4347-aeb8-3c5d7be6ec6a"));
                    if(document != null)
                    {
                        var storeDocument = await _cadService.InsertStoreDocument(cadUpload.StoreId, document.Id);
                    }


                }


                foreach (string cadpdfFileName in cadPDFFileNames)
                {

                    var document = await _cadService.InsertDocument(cadpdfFileName, fileStream.filepath + "\\" + cadpdfFileName, Guid.Parse("8ae98fb5-16ed-429e-af96-83b6caec15a5"));
                    if (document != null)
                    {
                        var storeDocument = await _cadService.InsertStoreDocument(cadUpload.StoreId, document.Id);
                    }


                }


            }

            var caduploadHistory = _cadService.InsertCadUploadHistory(cadUpload.StoreId, cadUpload.CadFile.FileName);

            return this.Ok();

            //}
            //catch (Exception ex)
            //{

            //    return HttpStatusCode.InternalServerError;
            //}

        }


        /// <summary>
        ///Get a cad uploaded hitory by store Identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns>Return Role Response</returns>
        [HttpGet]
        [Route("CadHistoryByStore/{id}")]
        public async Task<IActionResult> CadHistoryByStore(Guid id, CancellationToken ct)
        {

            return this.Result(await _cadService.GetCadUploadHistoryByStore(id, ct));

        }

    }
}
