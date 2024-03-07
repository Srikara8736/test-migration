using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Retail.Data.Entities.Stores;
using Retail.DTOs.Cad;
using Retail.DTOs.UserAccounts;
using Retail.DTOs.XML;
using Retail.Services.Cad;
using RetailApp.Authentication;
using System.ComponentModel.DataAnnotations;
using System.IO.Compression;
using System.Net;
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
        private readonly IMapper _mapper;

        #endregion

        #region Ctor
        public CadController(ICadService cadService, IAuthTokenBuilder authTokenBuilder, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment environment,IMapper mapper)
        {
            _cadService = cadService;
            _authTokenBuilder = authTokenBuilder;
            _httpContextAccessor = httpContextAccessor;
            _environment = environment;
            _mapper = mapper;
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
                    UserName = model.UserName,
                    Password= model.Password
                };

                var ip = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();

                var Item =  _authTokenBuilder.AuthTokenGeneration(login, ip).Result;

                if (!Item.IsSuccess)
                    return Unauthorized();

                var customer = Item.Data.Customer;
                return new
                {
                    accessToken = Item.Data.AccessToken,
                    customerName = customer.Name,
                    customerNo = customer.Id

                };


            }
            else
            {
                return Unauthorized();
            }

        }


        //[HttpGet]
        //[Route("External/Customers/GetAllcustomers")]
        //public List<CustomerItem> GetAllCustomers()
        //{
        //    return _cadService.GetAllCustomer();
        //}


        /// <summary>
        ///Get Stores using CustomerNo
        /// </summary>
        /// <returns>Customer Stores </returns>

        [HttpGet]
        [Route("External/Customers/GetStoresByCustomerNo")]
        public List<Retail.DTOs.Cad.Store> GetStoresByCustomerNo(string customerNo)
        {

            return _cadService.GetStoresByCustomerNo(customerNo);
        }


        /// <summary>
        ///Cad File Upload Version 1
        /// </summary>
        /// <param name="cadUpload">Cad Upload</param>
        /// <returns>Return Cad Upload  Status Response</returns>
        [HttpPost]
        [Route("Cad/UploadCad")]
        public async Task<object> UploadCad([FromForm]CadUploadDto cadUpload)
        {
            if (cadUpload.CadFile.ContentType != "application/x-zip-compressed")
            {        

                return HttpStatusCode.UnsupportedMediaType;
            }

            Guid? storeDataId = null;
            try
            {


                var fileStream = await UploadCadFileAsync(cadUpload.CadFile, cadUpload.StoreId.ToString());
                var caduploadHistory = await _cadService.InsertCadUploadHistory(cadUpload.StoreId, cadUpload.CadFile.FileName);
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

                    
                    if(cadUpload.Type.ToLower() == "space")
                        {
                       
                            Message cadData = new Message();
                            XmlSerializer serializer = new XmlSerializer(typeof(Message));
                                try
                                {
                                     cadData = (Message)serializer.Deserialize(new MemoryStream(CADContent));
                                }
                                catch (Exception ex)
                                {
                                    return new
                                    {
                                        IsSuccess = false,
                                        statusCode = HttpStatusCode.InternalServerError,
                                        message = "XML may not be valid format"
                                    };
                                }
                                if (cadData != null)
                                {
                                        var loadXml = await _cadService.LoadXMLData(cadData, cadUpload.StoreId, "Space", caduploadHistory.Id);
                                        storeDataId = loadXml.storeDataId;
                                }
                        
                        }
                        else if (cadUpload.Type.ToLower() == "order")
                        {
                            CADOrder cadData = new CADOrder();
                            XmlSerializer serializer = new XmlSerializer(typeof(CADOrder));
                            try
                            {
                                cadData = (CADOrder)serializer.Deserialize(new MemoryStream(CADContent));
                            }
                            catch (Exception ex)
                            {
                                return new
                                {
                                    IsSuccess = false,
                                    statusCode = HttpStatusCode.InternalServerError,
                                    message = "Order XML may not be valid, "
                                };
                            }
                            if (cadData != null)
                            {
                                var loadXml = await _cadService.LoadOrderListData(cadUpload.StoreId, cadData.MessageBlock);
                            }
                        }
                        else
                        {
                            CADData cadData = new CADData();
                            XmlSerializer serializer = new XmlSerializer(typeof(CADData));
                            try
                            {
                                cadData = (CADData)serializer.Deserialize(new MemoryStream(CADContent));
                            }
                            catch (Exception ex)
                            {
                                return new
                                {
                                    IsSuccess = false,
                                    statusCode = HttpStatusCode.InternalServerError,
                                    message = "XML may not be valid, "
                                };
                            }
                        if (cadData != null)
                            {
                                 var loadXml = await _cadService.LoadDrawingData(cadUpload.StoreId, cadData.MessageBlock.Messages.MessageData);
                            }
                        }

                   

                    var document = await _cadService.InsertDocument(cadFileName, fileStream.filepath + "\\" + cadFileName, Guid.Parse("bdfb90b9-3dd3-4347-aeb8-3c5d7be6ec6a"));
                    if(document != null)
                    {
                        var storeDocument = await _cadService.InsertStoreDocument(cadUpload.StoreId, document.Id,storeDataId);
                    }


                }


                foreach (string cadpdfFileName in cadPDFFileNames)
                {

                    var document = await _cadService.InsertDocument(cadpdfFileName,fileStream.filepath + "\\" + cadpdfFileName, Guid.Parse("8ae98fb5-16ed-429e-af96-83b6caec15a5"));
                    if (document != null)
                    {
                        var storeDocument = await _cadService.InsertStoreDocument(cadUpload.StoreId, document.Id, storeDataId);
                    }


                }

                    if (storeDataId != null)
                    {
                        await _cadService.UpdateCadUploadHistory(caduploadHistory.Id, cadUpload.StoreId, storeDataId);
                    }

                }

        

                return new
                {

                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK

                };

        }
            catch (Exception ex)
            {

                return new
                {
                    IsSuccess = false,
                    statusCode = HttpStatusCode.InternalServerError,
                    message = ex.Message
                };
}

        }




        /// <summary>
        ///Cad File Upload Version 2
        /// </summary>
        /// <param name="StoreId">Store Identifier</param>
        /// <param name="CadFile">Cad zip File</param>
        /// <param name="Type">Space / Department / Drawing / Grouping / Order</param>
        /// <returns>Return Cad Upload  Status Response</returns>
        [HttpPost]
        [Route("Cad/UploadCadAlt")]
        public async Task<object> UploadCadAlt([Required] Guid StoreId , [Required] IFormFile CadFile, [Required] string Type)
        {
            if (CadFile.ContentType != "application/x-zip-compressed")
            {

                return HttpStatusCode.UnsupportedMediaType;
            }
            Guid? storeDataId = null;

            try
            {

              var fileStream = await UploadCadFileAsync(CadFile, StoreId.ToString());
                var caduploadHistory = await _cadService.InsertCadUploadHistory(StoreId, CadFile.FileName);
                var stream = CadFile.OpenReadStream();

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


                        //string xmlStr = Encoding.UTF8.GetString(CADContent);
                        //string _byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());

                        //if (xmlStr.StartsWith(_byteOrderMarkUtf8))
                        //{
                        //    xmlStr = xmlStr.Remove(0, _byteOrderMarkUtf8.Length);
                        //}


                        //XDocument xdoc = XDocument.Parse(xmlStr);

                        //var cadXml = xdoc.Element("Message").Element("MetaData").Element("MandatoryProperties").Element("CADXml");
                        //var cadType = cadXml?.Attribute("Type").Value;


                        if (Type.ToLower() == "space")
                        {

                            Message cadData = new Message();
                            XmlSerializer serializer = new XmlSerializer(typeof(Message));
                            try
                            {
                                cadData = (Message)serializer.Deserialize(new MemoryStream(CADContent));
                            }
                            catch (Exception ex)
                            {
                                return new
                                {
                                    IsSuccess = false,
                                    statusCode = HttpStatusCode.InternalServerError,
                                    message = "XML may not be valid format"
                                };
                            }
                            if (cadData != null)
                            {
                                var loadXml = await _cadService.LoadXMLData(cadData, StoreId,"Space", caduploadHistory.Id);
                                storeDataId = loadXml.storeDataId;
                            }

                        }
                        else if(Type.ToLower() == "order")
                        {
                            CADOrder cadData = new CADOrder();
                            XmlSerializer serializer = new XmlSerializer(typeof(CADOrder));
                            try
                            {
                                cadData = (CADOrder)serializer.Deserialize(new MemoryStream(CADContent));
                            }
                            catch (Exception ex)
                            {
                                return new
                                {
                                    IsSuccess = false,
                                    statusCode = HttpStatusCode.InternalServerError,
                                    message = "Order XML may not be valid, "
                                };
                            }
                            if (cadData != null)
                            {
                                var loadXml = await _cadService.LoadOrderListData(StoreId, cadData.MessageBlock);
                            }
                        }

                        else if (Type.ToLower() == "department")
                        {
                            DepartmentMessageDto cadData = new DepartmentMessageDto();
                            XmlSerializer serializer = new XmlSerializer(typeof(DepartmentMessageDto));
                            try
                            {
                                cadData = (DepartmentMessageDto)serializer.Deserialize(new MemoryStream(CADContent));
                            }
                            catch (Exception ex)
                            {
                                return new
                                {
                                    IsSuccess = false,
                                    statusCode = HttpStatusCode.InternalServerError,
                                    message = "Order XML may not be valid, "
                                };
                            }
                            if (cadData != null)
                            {
                                var messageModel = _mapper.Map<Message>(cadData);

                                var loadXml = await _cadService.LoadXMLData(messageModel, StoreId,"Department", caduploadHistory.Id);
                            }
                        }

                        else if (Type.ToLower() == "grouping")
                        {

                            Message cadData = new Message();
                            XmlSerializer serializer = new XmlSerializer(typeof(Message));
                            try
                            {
                                cadData = (Message)serializer.Deserialize(new MemoryStream(CADContent));
                            }
                            catch (Exception ex)
                            {
                                return new
                                {
                                    IsSuccess = false,
                                    statusCode = HttpStatusCode.InternalServerError,
                                    message = "XML may not be valid format"
                                };
                            }
                            if (cadData != null)
                            {
                                var loadXml = await _cadService.LoadXMLData(cadData, StoreId, "Grouping", caduploadHistory.Id);
                                storeDataId = loadXml.storeDataId;
                            }

                        }

                        else
                        {
                            CADData cadData = new CADData();
                            XmlSerializer serializer = new XmlSerializer(typeof(CADData));
                            try
                            {
                                cadData = (CADData)serializer.Deserialize(new MemoryStream(CADContent));
                            }
                            catch (Exception ex)
                            {
                                return new
                                {
                                    IsSuccess = false,
                                    statusCode = HttpStatusCode.InternalServerError,
                                    message = "XML may not be valid, "
                                };
                            }
                            if (cadData != null)
                            {
                                var loadXml = await _cadService.LoadDrawingData(StoreId, cadData.MessageBlock.Messages.MessageData);
                            }
                        }



                        var document = await _cadService.InsertDocument(cadFileName, fileStream.filepath + "\\" + cadFileName, Guid.Parse("bdfb90b9-3dd3-4347-aeb8-3c5d7be6ec6a"));
                        if (document != null)
                        {
                            var storeDocument = await _cadService.InsertStoreDocument(StoreId, document.Id,storeDataId);
                        }

                    }


                    foreach (string cadpdfFileName in cadPDFFileNames)
                    {

                        var document = await _cadService.InsertDocument(cadpdfFileName, fileStream.filepath + "\\" + cadpdfFileName, Guid.Parse("8ae98fb5-16ed-429e-af96-83b6caec15a5"));
                        if (document != null)
                        {
                            var storeDocument = await _cadService.InsertStoreDocument(StoreId, document.Id, storeDataId);
                        }


                    }

                    if(storeDataId != null)
                    {
                        await _cadService.UpdateCadUploadHistory(caduploadHistory.Id, StoreId, storeDataId);
                    }
                       
                }

              
                return new
                {

                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK

                };

            }
            catch (Exception ex)
            {

                return new
                {
                    IsSuccess = false,
                    statusCode = HttpStatusCode.InternalServerError,
                    message = ex.Message
                };
            }

        }


        /// <summary>
        ///Get a cad uploaded hitory by store Identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns>Return Cad Upload History Response</returns>
        [HttpGet]
        [Route("CadHistoryByStore/{id}")]
        public async Task<IActionResult> CadHistoryByStore(Guid id, CancellationToken ct)
        {

            return this.Result(await _cadService.GetCadUploadHistoryByStore(id, ct));

        }

    }
}
