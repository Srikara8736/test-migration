using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Retail.Data.Entities.Common;
using Retail.Data.Entities.Customers;
using Retail.Data.Entities.Stores;
using Retail.Data.Entities.UserAccount;
using Retail.Data.Repository;
using Retail.DTOs;
using Retail.DTOs.Cad;
using Retail.DTOs.Customers;
using Retail.DTOs.Stores;
using Retail.DTOs.XML;
using Retail.Services.Common;
using Retail.Services.Master;
using System.Net;
using System.Xml.Linq;
using Customer = Retail.Data.Entities.Customers.Customer;
using Store = Retail.Data.Entities.Stores.Store;


namespace Retail.Services.Stores;

public class StoreService : IStoreService
{
    #region Fields

    private readonly RepositoryContext _repositoryContext;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;
    private readonly ICodeMasterService _codeMasterService;

    #endregion

    #region Ctor
    public StoreService(RepositoryContext repositoryContext, IMapper mapper, IConfiguration configuration, IWebHostEnvironment environment,ICodeMasterService codeMasterService)
    {
        _repositoryContext = repositoryContext;
        _mapper = mapper;
        _configuration = configuration;
        _environment = environment;
        _codeMasterService = codeMasterService;
    }
    #endregion

    #region Utilities

    /// <summary>
    /// Get Address By Identifiers
    /// </summary>
    /// <param name="addressId">Address Id</param>
    /// <returns> Address Infromation</returns>
    public async Task<Address?> getAddressById(Guid addressId)
    { 
        return await _repositoryContext.Addresses.Where(x => x.Id == addressId).AsNoTracking().FirstOrDefaultAsync(); 
    }


    /// <summary>
    /// Get Customer By Identifiers
    /// </summary>
    /// <param name="customerId">Customer Id</param>
    /// <returns> Customer Infromation</returns>
    public async Task<Customer?> getCustomerById(Guid customerId)
    {
        return await _repositoryContext.Customers.Where(x => x.Id == customerId).AsNoTracking().FirstOrDefaultAsync();
    }


    /// <summary>
    /// Get Status by Status Identifiers
    /// </summary>
    /// <param name="statusId">Store Id</param>
    /// <returns> Status Infromation</returns>
    public async Task<CodeMaster?> getStatusById(Guid statusId)
    {
        return await _repositoryContext.CodeMasters.Where(x => x.Id == statusId).AsNoTracking().FirstOrDefaultAsync();
    }



    /// <summary>
    /// Get Store Image items by Store Identifiers
    /// </summary>
    /// <param name="storeId">Store Id</param>
    /// <returns> Image items</returns>
    public async Task<List<StoreImage>> GetStoreImagesByStoreId(Guid storeId)
    {
         return await _repositoryContext.StoreImages.Where(x => x.StoreId == storeId).AsNoTracking().ToListAsync();
    }


    /// <summary>
    /// Get Image item by Identifiers
    /// </summary>
    /// <param name="id">id</param>
    /// <param name="ct">Cancellation Charages</param>
    /// <returns>Image Information</returns>
    public async Task<Data.Entities.FileSystem.Image> GetImageById(Guid id, CancellationToken ct = default)
    {
        return await _repositoryContext.Images.FirstOrDefaultAsync(x => x.Id == id, cancellationToken: ct);
    }



    /// <summary>
    /// Insert Image 
    /// </summary>
    /// <param name="image">image</param>
    /// <param name="ct">Cancellation Charages</param>
    /// <returns>Image Information</returns>
    public async Task<Data.Entities.FileSystem.Image> InsertImage(Data.Entities.FileSystem.Image image)
    {
        if (image == null)
            return null;


        await _repositoryContext.Images.AddAsync(image);
        await _repositoryContext.SaveChangesAsync();
        return image;

    }

    /// <summary>
    /// Delete Image
    /// </summary>
    /// <param name="image">image</param>
    /// <param name="ct">Cancellation Charages</param>
    /// <returns>True / False Image Delete Status</returns>
    public async Task<bool> DeleteImage(Guid ImageId)
    {
        var ImageItem = await _repositoryContext.Images.FirstOrDefaultAsync(x => x.Id == ImageId);

        if (ImageItem == null)
            return false;

        _repositoryContext.Images.Remove(ImageItem);
        await _repositoryContext.SaveChangesAsync();
        return true;

    }

    /// <summary>
    /// Insert store Image
    /// </summary>
    /// <param name="image">image</param>
    /// <param name="ct">Cancellation Charages</param>
    /// <returns>Store Image Information</returns>
    public async Task<StoreImage> InsertStoreImage(StoreImage image)
    {
        if (image == null)
            return null;

        await _repositoryContext.StoreImages.AddAsync(image);
        await _repositoryContext.SaveChangesAsync();
        return image;


    }



    /// <summary>
    /// Update store activity
    /// </summary>
    /// <param name="storeId">storeId</param>
    /// <param name="ct">Cancellation Charages</param>
    /// <returns>True / False to update store activity</returns>
    public async Task<bool> UpdateStoreActivity(Guid storeId)
    {
        if (storeId == null)
            return false;

       var store = await _repositoryContext.Stores.FirstOrDefaultAsync(x => x.Id == storeId);

        store.LastActivityDate = DateTime.UtcNow;
        await _repositoryContext.SaveChangesAsync();

        return true;


    }



    /// <summary>
    /// Update store Data activity
    /// </summary>
    /// <param name="storeDataId">storeId</param>
    /// <param name="statusId">storeId</param>
    /// <returns>True / False to update store activity</returns>
    public async Task<bool> UpdateStoreDataStatus(Guid storeDataId,Guid statusId)
    {
        if (storeDataId == null)
            return false;

        var store = await _repositoryContext.StoreDatas.FirstOrDefaultAsync(x => x.Id == storeDataId);

        if(store == null) 
            return false;

        store.StatusId = statusId;

        await _repositoryContext.SaveChangesAsync();

        return true;


    }

    /// <summary>
    /// Update store Data activity
    /// </summary>
    /// <param name="storeDataId">storeId</param>
    /// <param name="storeData">storeData</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>True / False to update store activity</returns>
    public async Task<ResultDto<bool>> UpdateStoreData(Guid id, StoreDataStatusDto storeData, CancellationToken ct = default)
    {

        if (storeData == null)
        {
            var errorResponse = new ResultDto<bool>
            {
                ErrorMessage = StringResources.BadRequest,
                IsSuccess = false
            };
            return errorResponse;
        }

       var store = await _repositoryContext.StoreDatas.FirstOrDefaultAsync(x => x.Id == id);

        if (store == null)
        {
            var errorResponse = new ResultDto<bool>
            {
                ErrorMessage = StringResources.NoResultsFound,
                IsSuccess = false
            };
            return errorResponse;
        }


        if (storeData.StatusId == Guid.Parse(_configuration["StatusValues:StoreDataDefault"]))
        {
            var existingData = await _repositoryContext.StoreDatas.Where(x => x.StoreId == store.StoreId && x.CadFileTypeId == store.CadFileTypeId && x.StatusId == Guid.Parse(_configuration["StatusValues:StoreDataDefault"])).ToListAsync();

           foreach(var item in existingData)
            {
                await UpdateStoreDataStatus(item.Id, Guid.Parse("6E9EC422-3537-11EE-BE56-0242AC120002"));
            }

        }
        


        store.StatusId = storeData.StatusId;
        store.VersionNumber = storeData.VersionNumber;
        store.VersionName = storeData.VersionName;
        store.Comments = storeData.Comments;
        await _repositoryContext.SaveChangesAsync();
            

        var successResponse = new ResultDto<bool>
        {
           Data= true,
            IsSuccess = true
        };
        return successResponse;




    }


    /// <summary>
    /// Insert customer Image
    /// </summary>
    /// <param name="image">Image</param>
    /// <param name="ct">Cancellation Charages</param>
    /// <returns>Insert customer Image</returns>
    public async Task<CustomerImage> InsertCustomerImage(CustomerImage image)
    {
        if (image == null)
            return null;

        await _repositoryContext.CustomerImages.AddAsync(image);
        await _repositoryContext.SaveChangesAsync();
        return image;


    }


    /// <summary>
    /// Insert customer Address
    /// </summary>
    /// <param name="addressDto">Address</param>
    /// <param name="ct">Cancellation Charages</param>
    /// <returns>Insert customer Address</returns>
    public async Task<Address> InsertCustomerAddress(AddressDto addressDto, CancellationToken ct)
    {
        try
        {
            var address = _mapper.Map<Address>(addressDto);
            await _repositoryContext.Addresses.AddAsync(address, ct);
            await _repositoryContext.SaveChangesAsync(ct);
            return address;
        }
        catch (Exception ex)
        {
            return null;
        }

    }

    /// <summary>
    /// Update Customer Address
    /// </summary>
    /// <param name="addressId">addressId</param>
    /// <param name="addressDto">Address</param>
    /// <param name="ct">Cancellation Charages</param>
    /// <returns>Update Customer Image</returns>
    public async Task<Address> UpdateCustomerAddress(Guid addressId, AddressDto addressDto, CancellationToken ct)
    {
        try
        {
            var address = await _repositoryContext.Addresses.Where(x => x.Id == addressId).FirstOrDefaultAsync();
            if (address == null)
                return null;

            address.City = addressDto.City;
            address.Street = addressDto.Street;
            address.ZipCode = addressDto.ZipCode;
            address.Country = addressDto.Country;
            address.Latitude = addressDto.Latitude;
            address.Longitude = addressDto.Longitude;
            address.FullAddress = addressDto.FullAddress;


            await _repositoryContext.SaveChangesAsync(ct);
            return address;
        }
        catch (Exception ex)
        {
            return null;
        }

    }

    /// <summary>
    /// Delete Customer Address
    /// </summary>
    /// <param name="addressId">addressId</param>
    /// <param name="ct">Cancellation Charages</param>
    /// <returns>Delete Customer Address</returns>
    public async Task<bool> DeleteAddress(Guid addressId, CancellationToken ct)
    {
        try
        {
            var address = await _repositoryContext.Addresses.Where(x => x.Id == addressId).FirstOrDefaultAsync();
            if (address == null)
                return false;

            _repositoryContext.Addresses.Remove(address);
            await _repositoryContext.SaveChangesAsync(ct);
            return true;
        }
        catch (Exception ex)
        {
            return true;
        }

    }



    #endregion


    #region Methods


    /// <summary>
    /// Get all Stores by CustomerId
    /// </summary>
    /// <param name="customerId">customerId</param>
    /// <param name="ct">cancellation token</param>
    /// <returns>Store List belongs customer</returns>
    public async Task<List<StoreResponseDto>> GetStoresByCustomerId(Guid customerId, CancellationToken ct = default)
    {

       var stores = await _repositoryContext.Stores.Where(x => x.CustomerId == customerId).OrderByDescending(t => t.LastActivityDate).ToListAsync();

    
        if (stores == null)
        {
            return null;
        }

        var codeMaster = await _repositoryContext.CodeMasters.FirstOrDefaultAsync(x => x.Type == "CadType" && x.Value == "Space");

        if (codeMaster == null)        {
            
            return null;
        }

        var storeResponse = _mapper.Map<List<StoreResponseDto>>(stores);
        var path = _configuration["AssetLocations:StoreImages"];


       

        foreach (var store in storeResponse)
        {
            store.StoreSearchkey = store.Name + "-" + store.StoreNumber;

            var storeData = await _repositoryContext.StoreDatas.Where(x => x.StoreId == store.Id && x.CadFileTypeId == codeMaster.Id).ToListAsync();
            if(storeData != null)
            {
                foreach(var item in storeData)
                {
                    var storeDataVersion = new StoreDataVersion()
                    {
                        Id = item.Id,
                        Version = item.VersionName ,
                        VersionNumber = item.VersionNumber

                    };
                    var storeUploadType = await _codeMasterService.GetStatusById(item.CadFileTypeId, null, default);
                    if (storeUploadType.Data != null)
                    {
                        storeDataVersion.CadTypeId = storeUploadType.Data.Id;
                        storeDataVersion.CadTypeName = storeUploadType.Data.Value;
                    }
                    store.storeDataVersions.Add(storeDataVersion);
                }
            }


            if (store.AddressId != null)
            {

                var result = _repositoryContext.Addresses.Where(x => x.Id == store.AddressId).FirstOrDefault();
                if (result != null)
                {
                    var customerAddress = _mapper.Map<AddressDto>(result);
                    store.Address = customerAddress;
                }
;
            }

            if (store.CustomerId != null)
            {

                var result = _repositoryContext.Customers.Where(x => x.Id == store.CustomerId).FirstOrDefault();
                if (result != null)
                {
                    var customerAddress = _mapper.Map<CustomerDto>(result);
                    store.customer = customerAddress;
                }
;
            }
            if (store.StatusId != null)
            {

                var result = _repositoryContext.CodeMasters.Where(x => x.Id == store.StatusId).FirstOrDefault();
                if (result != null)
                {

                    store.StoreStatus = result.Value;
                }
;
            }

            var storeImages = await GetStoreImagesByStoreId(store.Id);
            foreach(var img in storeImages)
            {

                var image = await GetImageById((Guid)img.ImageId);
                if (image != null)
                {

                   
                    var storeImageItem = new ImageDto()
                    {
                        Id = img.Id,
                        ImageId = image.Id,
                        ImageUrl = path + image.FileName

                    };

                    string fileName = Path.GetFileNameWithoutExtension(image.FileName);
                    string filePath = Path.GetDirectoryName(image.FileName);



                    DirectoryInfo dir = new DirectoryInfo(_environment.WebRootPath + path + filePath);
                    FileInfo[] filesInDir = dir.GetFiles("*" + fileName + "*.*");


                    foreach (var item in filesInDir)
                    {
                        var itemName = Path.GetFileNameWithoutExtension(item.Name);
                        if (itemName != fileName)
                            storeImageItem.ThumnailUrls.Add(path+ store.Id+"/" + item.Name);
                    }


                    store.StoreImages.Add(storeImageItem);
                }
                   

                
            }

            //cad upload History

           
            var storeDataHistory = await StoreDataFileHistoryByStoreId(store.Id);
            store.StoreDataHistories = storeDataHistory;

            var storeLiveData = storeData?.Where(x => x.StoreId == store.Id && x.CadFileTypeId == codeMaster.Id && x.StatusId == Guid.Parse(_configuration["StatusValues:StoreDataDefault"])).OrderByDescending(x => x.VersionNumber).FirstOrDefault();          

            if (storeLiveData != null)
            {

                store.PdfLink = await PdfFileUrlByStoreId(store.Id, storeLiveData.Id);

                var uploadHistory = await _repositoryContext.CadUploadHistories.Where(x => x.StoreId == store.Id && x.StoreDataId == storeLiveData.Id && x.Status).OrderByDescending(y => y.UploadOn).FirstOrDefaultAsync();

                if (uploadHistory != null)
                {
                    var cadResponse = _mapper.Map<DTOs.Cad.CadUploadHistoryResponseDto>(uploadHistory);
                    store.cadUploadHistory = cadResponse;
                }

            }

        }


       
        return storeResponse;

    }


    /// <summary>
    /// Get File History of Versions
    /// </summary>
    /// <param name="storeId">storeId</param>
    /// <returns>Store PDF Url</returns>
    public async Task<List<StoreDataHistoryDto>> StoreDataFileHistoryByStoreId(Guid storeId)
    {

        var storeDataHistories = new List<StoreDataHistoryDto>();

        var storeDataItems = await _repositoryContext.StoreDatas.Where(x => x.StoreId == storeId).OrderByDescending(x => x.CreatedOn).ToListAsync();

       
        foreach (var item in storeDataItems)
        {

            var storeData = new StoreDataHistoryDto
            {
                StoreId = item.StoreId,
                StoreDataId = item.Id,
                Name = item.VersionName,
                VersionNumber = item.VersionNumber,
                Comments = item.Comments
            };
            var storeStatus = await _codeMasterService.GetStatusById(item.StatusId, null, default);
            if(storeStatus.Data != null)
            {
                storeData.Status = storeStatus.Data.Value;
                storeData.StatusId = storeStatus.Data.Id;
            }

            var storeUploadType = await _codeMasterService.GetStatusById(item.CadFileTypeId, null, default);
            if (storeUploadType.Data != null)
            {
                storeData.UploadType = storeUploadType.Data.Value;
                storeData.UploadTypeId = storeUploadType.Data.Id;
            }

            var storeDocuments = await _repositoryContext.StoreDocuments.Where(x => x.StoreDataId == item.Id).ToListAsync();            
            foreach(var sDoc in storeDocuments)
            {
                var docResult = await _repositoryContext.Documents.FirstOrDefaultAsync(x => x.Id == sDoc.DocumentId);
                               
                if(docResult != null)
                {
                    var documentHistory = new DocumentHistoryDto
                    {
                        StoreDocumentId = sDoc.Id,
                        DocumentId = docResult.Id,
                        Path = docResult.Path,
                        FileName = docResult.Name
                    };

                    var storeFileType = await _codeMasterService.GetStatusById(docResult.StatusId, null, default);
                    if (storeFileType.Data != null)
                    {
                        documentHistory.FileType = storeFileType.Data.Value;
                        documentHistory.FileTypeId = storeFileType.Data.Id;
                    }
                    storeData.documentHistories.Add(documentHistory);

                }
            }



            var uploadHistory = await _repositoryContext.CadUploadHistories.Where(x => x.StoreId == item.StoreId && x.StoreDataId == item.Id && x.Status).OrderByDescending(y => y.UploadOn).FirstOrDefaultAsync();

            if (uploadHistory != null)
            {
                var cadResponse = _mapper.Map<DTOs.Cad.CadUploadHistoryResponseDto>(uploadHistory);
                storeData.cadUploadHistory = cadResponse;
            }

            storeDataHistories.Add(storeData);
        }

        
        return storeDataHistories;
    }


    /// <summary>
    /// Get PDF Url By store Id
    /// </summary>
    /// <param name="storeId">storeId</param>
    /// <returns>Store PDF Url</returns>
    public async Task<string> PdfFileUrlByStoreId(Guid storeId,Guid storeDataId)
    {
        var path = string.Empty;


        var result = from at in _repositoryContext.StoreDocuments
                     join cat in _repositoryContext.Documents on at.DocumentId equals cat.Id
                     where cat.StatusId == Guid.Parse(_configuration["StatusValues:PDFDefault"]) && at.StoreId == storeId && at.StoreDataId == storeDataId
                     orderby at.UploadedOn descending
                     select cat.Path;

        path = await result.FirstOrDefaultAsync();

        return path;
    }

    /// <summary>
    /// Get all Stores
    /// </summary>
    /// <param name="customerId">customerId</param>
    /// <param name="keyword">keyword</param>
    /// <param name="pageIndex">page Index</param>
    /// <param name="pageSize">page size</param>
    /// <param name="ct">cancellation token</param>
    /// <returns>Store List with Pagination</returns>
    public async Task<PaginationResultDto<PagedList<StoreResponseDto>>> GetAllStores(Guid? customerId = null, string? keyword = null, int pageIndex = 0, int pageSize = 0, CancellationToken ct = default)
    {

        var query = from p in _repositoryContext.Stores
                    select p;



        if (customerId != null)
        {
            query = query.Where(x => x.CustomerId == customerId);
        }


        if (keyword != null)
        {
            query = query.Where(x => x.Name.Contains(keyword));
        }

        var stores = await PagedList<Store>.ToPagedList(query.OrderByDescending(on => on.LastActivityDate), pageIndex, pageSize);

        if (stores == null)
        {
            var errorResponse = new PaginationResultDto<PagedList<StoreResponseDto>>
            {
                IsSuccess = false,
                ErrorMessage = StringResources.NoResultsFound,
                StatusCode = HttpStatusCode.NoContent
            };

            return errorResponse;
        }

        var codeMaster = await _repositoryContext.CodeMasters.FirstOrDefaultAsync(x => x.Type == "CadType" && x.Value == "Space");

        if (codeMaster == null)
        {
            var storeResult = new PaginationResultDto<PagedList<StoreResponseDto>>
            {
                ErrorMessage = StringResources.NoResultsFound,
                IsSuccess = false
            };
            return storeResult;
        }

        var storeResponse = _mapper.Map<PagedList<StoreResponseDto>>(stores);
        

        var path = _configuration["AssetLocations:StoreImages"];


        foreach (var store in storeResponse)
        {
            store.StoreSearchkey = store.Name + "-" + store.StoreNumber;

            var storeData = await _repositoryContext.StoreDatas.Where(x => x.StoreId == store.Id && x.CadFileTypeId == codeMaster.Id).ToListAsync();

              if (storeData != null)
            {
                foreach (var item in storeData)
                {
                    var storeDataVersion = new StoreDataVersion()
                    {
                        Id = item.Id,
                        Version = item.VersionName ,
                        VersionNumber = item.VersionNumber.ToString()

                    };
                    var storeUploadType = await _codeMasterService.GetStatusById(item.CadFileTypeId, null, default);
                    if (storeUploadType.Data != null)
                    {
                        storeDataVersion.CadTypeId = storeUploadType.Data.Id;
                        storeDataVersion.CadTypeName = storeUploadType.Data.Value;
                    }
                    store.storeDataVersions.Add(storeDataVersion);
                }
            }




            if (store.AddressId != null)
            {
               
                var result = _repositoryContext.Addresses.Where(x => x.Id == store.AddressId).FirstOrDefault();
                if (result != null)
                {
                    var customerAddress = _mapper.Map<AddressDto>(result);
                    store.Address = customerAddress;
                }
;
            }

            if (store.CustomerId != null)
            {

                var result = _repositoryContext.Customers.Where(x => x.Id == store.CustomerId).FirstOrDefault();
                if (result != null)
                {
                    var customerAddress = _mapper.Map<CustomerDto>(result);
                    store.customer = customerAddress;
                }
;
            }
            if (store.StatusId != null)
            {

                var result = _repositoryContext.CodeMasters.Where(x => x.Id == store.StatusId).FirstOrDefault();
                if (result != null)
                {
                   
                    store.StoreStatus = result.Value;
                }
;
            }


            var storeImages = await GetStoreImagesByStoreId(store.Id);
            foreach (var img in storeImages)
            {
                var image = await GetImageById((Guid)img.ImageId);
                if (image != null)
                {
                    var storeImageItem = new ImageDto()
                    {
                        Id = img.Id,
                        ImageId = image.Id,
                        ImageUrl = path + image.FileName

                    };

                    string fileName = Path.GetFileNameWithoutExtension(image.FileName);
                    string filePath = Path.GetDirectoryName(image.FileName);



                    DirectoryInfo dir = new DirectoryInfo(_environment.WebRootPath + path + filePath);
                    FileInfo[] filesInDir = dir.GetFiles("*" + fileName + "*.*");


                    foreach (var item in filesInDir)
                    {
                        var itemName = Path.GetFileNameWithoutExtension(item.Name);
                        if (itemName != fileName)
                            storeImageItem.ThumnailUrls.Add(path + store.Id + "/" + item.Name);
                    }


                    store.StoreImages.Add(storeImageItem);
                }
            }

            
            var storeDataHistory = await StoreDataFileHistoryByStoreId(store.Id);
            store.StoreDataHistories = storeDataHistory;


            var storeLiveData = storeData?.Where(x => x.StoreId == store.Id && x.CadFileTypeId == codeMaster.Id && x.StatusId == Guid.Parse(_configuration["StatusValues:StoreDataDefault"])).OrderByDescending(x => x.VersionNumber).FirstOrDefault();

            if(storeLiveData != null)
            {

                store.PdfLink = await PdfFileUrlByStoreId(store.Id, storeLiveData.Id);

                var uploadHistory = await _repositoryContext.CadUploadHistories.Where(x => x.StoreId == store.Id && x.StoreDataId == storeLiveData.Id && x.Status).OrderByDescending(y => y.UploadOn).FirstOrDefaultAsync();

                if (uploadHistory != null)
                {
                    var cadResponse = _mapper.Map<DTOs.Cad.CadUploadHistoryResponseDto>(uploadHistory);
                    store.cadUploadHistory = cadResponse;
                }

            }
            


        }





        var response = new PaginationResultDto<PagedList<StoreResponseDto>>
        {
            IsSuccess = true,
            Data = storeResponse,
            TotalCount = stores.TotalCount,
            TotalPages = stores.TotalPages
        };
        return response;

    }


    /// <summary>
    /// Get all Grid Data
    /// </summary>
    /// <param name="StoreId">Store Identifier</param>
    /// <param name="ct">cancellation token</param>
    /// <returns>Store Grid Data</returns>
    public async Task<ResultDto<List<ChartGridDto>>> GetGridData(Guid StoreId, Guid? StoreDataId, bool IsGroup = false, CancellationToken ct = default)
    {

        var store = await _repositoryContext.Stores.FirstOrDefaultAsync(x => x.Id == StoreId, ct);
        if (store == null)
        {
            var storeResult = new ResultDto<List < ChartGridDto>> ()
            {
                ErrorMessage = StringResources.NoResultsFound,
                IsSuccess = false
            };
            return storeResult;
        }

        var fileterValue = "Space";

        if (IsGroup)
            fileterValue = "Grouping";

        var codeMaster = await _repositoryContext.CodeMasters.FirstOrDefaultAsync(x => x.Type == "CadType" && x.Value == fileterValue);

        if (codeMaster == null)
        {
            var storeResult = new ResultDto<List<ChartGridDto>>()
            {
                ErrorMessage = StringResources.NoResultsFound,
                IsSuccess = false
            };
            return storeResult;
        }

        //var storeData = await _repositoryContext.StoreDatas.Where(x => x.StoreId == StoreId && x.CadFileTypeId == codeMaster.Id).OrderByDescending(x => x.VersionNumber).FirstOrDefaultAsync();

        var storeData = await GetStoreData(StoreId, codeMaster.Id, StoreDataId);

        if (storeData == null )
        {
            var storeResult = new ResultDto<List<ChartGridDto>>()
            {
                ErrorMessage = StringResources.NoResultsFound,
                IsSuccess = false
            };
            return storeResult;
        }


        try
        {
            var query = (from at in _repositoryContext.AreaTypes
                         join cat in _repositoryContext.Categories on at.Id equals cat.AreaTypeId
                         join sp in _repositoryContext.Spaces on cat.Id equals sp.CategoryId
                         join stsp in _repositoryContext.StoreSpaces on sp.Id equals stsp.SpaceId
                         where stsp.StoreId == StoreId && stsp.StoreDataId == storeData.Id && stsp.CadFileTypeId == codeMaster.Id
                         select new
                         {
                             CategoryId = cat.Id,
                             CategoryName = cat.Name,
                             AreaTypeId = at.Id,
                             AreaTypeName = at.Name,
                             SpaceName = sp.Name,
                             SpaceUnit = stsp.Unit,
                             SpaceAtricles = stsp.Articles,
                             SpaceArea = stsp.Area,
                             SpacePieces = stsp.Pieces,
                             SpaceId = sp.Id

                         }).ToList();



            var areaTypeGrid = new List<ChartGridDto>();

            var areaTypeGroupResult = query.GroupBy(x => x.AreaTypeId).ToList();

            foreach (var item in areaTypeGroupResult)
            {
                var areaType = new ChartGridDto();

                var categoryGrid = new List<CategoryGridDto>();

                var categoryGroup = item.GroupBy(x => x.CategoryId).ToList();
                foreach (var categoryResult in categoryGroup)
                {
                    var category = new CategoryGridDto();

                    var spaceGrid = new List<SpaceGridDto>();

                    foreach (var result in categoryResult)
                    {
                        areaType.AreaType = result.AreaTypeName.Trim();
                        areaType.AreaTypeId = result.AreaTypeId;

                        category.CategoryId = result.CategoryId;
                        category.Category = result.CategoryName.Trim();

                        var space = new SpaceGridDto
                        {   SpaceId = result.SpaceId,
                            Space = result.SpaceName.Trim(),
                            Unit = result.SpaceUnit,
                            Area = (decimal)result.SpaceArea,
                            Pieces = (decimal)result.SpacePieces,
                            Atricles = (decimal)result.SpaceAtricles
                        };
                        spaceGrid.Add(space);

                    }
                    category.Spaces = spaceGrid;
                    category.TotalArea = spaceGrid.Sum(x => x.Area);
                    categoryGrid.Add(category);

                };


                areaType.Categories = categoryGrid;
                areaType.TotalArea = categoryGrid.Sum(x => x.TotalArea);
                areaType.TotalAreaPercentage = 100;
                areaTypeGrid.Add(areaType);
            }

            foreach(var areatype in areaTypeGrid)
            {
                foreach(var category in areatype.Categories)
                {
                    foreach (var space in category.Spaces)
                    {
                        space.TotalPercentage = Math.Round((space.Area / category.TotalArea) * 100,0);
                    }

                    category.TotalAreaPercentage = Math.Round((category.TotalArea / areatype.TotalArea) * 100, 0); 
                }

            }




            var successResponse = new ResultDto<List<ChartGridDto>>
            {
                IsSuccess = true,
                Data = areaTypeGrid
            };

            return successResponse;

        }
        catch(Exception ex)
        {
            var storeResult = new ResultDto<List<ChartGridDto>>()
            {
                ErrorMessage = StringResources.InvalidArgument,
                StatusCode = HttpStatusCode.InternalServerError
            };
            return storeResult;
        }
        

    }



    /// <summary>
    /// Get all Grid Data of Department List
    /// </summary>
    /// <param name="StoreId">Store Identifier</param>
    /// <param name="ct">cancellation token</param>
    /// <returns>Store Department List Grid Data</returns>
    public async Task<ResultDto<DaparmentListDto>> GetDepartmentGridData(Guid StoreId, Guid? StoreDataId, CancellationToken ct = default)
    {

        var store = await _repositoryContext.Stores.FirstOrDefaultAsync(x => x.Id == StoreId, ct);
        if (store == null)
        {
            var storeResult = new ResultDto<DaparmentListDto>()
            {
                ErrorMessage = StringResources.NoResultsFound,
                IsSuccess = false
            };
            return storeResult;
        }

        var codeMaster = await _repositoryContext.CodeMasters.FirstOrDefaultAsync(x => x.Type == "CadType" && x.Value == "Department");


        if (codeMaster == null)
        {
            var storeResult = new ResultDto<DaparmentListDto>()
            {
                ErrorMessage = StringResources.NoResultsFound,
                IsSuccess = false
            };
            return storeResult;
        }

       

        var storeData = await GetStoreData(StoreId, codeMaster.Id, StoreDataId);
             

        if (storeData == null)
        {
            var storeResult = new ResultDto<DaparmentListDto>()
            {
                ErrorMessage = StringResources.NoResultsFound,
                IsSuccess = false
            };
            return storeResult;
        }


        try
        {
            var query = (from at in _repositoryContext.AreaTypes
                         join cat in _repositoryContext.Categories on at.Id equals cat.AreaTypeId
                         join sp in _repositoryContext.Spaces on cat.Id equals sp.CategoryId
                         join stsp in _repositoryContext.StoreSpaces on sp.Id equals stsp.SpaceId                        
                         where stsp.StoreId == StoreId && stsp.StoreDataId == storeData.Id && stsp.CadFileTypeId == codeMaster.Id
                         select new
                         {
                             CategoryId = cat.Id,
                             CategoryName = cat.Name,
                             AreaTypeId = at.Id,
                             AreaTypeName = at.Name,
                             SpaceName = sp.Name,
                             SpaceUnit = stsp.Unit,
                             SpaceAtricles = stsp.Articles,
                             SpaceArea = stsp.Area,
                             SpacePieces = stsp.Pieces,
                             SpaceId =sp.Id,
                             MissingCat = cat.CategoryId,

                         }).ToList();



           var categories = _repositoryContext.CadStoreCategories.Where(x => x.StoreDataId == storeData.Id && x.StoreId == StoreId).ToList();



            var areaTypeGrid = new List<ChartGridDto>();


            var areaTypeGroupResult = query.GroupBy(x => x.AreaTypeId).ToList();

            foreach (var item in areaTypeGroupResult)
            {
                var areaType = new ChartGridDto();

                var categoryGrid = new List<CategoryGridDto>();

                var categoryGroup = item.GroupBy(x => x.CategoryId).ToList();
                foreach (var categoryResult in categoryGroup)
                {
                    var category = new CategoryGridDto();

                    var spaceGrid = new List<SpaceGridDto>();

                    foreach (var result in categoryResult)
                    {
                        areaType.AreaType = result.AreaTypeName.Trim();
                        areaType.AreaTypeId = result.AreaTypeId;

                        category.CategoryId = result.CategoryId;
                        category.Category = result.CategoryName.Trim();

                        var space = new SpaceGridDto
                        {
                            SpaceId = result.SpaceId,
                            Space = result.SpaceName.Trim(),
                            Unit = result.SpaceUnit,
                            Area = (decimal)result.SpaceArea,
                            Pieces = (decimal)result.SpacePieces,
                            Atricles = (decimal)result.SpaceAtricles
                        };
                        spaceGrid.Add(space);

                    }
                    category.Spaces = spaceGrid;
                    category.TotalArea = spaceGrid.Sum(x => x.Area);
                    categoryGrid.Add(category);

                };


                areaType.Categories = categoryGrid;
                areaType.TotalArea = categoryGrid.Sum(x => x.TotalArea);
                areaType.TotalAreaPercentage = 100;
                areaTypeGrid.Add(areaType);
            }

            foreach (var areatype in areaTypeGrid)
            {
                foreach (var category in areatype.Categories)
                {
                   foreach(var space in category.Spaces)
                    {
                        space.TotalPercentage = Math.Round((space.Area / category.TotalArea) * 100,0);
                    }

                    var itemToRemove = categories.FirstOrDefault(r => r.CategoryId == category.CategoryId);
                    if (itemToRemove != null)
                        categories.Remove(itemToRemove);

                    category.TotalAreaPercentage = Math.Round((category.TotalArea / areatype.TotalArea) * 100, 0);
                }

            }

            var departmentList = new DaparmentListDto
            {
                chartGrids = areaTypeGrid,
                TotalArea = areaTypeGrid.Sum(x => x.TotalArea)
            };

            var successResponse = new ResultDto<DaparmentListDto>
            {
                IsSuccess = true,
                Data = departmentList
            };

            return successResponse;

        }
        catch (Exception ex)
        {
            var storeResult = new ResultDto<DaparmentListDto>()
            {
                ErrorMessage = StringResources.InvalidArgument,
                StatusCode = HttpStatusCode.InternalServerError
            };
            return storeResult;
        }


    }



    /// <summary>
    /// Get Chart items of Draft Category of Charts
    /// </summary>
    /// <param name="charts">Generated Chart List</param>
    /// <param name="storeId">Store Identifier</param>
    /// <param name="storeDataId">Store DataIdentifier</param>
    /// <returns>Chart items of Draft Category</returns>
    public void DraftCategoryChart(List<ChartGraphDto> charts, Guid storeId, Guid storeDataId)
    {
        var query = (from cat in _repositoryContext.Categories
                     join stsp in _repositoryContext.StoreSpaces on cat.Id equals stsp.CategoryId
                     where cat.AreaTypeId == null && stsp.StoreId == storeId && stsp.StoreDataId == storeDataId
                     orderby cat.CadServiceNumber
                     select new
                     {
                         categoryName = cat.Name.Trim(),
                         article = stsp.Articles,
                         unit = stsp.Unit,

                     });

        var DraftchartData = new ChartGraphDto();
        DraftchartData.ChartTitle = "Draft Area";
        DraftchartData.ChartCategory = "Article";
        DraftchartData.ChartType = "Pie";

        var totalArea = query.Sum(x => x.article);
        foreach (var item in query)
        {
           

            var chartItem = new ChartItemDto
            {
                Key = item.categoryName,
                Value = (decimal)item.article,
                TotalPercentage =  Math.Round((decimal)(item.article / totalArea * 100), 0),
                Unit = item.unit,

            };
            DraftchartData.chartItems.Add(chartItem);
        }

        if(DraftchartData.chartItems.Count > 0)
            charts.Add(DraftchartData);

    }




    /// <summary>
    /// Get Store Chart items of Draft Category of Charts
    /// </summary>
    /// <param name="charts">Generated Chart List</param>
    /// <param name="storeId">Store Identifier</param>
    /// <param name="storeDataId">Store DataIdentifier</param>
    /// <returns>Chart items of Store Draft Category</returns>
    public void DraftStoreCategoryChart(List<StoreChartGraphDto> charts, Guid storeId, Guid storeDataId)
    {
        var query = (from cat in _repositoryContext.Categories
                     join stsp in _repositoryContext.StoreSpaces on cat.Id equals stsp.CategoryId
                     where cat.AreaTypeId == null && stsp.StoreId == storeId && stsp.StoreDataId == storeDataId
                     orderby cat.CadServiceNumber
                     select new
                     {
                         categoryName = cat.Name.Trim(),
                         article = stsp.Articles,
                         unit = stsp.Unit,

                     });

        var DraftchartData = new StoreChartGraphDto();
        DraftchartData.ChartTitle = "Draft Area";
        DraftchartData.ChartCategory = "Article";
        DraftchartData.ChartType = "Pie";

        var totalArea = query.Sum(x => x.article);
        foreach (var item in query)
        {


            var chartItem = new StoreChartItemDto
            {
                Key = item.categoryName,
                Value = (decimal)item.article,
                TotalPercentage = Math.Round((decimal)(item.article / totalArea * 100), 0),
                Unit = item.unit,

            };
            DraftchartData.chartItems.Add(chartItem);
        }

        if (DraftchartData.chartItems.Count > 0)
            charts.Add(DraftchartData);

    }

    /// <summary>
    /// Get all Chart Data
    /// </summary>
    /// <param name="StoreId">Store Identifier</param>
    /// <param name="ct">cancellation token</param>
    /// <returns>Store Chart Data</returns>
    public async Task<ResultDto<List<ChartGraphDto>>> GetChartData(Guid StoreId, Guid? StoreDataId, string? type = null, CancellationToken ct = default)
    {

        var store = await _repositoryContext.Stores.FirstOrDefaultAsync(x => x.Id == StoreId, ct);
        if (store == null)
        {
            var storeResult = new ResultDto<List<ChartGraphDto>>()
            {
                ErrorMessage = StringResources.NoResultsFound,IsSuccess = false
            };
            return storeResult;
        }

        var fileterValue = "Space";

        if (type != null)
            fileterValue = type;

        var codeMaster = await _repositoryContext.CodeMasters.FirstOrDefaultAsync(x => x.Type == "CadType" && x.Value.ToLower().Trim() == fileterValue.ToLower().Trim());

      
        if (codeMaster == null)
        {
            var storeResult = new ResultDto<List<ChartGraphDto>>()
            {
                ErrorMessage = StringResources.NoResultsFound,
                IsSuccess = false
            };
            return storeResult;
        }

      

        var storeData = await GetStoreData(StoreId, codeMaster.Id, StoreDataId);

        if (storeData == null )
        {
            var storeResult = new ResultDto<List<ChartGraphDto>>()
            {
                ErrorMessage = StringResources.NoResultsFound,
                IsSuccess = false
            };
            return storeResult;
        }

        try
        {
            var query = (from at in _repositoryContext.AreaTypes
                         join cat in _repositoryContext.Categories on at.Id equals cat.AreaTypeId
                         join sp in _repositoryContext.Spaces on cat.Id equals sp.CategoryId
                         join stsp in _repositoryContext.StoreSpaces on sp.Id equals stsp.SpaceId
                         where stsp.StoreId == StoreId && stsp.StoreDataId == storeData.Id && stsp.CadFileTypeId == codeMaster.Id
                         orderby cat.CadServiceNumber 
                         select new
                         {
                             CategoryId = cat.Id,
                             CategoryName = cat.Name,
                             AreaTypeId = at.Id,
                             AreaTypeName = at.Name,
                             SpaceId = sp.Id,
                             SpaceName = sp.Name,
                             SpaceUnit = stsp.Unit,
                             SpaceAtricles = stsp.Articles,
                             SpaceArea = stsp.Area,
                             SpacePieces = stsp.Pieces,
                             SpaceCadNumber = sp.CadServiceNumber

                         }).ToList();



            var areaTypeGrid = new List<ChartGridDto>();

            var areaTypeGroupResult = query.GroupBy(x => x.AreaTypeId).ToList();

            foreach (var item in areaTypeGroupResult)
            {
                var areaType = new ChartGridDto();

                var categoryGrid = new List<CategoryGridDto>();

                var categoryGroup = item.GroupBy(x => x.CategoryId).ToList();
                foreach (var categoryResult in categoryGroup)
                {
                    var category = new CategoryGridDto();

                    var spaceGrid = new List<SpaceGridDto>();

                    foreach (var result in categoryResult.OrderBy(x => x.SpaceCadNumber) )
                    {
                        areaType.AreaType = result.AreaTypeName.Trim();
                        areaType.AreaTypeId = result.AreaTypeId;

                        category.CategoryId = result.CategoryId;
                        category.Category = result.CategoryName.Trim();

                        var space = new SpaceGridDto
                        {
                            SpaceId = result.SpaceId,
                            Space = result.SpaceName.Trim(),
                            Unit = result.SpaceUnit,
                            Area = (decimal)result.SpaceArea,
                            Pieces = (decimal)result.SpacePieces,
                            Atricles = (decimal)result.SpaceAtricles
                        };
                        spaceGrid.Add(space);

                    }
                    category.Spaces = spaceGrid;
                    category.TotalArea = spaceGrid.Sum(x => x.Area);
                    categoryGrid.Add(category);

                };


                areaType.Categories = categoryGrid;
                areaType.TotalArea = categoryGrid.Sum(x => x.TotalArea);
                areaType.TotalAreaPercentage = 100;
                areaTypeGrid.Add(areaType);
            }


            foreach (var areatype in areaTypeGrid)
            {
                foreach (var category in areatype.Categories)
                {
                    category.TotalAreaPercentage = Math.Round((category.TotalArea / areatype.TotalArea) * 100, 0);
                }

            }


            var chartItems = new List<ChartGraphDto>();

            //Chart Total Area


            var areachartData = new ChartGraphDto();
            areachartData.ChartTitle = "Total Area";
            areachartData.ChartCategory = fileterValue;
            areachartData.ChartType = "Pie";

            var totalArea = areaTypeGrid.Sum(x => x.TotalArea);

            foreach (var areaType in areaTypeGrid)
            {
              

                var chartItem = new ChartItemDto
                {
                    CategoryId = areaType.AreaTypeId,
                    Key = areaType.AreaType,
                    Value = areaType.TotalArea,
                    TotalPercentage = Math.Round((areaType.TotalArea / totalArea) * 100, 0),
                    Unit = "m2"
                    
                   
                };                
                areachartData.chartItems.Add(chartItem);
            }

            if(areachartData.chartItems.Count > 0)
                chartItems.Add(areachartData);





            //Chart Pie Sales Area


            var areaTypeId = areaTypeGrid.Where( y => y.AreaType == "SalesArea").Select(x => x.AreaTypeId).FirstOrDefault();
            var categoriesItems = areaTypeGrid.Where( y => y.AreaType == "SalesArea").Select(x =>  x.Categories);
           
            foreach (var categories in categoriesItems)
            {
                var totalAreacategoriesItem = categories.Sum(x => x.TotalArea);


                var chartData = new ChartGraphDto();
                chartData.ChartTitle = "Sales Area";
                chartData.ChartCategory = fileterValue;
                chartData.ChartType = "Pie";
                foreach (var category in categories)
                {
                    var chartItem = new ChartItemDto
                    {
                        CategoryId = category.CategoryId,
                        ParentCategoryId = areaTypeId,
                        Key = category.Category,
                        Value =category.TotalArea,
                        TotalPercentage = Math.Round((category.TotalArea / totalAreacategoriesItem) * 100, 0),
                        Unit = "m2"
                    };

                    chartData.chartItems.Add(chartItem);

                    var spaceData = new ChartGraphDto();
                    spaceData.ChartTitle = category.Category;
                    spaceData.ChartCategory = fileterValue;
                    spaceData.ChartType = "Pie";

                    var totalspaceArea = category.Spaces.Sum(x => x.Area);


                    foreach (var spaceItem in category.Spaces)
                    {
                        var spaceChartItem = new ChartItemDto
                        {
                            CategoryId = spaceItem.SpaceId,
                            ParentCategoryId = category.CategoryId,
                            Key = spaceItem.Space,
                            Value = spaceItem.Area,
                            TotalPercentage = Math.Round((spaceItem.Area / totalspaceArea) * 100, 0),
                            Unit = spaceItem.Unit
                        };
                        spaceData.chartItems.Add(spaceChartItem);
                    }
                   // chartItems.Add(spaceData);
                }



                chartItems.Add(chartData);
            }




            //Chart Serice & various


            var mainAreaItems = areaTypeGrid.Where(y => y.AreaType != "SalesArea");

           

            foreach (var mainItem in mainAreaItems)
            {
              

                var chartData = new ChartGraphDto();
                chartData.ChartTitle = mainItem.AreaType;
                chartData.ChartCategory = fileterValue;
                chartData.ChartType = "Pie";

          
                foreach (var category in mainItem.Categories)
                {
                    var totalspaceArea = category.Spaces.Sum(x => x.Area);

                    foreach (var spaceItem in category.Spaces)
                    {
                        var spaceChartItem = new ChartItemDto
                        {
                            CategoryId = spaceItem.SpaceId,
                            ParentCategoryId = mainItem.AreaTypeId,
                            Key = spaceItem.Space,
                            Value = spaceItem.Area,
                            TotalPercentage = Math.Round((spaceItem.Area / totalspaceArea) * 100, 0),
                            Unit = spaceItem.Unit
                        };
                        chartData.chartItems.Add(spaceChartItem);
                    }
                    
                }



                chartItems.Add(chartData);
            }


            foreach (var categories in categoriesItems)
            {
                var totalAreacategoriesItem = categories.Sum(x => x.TotalArea);

                
                foreach (var category in categories)
                {                    

                    var spaceData = new ChartGraphDto();
                    spaceData.ChartTitle = category.Category;
                    spaceData.ChartCategory = fileterValue;
                    spaceData.ChartType = "Pie";

                    var totalspaceArea = category.Spaces.Sum(x => x.Area);


                    foreach (var spaceItem in category.Spaces)
                    {
                        var spaceChartItem = new ChartItemDto
                        {
                            CategoryId = spaceItem.SpaceId,
                            ParentCategoryId = category.CategoryId,
                            Key = spaceItem.Space,
                            Value = spaceItem.Area,
                            TotalPercentage = Math.Round((spaceItem.Area / totalspaceArea) * 100, 0),
                            Unit = spaceItem.Unit
                        };
                        spaceData.chartItems.Add(spaceChartItem);
                    }
                    chartItems.Add(spaceData);
                }



               
            }






            //Chart Bar Sales Area


            var categoriesBarItems = areaTypeGrid.Where(y => y.AreaType == "SalesArea").Select(x => x.Categories);

            foreach (var categories in categoriesBarItems)
            {
              
                foreach (var category in categories)
                {
                 

                    var spaceAtricleData = new ChartGraphDto();
                    spaceAtricleData.ChartTitle = category.Category + " Articles"; 
                    spaceAtricleData.ChartCategory = "Article"; 
                    spaceAtricleData.ChartType = "Bar"; 


                    var spacePiecesData = new ChartGraphDto();
                    spacePiecesData.ChartTitle = category.Category + " Pieces";
                    spacePiecesData.ChartCategory = "Article";
                    spacePiecesData.ChartType = "Bar";

                    foreach (var spaceItem in category.Spaces)
                    {
                      
                       var spaceArtlicleChartItem = new ChartItemDto
                        {
                            Key = spaceItem.Space,
                            Value = Math.Round( spaceItem.Atricles,0),
                            Unit = spaceItem.Unit
                        };
                        spaceAtricleData.chartItems.Add(spaceArtlicleChartItem);


                        var spacePieceChartItem = new ChartItemDto
                        {
                            Key = spaceItem.Space,
                            Value = Math.Round( spaceItem.Pieces,0),
                            Unit = spaceItem.Unit
                        };
                        spacePiecesData.chartItems.Add(spacePieceChartItem);
                    }



                    chartItems.Add(spaceAtricleData);
                    chartItems.Add(spacePiecesData);
                }



                
            }

            
            DraftCategoryChart(chartItems, StoreId,storeData.Id);


            var successResponse = new ResultDto<List<ChartGraphDto>>
            {
                IsSuccess = true,
                Data = chartItems
            };

            return successResponse;

        }
        catch (Exception ex)
        {
            var storeResult = new ResultDto<List<ChartGraphDto>>()
            {
                ErrorMessage = StringResources.InvalidArgument,
                StatusCode = HttpStatusCode.InternalServerError
            };
            return storeResult;
        }


    }


    public async Task<StoreData?> GetStoreData(Guid storeId,Guid CadFileTypeId, Guid? storeDataId)
    {
       
        if (storeDataId != null)
        {
            return await _repositoryContext.StoreDatas.FirstOrDefaultAsync(x => x.StoreId == storeId && x.CadFileTypeId == CadFileTypeId && x.Id == storeDataId);
        }        
        return await _repositoryContext.StoreDatas.Where(x => x.StoreId == storeId && x.CadFileTypeId == CadFileTypeId && x.StatusId == Guid.Parse(_configuration["StatusValues:StoreDataDefault"])).OrderByDescending(x => x.VersionNumber).FirstOrDefaultAsync();


    }

    /// <summary>
    /// Get Store Chart Data
    /// </summary>
    /// <param name="StoreId">Store Identifier</param>
    /// <param name="ct">cancellation token</param>
    /// <returns>Store Chart Data</returns>
    public async Task<ResultDto<List<StoreChartGraphDto>>> GetStoreChartData(Guid StoreId, Guid? StoreDataId, CancellationToken ct = default)
    {

        var store = await _repositoryContext.Stores.FirstOrDefaultAsync(x => x.Id == StoreId, ct);
        if (store == null)
        {
            var storeResult = new ResultDto<List<StoreChartGraphDto>>()
            {
                ErrorMessage = StringResources.NoResultsFound,
                IsSuccess = false
            };
            return storeResult;
        }

        var codeMaster = await _repositoryContext.CodeMasters.FirstOrDefaultAsync(x => x.Type == "CadType" && x.Value == "Space");

        if (codeMaster == null)
        {
            var storeResult = new ResultDto<List<StoreChartGraphDto>>()
            {
                ErrorMessage = StringResources.NoResultsFound,
                IsSuccess = false
            };
            return storeResult;
        }

        //var storeData = await _repositoryContext.StoreDatas.Where(x => x.StoreId == StoreId && x.CadFileTypeId == codeMaster.Id && x.StatusId == Guid.Parse(_configuration["StatusValues:StoreDataDefault"])).OrderByDescending(x => x.VersionNumber).FirstOrDefaultAsync();

        //if(StoreDataId != null)
        //{
        //    storeData = await _repositoryContext.StoreDatas.FirstOrDefaultAsync(x => x.StoreId == StoreId && x.CadFileTypeId == codeMaster.Id && x.Id == StoreDataId);
        //}

        var storeData = await GetStoreData(StoreId, codeMaster.Id, StoreDataId);

        if (storeData == null)
        {
            var storeResult = new ResultDto<List<StoreChartGraphDto>>()
            {
                ErrorMessage = StringResources.NoResultsFound,
                IsSuccess = false
            };
            return storeResult;
        }

        try
        {
            var query = (from at in _repositoryContext.AreaTypes
                         join cat in _repositoryContext.Categories on at.Id equals cat.AreaTypeId
                         join sp in _repositoryContext.Spaces on cat.Id equals sp.CategoryId
                         join stsp in _repositoryContext.StoreSpaces on sp.Id equals stsp.SpaceId
                         where stsp.StoreId == StoreId && stsp.StoreDataId == storeData.Id && stsp.CadFileTypeId == codeMaster.Id
                         orderby cat.CadServiceNumber
                         select new
                         {
                             CategoryId = cat.Id,
                             CategoryName = cat.Name,
                             AreaTypeId = at.Id,
                             AreaTypeName = at.Name,
                             SpaceId = sp.Id,
                             SpaceName = sp.Name,
                             SpaceUnit = stsp.Unit,
                             SpaceAtricles = stsp.Articles,
                             SpaceArea = stsp.Area,
                             SpacePieces = stsp.Pieces,
                             SpaceCadNumber = sp.CadServiceNumber

                         }).ToList();



            var areaTypeGrid = new List<ChartGridDto>();

            var areaTypeGroupResult = query.GroupBy(x => x.AreaTypeId).ToList();

            foreach (var item in areaTypeGroupResult)
            {
                var areaType = new ChartGridDto();

                var categoryGrid = new List<CategoryGridDto>();

                var categoryGroup = item.GroupBy(x => x.CategoryId).ToList();
                foreach (var categoryResult in categoryGroup)
                {
                    var category = new CategoryGridDto();

                    var spaceGrid = new List<SpaceGridDto>();

                    foreach (var result in categoryResult.OrderBy(x => x.SpaceCadNumber))
                    {
                        areaType.AreaType = result.AreaTypeName.Trim();
                        areaType.AreaTypeId = result.AreaTypeId;

                        category.CategoryId = result.CategoryId;
                        category.Category = result.CategoryName.Trim();

                        var space = new SpaceGridDto
                        {
                            SpaceId = result.SpaceId,
                            Space = result.SpaceName.Trim(),
                            Unit = result.SpaceUnit,
                            Area = (decimal)result.SpaceArea,
                            Pieces = (decimal)result.SpacePieces,
                            Atricles = (decimal)result.SpaceAtricles
                        };
                        spaceGrid.Add(space);

                    }
                    category.Spaces = spaceGrid;
                    category.TotalArea = spaceGrid.Sum(x => x.Area);
                    categoryGrid.Add(category);

                };


                areaType.Categories = categoryGrid;
                areaType.TotalArea = categoryGrid.Sum(x => x.TotalArea);
                areaType.TotalAreaPercentage = 100;
                areaTypeGrid.Add(areaType);
            }


            foreach (var areatype in areaTypeGrid)
            {
                foreach (var category in areatype.Categories)
                {
                    category.TotalAreaPercentage = Math.Round((category.TotalArea / areatype.TotalArea) * 100, 0);
                }

            }


            var chartItems = new List<StoreChartGraphDto>();

            //Chart Total Area


            var areachartData = new StoreChartGraphDto();
            areachartData.ChartTitle = "Total Area";
            areachartData.ChartCategory = "Space";
            areachartData.ChartType = "Pie";

            var totalArea = areaTypeGrid.Sum(x => x.TotalArea);

            foreach (var areaType in areaTypeGrid)
            {

                var areaChartItem = new StoreChartItemDto
                {
                    CategoryId = areaType.AreaTypeId,
                    Key = areaType.AreaType,
                    Value = areaType.TotalArea,
                    TotalPercentage = Math.Round((areaType.TotalArea / totalArea) * 100, 0),
                    Unit = "m2"


                };

                foreach (var category in areaType.Categories)
                {
                    var totalAreacategoriesItem = areaType.Categories.Sum(x => x.TotalArea);

                    var categoryChartItem = new StoreChartItemDto
                    {
                        CategoryId = category.CategoryId,
                        ParentCategoryId = areaType.AreaTypeId,
                        Key = category.Category,
                        Value = category.TotalArea,
                        TotalPercentage = Math.Round((category.TotalArea / totalAreacategoriesItem) * 100, 0),
                        Unit = "m2"
                    };

                   
                    

                    var totalspaceArea = category.Spaces.Sum(x => x.Area);

                    foreach (var spaceItem in category.Spaces)
                    {
                        var spaceChartItem = new StoreChartItemDto
                        {
                            CategoryId = spaceItem.SpaceId,
                            ParentCategoryId = category.CategoryId,
                            Key = spaceItem.Space,
                            Value = spaceItem.Area,
                            TotalPercentage = Math.Round((spaceItem.Area / totalspaceArea) * 100, 0),
                            Unit = spaceItem.Unit
                        };
                        categoryChartItem.chartItems.Add(spaceChartItem);
                    }


                    areaChartItem.chartItems.Add(categoryChartItem);


                   
                }

                areachartData.chartItems.Add(areaChartItem);
            }

            if (areachartData.chartItems.Count > 0)
                chartItems.Add(areachartData);



            //Chart Bar Sales Area


            var categoriesBarItems = areaTypeGrid.Where(y => y.AreaType == "SalesArea").Select(x => x.Categories);

            foreach (var categories in categoriesBarItems)
            {

                foreach (var category in categories)
                {


                    var spaceAtricleData = new StoreChartGraphDto();
                    spaceAtricleData.ChartTitle = category.Category + " Articles";
                    spaceAtricleData.ChartCategory = "Article";
                    spaceAtricleData.ChartType = "Bar";


                    var spacePiecesData = new StoreChartGraphDto();
                    spacePiecesData.ChartTitle = category.Category + " Pieces";
                    spacePiecesData.ChartCategory = "Article";
                    spacePiecesData.ChartType = "Bar";

                    foreach (var spaceItem in category.Spaces)
                    {

                        var spaceArtlicleChartItem = new StoreChartItemDto
                        {
                            Key = spaceItem.Space,
                            Value = Math.Round(spaceItem.Atricles, 0),
                            Unit = spaceItem.Unit
                        };
                        spaceAtricleData.chartItems.Add(spaceArtlicleChartItem);


                        var spacePieceChartItem = new StoreChartItemDto
                        {
                            Key = spaceItem.Space,
                            Value = Math.Round(spaceItem.Pieces, 0),
                            Unit = spaceItem.Unit
                        };
                        spacePiecesData.chartItems.Add(spacePieceChartItem);
                    }



                    chartItems.Add(spaceAtricleData);
                    chartItems.Add(spacePiecesData);
                }




            }


            DraftStoreCategoryChart(chartItems, StoreId, storeData.Id);


            var successResponse = new ResultDto<List<StoreChartGraphDto>>
            {
                IsSuccess = true,
                Data = chartItems
            };

            return successResponse;

        }
        catch (Exception ex)
        {
            var storeResult = new ResultDto<List<StoreChartGraphDto>>()
            {
                ErrorMessage = ex.Message,
                StatusCode = HttpStatusCode.InternalServerError
            };
            return storeResult;
        }


    }




    /// <summary>
    /// Update Store Images
    /// </summary>
    /// <param name="storeId">Store Identifier</param>
    /// <param name="imgUrl">Store Image URL</param>
    /// <param name="fileType">File Type</param>
    /// <param name="fileExtension">File Extension</param>
    /// <returns>True / False of Store Image Updation</returns>
    public async Task<ResultDto<bool>> UploadStoreImage(string storeId, string imgUrl, string fileType, string fileExtension)
    {
        if(storeId == null || imgUrl == null)
        {
            var customerResult = new ResultDto<bool>
            {
                ErrorMessage = StringResources.InvalidArgument,
                StatusCode = HttpStatusCode.BadRequest,
                Data = false

            };          


            return customerResult;
        }

        var imageItem = new Data.Entities.FileSystem.Image()
        {
            FileName = imgUrl,
            UploadedOn = DateTime.UtcNow,
            FileExtension = fileExtension,
            FileType = fileType,
            UploadedBy = "Retail"

        };

        var img = await InsertImage(imageItem);
        if(img == null)
        {
            var customerResult = new ResultDto<bool>
            {
                ErrorMessage = StringResources.InvalidArgument,
                StatusCode = HttpStatusCode.BadRequest,
                Data = false

            };

            return customerResult;
        }

        var storeImage = new StoreImage()
        {
            ImageId = img.Id,
            StoreId = Guid.Parse(storeId)
        };

        var storeResult = await InsertStoreImage(storeImage);

        if (storeResult == null)
        {
            var customerResult = new ResultDto<bool>
            {
                ErrorMessage = StringResources.InvalidArgument,
                StatusCode = HttpStatusCode.BadRequest,
                Data = false

            };
        }

        var result = new ResultDto<bool>
        {
           IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Data = true

        };


        return result;
    }



    /// <summary>
    /// Delete Store Images
    /// </summary>
    /// <param name="storeId">Store Identifier</param>
    /// <param name="storeImageId">Store Image Id</param>
    /// <param name="ImageId">Image  Id</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>True / False of Store Image Deletion</returns>
    public virtual async Task<ResultDto<bool>> DeleteStoreImage(Guid storeId, Guid storeImageId, Guid ImageId, CancellationToken ct = default)
    {
        var storeImage = await _repositoryContext.StoreImages.FirstOrDefaultAsync(x => x.Id == storeImageId && x.StoreId == storeId, ct);
        if (storeImage == null)
        {
            var response = new ResultDto<bool>
            {
                IsSuccess = false,
                ErrorMessage = StringResources.RecordNotFound
            };
            return response;

        }

        _repositoryContext.StoreImages.Remove(storeImage);
        await _repositoryContext.SaveChangesAsync(ct);

        var deleteImage = await DeleteImage(ImageId);

        var successResponse = new ResultDto<bool>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK
        };
        return successResponse;



    }


    /// <summary>
    /// Get all Drawing Grid Data By store
    /// </summary>
    /// <param name="StoreId">Store Identifier</param>
    /// <param name="ct">cancellation token</param>
    /// <returns>Store Drawing Tye Grid Data</returns>
    public async Task<ResultDto<List<DrawingListResponseDto>>> GetDrawingGridData(Guid StoreId, Guid? StoreDataId, CancellationToken ct = default)
    {

        var store = await _repositoryContext.Stores.FirstOrDefaultAsync(x => x.Id == StoreId, ct);
        if (store == null)
        {
            var storeResult = new ResultDto<List<DrawingListResponseDto>>()
            {
                ErrorMessage = StringResources.NoResultsFound,
                IsSuccess = false
            };
            return storeResult;
        }

        var filterValue = "Drawing";


        var codeMaster = await _repositoryContext.CodeMasters.FirstOrDefaultAsync(x => x.Type == "CadType" && x.Value.ToLower().Trim() == filterValue.ToLower().Trim());


        if (codeMaster == null)
        {
            var storeResult = new ResultDto<List<DrawingListResponseDto>>()
            {
                ErrorMessage = StringResources.NoResultsFound,
                IsSuccess = false
            };
            return storeResult;
        }



        var storeData = await GetStoreData(StoreId, codeMaster.Id, StoreDataId);

        if (storeData == null)
        {
            var storeResult = new ResultDto<List<DrawingListResponseDto>>()
            {
                ErrorMessage = StringResources.NoResultsFound,
                IsSuccess = false
            };
            return storeResult;
        }

        var storeDrawingList = await _repositoryContext.DrawingLists.Where(x => x.StoreId == StoreId && x.StoreDataId == storeData.Id).ToListAsync();

        if (storeDrawingList.Count <= 0)
        {
            var storeResult = new ResultDto<List<DrawingListResponseDto>>()
            {
                ErrorMessage = StringResources.NoResultsFound,
                IsSuccess = false
            };
            return storeResult;
        }

        var storeDrawingResponse = _mapper.Map<List<DrawingListResponseDto>>(storeDrawingList);


      

        var response = new ResultDto<List<DrawingListResponseDto>>()
        {
            IsSuccess = true,
            Data = storeDrawingResponse
        };
        return response;

    }



    /// <summary>
    /// Get all Order Grid Data
    /// </summary>
    /// <param name="StoreId">Store Identifier</param>
    /// <param name="ct">cancellation token</param>
    /// <returns>Store Grid Data</returns>
    public async Task<ResultDto<PackageDataDto>> GetOrderListGridData(Guid StoreId, CancellationToken ct = default)
    {

        var storePackageList = await _repositoryContext.PackageDatas.Where(x => x.StoreId == StoreId).FirstOrDefaultAsync();

        if (storePackageList == null)
        {
            var storeResult = new ResultDto<PackageDataDto>()
            {
                ErrorMessage = StringResources.NoResultsFound,
                IsSuccess = false
            };
            return storeResult;
        }

        var storePackageResponse = _mapper.Map<PackageDataDto>(storePackageList);


        if (storePackageList.StatusId != null)
        {

            var result = _repositoryContext.CodeMasters.Where(x => x.Id == storePackageList.StatusId).FirstOrDefault();
            if (result != null)
                storePackageResponse.StatusName = result.Value;
        }


        if (storePackageList.StoreId != null)
        {

            var result = _repositoryContext.Stores.Where(x => x.Id == storePackageList.StoreId).FirstOrDefault();
            if (result != null)
                storePackageResponse.StoreName = result.Name;
        }


        var storeOrderList = await _repositoryContext.OrderLists.Where(x => x.StoreId == StoreId && x.PackageDataId == storePackageResponse.Id).ToListAsync();

        var storeOrderListResponse = _mapper.Map<List<OrderListDto>>(storeOrderList);

        storePackageResponse.OrderList = storeOrderListResponse;

          var response = new ResultDto<PackageDataDto>()
        {
            IsSuccess = true,
            Data = storePackageResponse
          };
        return response;

    }




    /// <summary>
    /// Get all General List Grid Data
    /// </summary>
    /// <param name="StoreId">Store Identifier</param>
    /// <param name="ct">cancellation token</param>
    /// <returns>Store Grid Data</returns>
    public async Task<ResultDto<List<GeneralListTypeDataDto>>> GetGeneralListTypeGridData(Guid StoreId, CancellationToken ct = default)
    {

        var generalList = await _repositoryContext.GeneralListTypeDatas.Where(x => x.StoreId == StoreId).ToListAsync(ct);

        if (generalList.Count <= 0)
        {
            var storeResult = new ResultDto<List<GeneralListTypeDataDto>>()
            {
                ErrorMessage = StringResources.NoResultsFound,
                IsSuccess = false
            };
            return storeResult;
        }

     

        var generalListData = new List<GeneralListTypeDataDto>();
       

        foreach (var item in generalList)
        {
            var model = new GeneralListTypeDataDto
            {
                Id = item.Id,
                Name = item.Koncept1,
                RealPercentage = item.RealPercentage,
                Realm2 = item.Realm2
            };

            if (item.Koncept2 != null)
                model.Name = model.Name + " " + item.Koncept2;


            if (item.StoreId != null)
            {
                model.StoreId = StoreId;
                var result = _repositoryContext.Stores.Where(x => x.Id == item.StoreId).FirstOrDefault();
                if (result != null)
                    model.StoreName = result.Name;
            }

            generalListData.Add(model);

        }
        

        var response = new ResultDto<List<GeneralListTypeDataDto>>()
        {
            IsSuccess = true,
            Data = generalListData
        };
        return response;

    }




    /// <summary>
    /// Insert Store Information
    /// </summary>
    /// <param name="storeDto">store</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Store Infromation</returns>
    public async Task<ResultDto<StoreResponseDto>> InsertStore(StoreDto storeDto, CancellationToken ct = default)
    {
        if (storeDto == null)
        {
            var errorResponse = new ResultDto<StoreResponseDto>
            {
                ErrorMessage = StringResources.RecordNotFound,
                IsSuccess = false
            };
            return errorResponse;
        }


        var store = _mapper.Map<Store>(storeDto);

        await _repositoryContext.Stores.AddAsync(store, ct);
        await _repositoryContext.SaveChangesAsync(ct);


        var storeResponse = await GetStoreById(store.Id,ct);


        var result = new ResultDto<StoreResponseDto>
        {
            Data = storeResponse.Data,
            IsSuccess = true
        };

        return result;

    }


    /// <summary>
    /// Update Store Information
    /// </summary>
    /// <param name="id">Store Id</param>
    /// <param name="storeDto">Store</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Updated Store Information</returns>
    public async Task<ResultDto<StoreResponseDto>> UpdateStore(string storeId, StoreDto storeDto, CancellationToken ct = default)
    {

        if (storeDto == null)
        {
            var errorResponse = new ResultDto<StoreResponseDto>
            {
                ErrorMessage = StringResources.BadRequest,
                StatusCode = HttpStatusCode.BadRequest
            };
            return errorResponse;
        }

        var result = await _repositoryContext.Stores.FirstOrDefaultAsync(x => x.Id == new Guid(storeId), ct);


        if (result == null)
        {
            var errorResponse = new ResultDto<StoreResponseDto>
            {
                ErrorMessage = StringResources.RecordNotFound,
                StatusCode = HttpStatusCode.NotFound
            };
            return errorResponse;
        }


        result.Name = storeDto.Name;
        result.TotalArea = storeDto.TotalArea;
        result.StoreNumber = storeDto.StoreNumber;
        result.StatusId = storeDto.StatusId;
        await _repositoryContext.SaveChangesAsync(ct);

        var address = await UpdateCustomerAddress((Guid)storeDto.AddressId, storeDto.Address, ct);

       

        var storeResponse = await GetStoreById(new Guid(storeId), ct);

        var storeResult = new ResultDto<StoreResponseDto>
        {
            Data = storeResponse.Data,
            IsSuccess = true
        };
        return storeResult;

    }



    /// <summary>
    /// Delete Store
    /// </summary>
    /// <param name="id">Store Id</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>True / False status of Store Deletion</returns>
    public async Task<ResultDto<bool>> DeleteStore(string id, CancellationToken ct = default)
    {

        var store = await _repositoryContext.Stores.FirstOrDefaultAsync(x => x.Id == new Guid(id), ct);

        if (store == null)
        {
            var response = new ResultDto<bool>
            {
                ErrorMessage = StringResources.RecordNotFound,
                StatusCode = HttpStatusCode.NotFound
            };
            return response;

        }
        var addressId = store.AddressId;

        _repositoryContext.Stores.Remove(store);

        await _repositoryContext.SaveChangesAsync(ct);

       await DeleteAddress(addressId, ct);

        var successResponse = new ResultDto<bool>
        {
            IsSuccess = true,
            Data = true
        };

        return successResponse;

    }


    /// <summary>
    /// Get Store By Id
    /// </summary>
    /// <param name="storeId">customerId</param>
    /// <param name="ct">cancellation token</param>
    /// <returns>Store Information</returns>
    public async Task<ResultDto<StoreResponseDto>> GetStoreById(Guid storeId, CancellationToken ct = default)
    {

        var storeItem = await _repositoryContext.Stores.Where(x => x.Id == storeId).FirstOrDefaultAsync();

        if (storeItem == null)
        {
            var errorResponse = new ResultDto<StoreResponseDto>
            {
                IsSuccess = false,
                ErrorMessage = StringResources.NoResultsFound,
                StatusCode = HttpStatusCode.NoContent
            };

            return errorResponse;
        }

        var codeMaster = await _repositoryContext.CodeMasters.FirstOrDefaultAsync(x => x.Type == "CadType" && x.Value == "Space");

        if (codeMaster == null)
        {
            var storeResult = new ResultDto<StoreResponseDto>
            {
                ErrorMessage = StringResources.NoResultsFound,
                IsSuccess = false
            };
            return storeResult;
        }


        var updateActivityResult = await UpdateStoreActivity(storeId);

        var store = _mapper.Map<StoreResponseDto>(storeItem);

        store.StoreSearchkey = store.Name + "-" + store.StoreNumber;


        var path = _configuration["AssetLocations:StoreImages"];


        var storeData = await _repositoryContext.StoreDatas.Where(x => x.StoreId == store.Id).ToListAsync();
        
        if (storeData.Count > 0)
        {
            foreach (var item in storeData)
            {
                var storeDataVersion = new StoreDataVersion()
                {
                    Id = item.Id,
                    Version = item.VersionName ,
                    VersionNumber = item.VersionNumber

                };

                var storeUploadType = await _codeMasterService.GetStatusById(item.CadFileTypeId, null, default);
                if (storeUploadType.Data != null)
                {
                    storeDataVersion.CadTypeId = storeUploadType.Data.Id;
                    storeDataVersion.CadTypeName = storeUploadType.Data.Value;
                }
                store.storeDataVersions.Add(storeDataVersion);
            }
        }


        if (store.AddressId != null)
        {

            var result = await getAddressById((Guid)store.AddressId);
            if (result != null)
            {
                var customerAddress = _mapper.Map<AddressDto>(result);
                store.Address = customerAddress;
            }

        }

        if (store.CustomerId != null)
        {

            var result = await getCustomerById((Guid)store.CustomerId);
            if (result != null)
            {
                var customer = _mapper.Map<CustomerDto>(result);
                store.customer = customer;
            }
    ;
        }

        if (store.StatusId != null)
        {

            var result = await getStatusById((Guid)store.StatusId);
            if (result != null)
            {

                store.StoreStatus = result.Value;
            }
    ;
        }


        var storeImages = await GetStoreImagesByStoreId(store.Id);

        foreach (var img in storeImages)
        {
            var image = await GetImageById((Guid)img.ImageId);
            if (image != null)
            {
                var storeImageItem = new ImageDto()
                {
                    Id = img.Id,
                    ImageId = image.Id,
                    ImageUrl = path + image.FileName

                };

                string fileName = Path.GetFileNameWithoutExtension(image.FileName);
                string filePath = Path.GetDirectoryName(image.FileName);


                if (System.IO.Directory.Exists(_environment.WebRootPath + path + filePath))
                {
                    DirectoryInfo dir = new DirectoryInfo(_environment.WebRootPath + path + filePath);


                    FileInfo[] filesInDir = dir.GetFiles("*" + fileName + "*.*");


                    foreach (var item in filesInDir)
                    {
                        var itemName = Path.GetFileNameWithoutExtension(item.Name);
                        if (itemName != fileName)
                            storeImageItem.ThumnailUrls.Add(path + store.Id + "/" + item.Name);
                    }


                  
                }
                store.StoreImages.Add(storeImageItem);
            }


        }

        //cad upload History


        var storeDataHistory = await StoreDataFileHistoryByStoreId(storeId);
        store.StoreDataHistories = storeDataHistory;

        var storeLiveData = storeData?.Where(x => x.StoreId == store.Id && x.CadFileTypeId == codeMaster.Id && x.StatusId == Guid.Parse(_configuration["StatusValues:StoreDataDefault"])).OrderByDescending(x => x.VersionNumber).FirstOrDefault();

            if(storeLiveData != null)
            {

                store.PdfLink = await PdfFileUrlByStoreId(store.Id, storeLiveData.Id);

                var uploadHistory = await _repositoryContext.CadUploadHistories.Where(x => x.StoreId == store.Id && x.StoreDataId == storeLiveData.Id && x.Status).OrderByDescending(y => y.UploadOn).FirstOrDefaultAsync();

                if (uploadHistory != null)
                {
                    var cadResponse = _mapper.Map<DTOs.Cad.CadUploadHistoryResponseDto>(uploadHistory);
                    store.cadUploadHistory = cadResponse;
                }

            }

        var response = new ResultDto<StoreResponseDto>
        {
            IsSuccess = true,
            Data = store,
        };
        return response;

    }



    /// <summary>
    /// Get all StoreStatus
    /// </summary>
    /// <param name="pageIndex">PageIndex</param>
    /// <param name="pageSize">PageSize</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Get Store status with Pagination</returns>
    public virtual async Task<PaginationResultDto<PagedList<StoreStatusResponseDto>>> GetAllStoreStatus(string keyword = null, int pageIndex = 0, int pageSize = 0, CancellationToken ct = default)
    {

        var query = from p in _repositoryContext.CodeMasters
                    where p.Type== "StoreStatus"
                    select p;

        if (keyword != null)
        {
            query = query.Where(x => x.Value.Contains(keyword));
        }

        var  StoreStatus = await PagedList<CodeMaster>.ToPagedList(query.OrderBy(on => on.Order), pageIndex, pageSize);
        if ( StoreStatus == null)
        {
            var errorResponse = new PaginationResultDto<PagedList<StoreStatusResponseDto>>
            {
                ErrorMessage = StringResources.NoResultsFound,
                StatusCode = HttpStatusCode.NoContent
            };
            return errorResponse;
        }

        var  StoreStatusResponse = _mapper.Map<PagedList<StoreStatusResponseDto>>( StoreStatus);



        var response = new PaginationResultDto<PagedList<StoreStatusResponseDto>>
        {
            IsSuccess = true,
            Data =  StoreStatusResponse,
            TotalCount =  StoreStatus.TotalCount,
            TotalPages =  StoreStatus.TotalPages
        };
        return response;
    }



    /// <summary>
    /// Get all StoreDataStatus
    /// </summary>
    /// <param name="pageIndex">PageIndex</param>
    /// <param name="pageSize">PageSize</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Get Store status with Pagination</returns>
    public virtual async Task<PaginationResultDto<PagedList<StoreStatusResponseDto>>> GetAllStoreDataStatus(string keyword = null, int pageIndex = 0, int pageSize = 0, CancellationToken ct = default)
    {

        var query = from p in _repositoryContext.CodeMasters
                    where p.Type == "StoreDataStatus"
                    select p;

        if (keyword != null)
        {
            query = query.Where(x => x.Value.Contains(keyword));
        }

        var StoreStatus = await PagedList<CodeMaster>.ToPagedList(query.OrderBy(on => on.Order), pageIndex, pageSize);
        if (StoreStatus == null)
        {
            var errorResponse = new PaginationResultDto<PagedList<StoreStatusResponseDto>>
            {
                ErrorMessage = StringResources.NoResultsFound
            };
            return errorResponse;
        }

        var StoreStatusResponse = _mapper.Map<PagedList<StoreStatusResponseDto>>(StoreStatus);



        var response = new PaginationResultDto<PagedList<StoreStatusResponseDto>>
        {
            IsSuccess = true,
            Data = StoreStatusResponse,
            TotalCount = StoreStatus.TotalCount,
            TotalPages = StoreStatus.TotalPages
        };
        return response;
    }


    /// <summary>
    /// Get Comparision chart data of Two versions
    /// </summary>
    /// <param name="FirstStoreId">First Store ID</param>
    /// <param name="FirstVersionId">First Version ID</param>
    /// <param name="SecondStoreId">Second Store ID</param>
    /// <param name="SecondVersionId">Second Version ID</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Comparision chart data</returns>
    public async Task<ResultDto<List<ComparisionChartGraphDto>>> CompareStoreVersionData(Guid FirstStoreId, Guid FirstVersionId, Guid SecondStoreId, Guid SecondVersionId,string? type, CancellationToken ct = default)
    {

        var store = await _repositoryContext.Stores.FirstOrDefaultAsync(x => x.Id == FirstStoreId, ct);
        if (store == null)
        {
            var storeResult = new ResultDto<List<ComparisionChartGraphDto>>()
            {
                ErrorMessage = StringResources.NoResultsFound,
                StatusCode = HttpStatusCode.NotFound,
                Data = null
            };
            return storeResult;
        }

        var secondStore = await _repositoryContext.Stores.FirstOrDefaultAsync(x => x.Id == FirstStoreId, ct);
        if (secondStore == null)
        {
            var storeResult = new ResultDto<List<ComparisionChartGraphDto>>()
            {
                ErrorMessage = StringResources.NoResultsFound,
                StatusCode = HttpStatusCode.NotFound,
                Data = null
            };
            return storeResult;
        }

        var storeDataV1 = await _repositoryContext.StoreDatas.Where(x => x.StoreId == FirstStoreId && x.Id == FirstVersionId).FirstOrDefaultAsync();
        if (storeDataV1 == null)
        {
            var storeResult = new ResultDto<List<ComparisionChartGraphDto>>()
            {
                ErrorMessage = StringResources.NoResultsFound,
                StatusCode = HttpStatusCode.OK,
                Data = null
            };
            return storeResult;
        }



        var storeDataV2 = await _repositoryContext.StoreDatas.Where(x => x.StoreId == SecondStoreId && x.Id == SecondVersionId).FirstOrDefaultAsync();
        if (storeDataV2 == null)
        {
            var storeResult = new ResultDto<List<ComparisionChartGraphDto>>()
            {
                ErrorMessage = StringResources.NoResultsFound,
                StatusCode = HttpStatusCode.OK,
                Data = null
            };
            return storeResult;
        }


        try
        {
        
            var version1 = await GetChartDataItem(FirstStoreId, storeDataV1.Id, type);

            var version2 = await GetChartDataItem(SecondStoreId, storeDataV2.Id, type);

            var comparisionList = new List<ComparisionChartGraphDto>();

            foreach(var item in version1)
            {
                var comparisionChart = new ComparisionChartGraphDto
                {
                    ChartTitle = item.ChartTitle,
                    ChartCategory = item.ChartCategory,
                    ChartType = item.ChartType
                };

                var itemVersion2 = version2.Where(x => x.ChartTitle == item.ChartTitle && x.ChartCategory == item.ChartCategory).FirstOrDefault();

                foreach(var chartItem in item.chartItems)
                {
                    var comparisonchartItem = new ComparisionChartItemDto();

                    if(itemVersion2 != null)
                    {
                        var chartItemVersion2 = itemVersion2.chartItems.Where(x => x.Key == chartItem.Key).FirstOrDefault();

                        if (chartItemVersion2 != null)
                        {
                            comparisonchartItem.V2Value = chartItemVersion2.Value;
                            comparisonchartItem.V2TotalPercentage = chartItemVersion2.TotalPercentage;
                        }

                    }
                   

                    comparisonchartItem.Unit = chartItem.Unit;
                    comparisonchartItem.Key = chartItem.Key;

                    comparisonchartItem.V1Value = chartItem.Value;
                    comparisonchartItem.V1TotalPercentage = chartItem.TotalPercentage;

                    comparisionChart.chartItems.Add(comparisonchartItem);

                }


                if (itemVersion2 != null)
                {
                    if(itemVersion2.chartItems.Count > 0 && item.chartItems.Count > 0)
                    {
                        var UpdatedChartItem = itemVersion2.chartItems.Where(s2 => !item.chartItems.Any(s1 => s1.Key == s2.Key)).ToList();
                        foreach(var chartItem in UpdatedChartItem)
                        {
                            var comparisonchartItem = new ComparisionChartItemDto();

                            comparisonchartItem.Unit = chartItem.Unit;
                            comparisonchartItem.Key = chartItem.Key;

                            comparisonchartItem.V2Value = chartItem.Value;
                            comparisonchartItem.V2TotalPercentage = chartItem.TotalPercentage;

                            comparisionChart.chartItems.Add(comparisonchartItem);
                        }

                    }


                }


                comparisionList.Add(comparisionChart);
            }




            var successResponse = new ResultDto<List<ComparisionChartGraphDto>>
            {
                IsSuccess = true,
                Data = comparisionList
            };

            return successResponse;

        }
        catch (Exception ex)
        {
            var storeResult = new ResultDto<List<ComparisionChartGraphDto>>()
            {
                ErrorMessage = ex.Message,
                StatusCode = HttpStatusCode.InternalServerError
            };
            return storeResult;
        }


    }



    /// <summary>
    /// Get chart data By Store Id
    /// </summary>
    /// <param name="storeId">Store ID</param>
    /// <param name="storeDataId">Store Data ID</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns></returns>
    public async Task<List<ChartGraphDto>> GetChartDataItem(Guid storeId, Guid storeDataId,string? type = null)
    {
        var query = (from at in _repositoryContext.AreaTypes
                     join cat in _repositoryContext.Categories on at.Id equals cat.AreaTypeId
                     join sp in _repositoryContext.Spaces on cat.Id equals sp.CategoryId
                     join stsp in _repositoryContext.StoreSpaces on sp.Id equals stsp.SpaceId
                     where stsp.StoreId == storeId && stsp.StoreDataId == storeDataId
                     orderby cat.CadServiceNumber
                     select new
                     {
                         CategoryId = cat.Id,
                         CategoryName = cat.Name,
                         AreaTypeId = at.Id,
                         AreaTypeName = at.Name,
                         SpaceName = sp.Name,
                         SpaceUnit = stsp.Unit,
                         SpaceAtricles = stsp.Articles,
                         SpaceArea = stsp.Area,
                         SpacePieces = stsp.Pieces,
                         SpaceCadNumber = sp.CadServiceNumber

                     }).ToList();

        var fileterValue = "Space";

        if (type != null)
            fileterValue = type;


        var areaTypeGrid = new List<ChartGridDto>();

        var areaTypeGroupResult = query.GroupBy(x => x.AreaTypeId).ToList();

        foreach (var item in areaTypeGroupResult)
        {
            var areaType = new ChartGridDto();

            var categoryGrid = new List<CategoryGridDto>();

            var categoryGroup = item.GroupBy(x => x.CategoryId).ToList();
            foreach (var categoryResult in categoryGroup)
            {
                var category = new CategoryGridDto();

                var spaceGrid = new List<SpaceGridDto>();

                foreach (var result in categoryResult.OrderBy(x => x.SpaceCadNumber))
                {
                    areaType.AreaType = result.AreaTypeName.Trim();
                    areaType.AreaTypeId = result.AreaTypeId;

                    category.CategoryId = result.CategoryId;
                    category.Category = result.CategoryName.Trim();

                    var space = new SpaceGridDto
                    {
                        Space = result.SpaceName.Trim(),
                        Unit = result.SpaceUnit,
                        Area = (decimal)result.SpaceArea,
                        Pieces = (decimal)result.SpacePieces,
                        Atricles = (decimal)result.SpaceAtricles
                    };
                    spaceGrid.Add(space);

                }
                category.Spaces = spaceGrid;
                category.TotalArea = spaceGrid.Sum(x => x.Area);
                categoryGrid.Add(category);

            };


            areaType.Categories = categoryGrid;
            areaType.TotalArea = categoryGrid.Sum(x => x.TotalArea);
            areaType.TotalAreaPercentage = 100;
            areaTypeGrid.Add(areaType);
        }


        foreach (var areatype in areaTypeGrid)
        {
            foreach (var category in areatype.Categories)
            {
                category.TotalAreaPercentage = Math.Round((category.TotalArea / areatype.TotalArea) * 100, 0);
            }

        }


        var chartItems = new List<ChartGraphDto>();

        //Chart Total Area
        var areachartData = new ChartGraphDto();
        areachartData.ChartTitle = "Total Area";
        areachartData.ChartCategory = fileterValue;
        areachartData.ChartType = "Pie";

        var totalArea = areaTypeGrid.Sum(x => x.TotalArea);

        foreach (var areaType in areaTypeGrid)
        {
            var chartItem = new ChartItemDto
            {
                Key = areaType.AreaType,
                Value = areaType.TotalArea,
                TotalPercentage = Math.Round((areaType.TotalArea / totalArea) * 100, 0),
                Unit = "m2"


            };
            areachartData.chartItems.Add(chartItem);
        }

        if (areachartData.chartItems.Count > 0)
            chartItems.Add(areachartData);





        //Chart Pie Sales Area
        var categoriesItems = areaTypeGrid.Where(y => y.AreaType == "SalesArea").Select(x => x.Categories);


        foreach (var categories in categoriesItems)
        {
            var totalAreacategoriesItem = categories.Sum(x => x.TotalArea);


            var chartData = new ChartGraphDto();
            chartData.ChartTitle = "Sales Area";
            chartData.ChartCategory = fileterValue;
            chartData.ChartType = "Pie";
            foreach (var category in categories)
            {
                var chartItem = new ChartItemDto
                {
                    Key = category.Category,
                    Value = category.TotalArea,
                    TotalPercentage = Math.Round((category.TotalArea / totalAreacategoriesItem) * 100, 0),
                    Unit = "m2"
                };

                chartData.chartItems.Add(chartItem);

                var spaceData = new ChartGraphDto();
                spaceData.ChartTitle = category.Category;
                spaceData.ChartCategory = fileterValue;
                spaceData.ChartType = "Pie";

                var totalspaceArea = category.Spaces.Sum(x => x.Area);


                foreach (var spaceItem in category.Spaces)
                {
                    var spaceChartItem = new ChartItemDto
                    {
                        Key = spaceItem.Space,
                        Value = spaceItem.Area,
                        TotalPercentage = Math.Round((spaceItem.Area / totalspaceArea) * 100, 0),
                        Unit = spaceItem.Unit
                    };
                    spaceData.chartItems.Add(spaceChartItem);
                }
                chartItems.Add(spaceData);
            }



            chartItems.Add(chartData);
        }


        //Chart Serice & various
        var mainAreaItems = areaTypeGrid.Where(y => y.AreaType != "SalesArea");



        foreach (var mainItem in mainAreaItems)
        {


            var chartData = new ChartGraphDto();
            chartData.ChartTitle = mainItem.AreaType;
            chartData.ChartCategory = fileterValue;
            chartData.ChartType = "Pie";


            foreach (var category in mainItem.Categories)
            {
                var totalspaceArea = category.Spaces.Sum(x => x.Area);

                foreach (var spaceItem in category.Spaces)
                {
                    var spaceChartItem = new ChartItemDto
                    {
                        Key = spaceItem.Space,
                        Value = spaceItem.Area,
                        TotalPercentage = Math.Round((spaceItem.Area / totalspaceArea) * 100, 0),
                        Unit = spaceItem.Unit
                    };
                    chartData.chartItems.Add(spaceChartItem);
                }

            }



            chartItems.Add(chartData);
        }




        //Chart Bar Sales Area
        var categoriesBarItems = areaTypeGrid.Where(y => y.AreaType == "SalesArea").Select(x => x.Categories);

        foreach (var categories in categoriesBarItems)
        {

            foreach (var category in categories)
            {


                var spaceAtricleData = new ChartGraphDto();
                spaceAtricleData.ChartTitle = category.Category + " Articles";
                spaceAtricleData.ChartCategory = "Article";
                spaceAtricleData.ChartType = "Bar";


                var spacePiecesData = new ChartGraphDto();
                spacePiecesData.ChartTitle = category.Category + " Pieces";
                spacePiecesData.ChartCategory = "Article";
                spacePiecesData.ChartType = "Bar";

                foreach (var spaceItem in category.Spaces)
                {

                    var spaceArtlicleChartItem = new ChartItemDto
                    {
                        Key = spaceItem.Space,
                        Value = Math.Round(spaceItem.Atricles, 0),
                        Unit = spaceItem.Unit
                    };
                    spaceAtricleData.chartItems.Add(spaceArtlicleChartItem);


                    var spacePieceChartItem = new ChartItemDto
                    {
                        Key = spaceItem.Space,
                        Value = Math.Round(spaceItem.Pieces, 0),
                        Unit = spaceItem.Unit
                    };
                    spacePiecesData.chartItems.Add(spacePieceChartItem);
                }



                chartItems.Add(spaceAtricleData);
                chartItems.Add(spacePiecesData);
            }
        }


        DraftCategoryChart(chartItems, storeId, storeDataId);

        return chartItems;
    }


    /// <summary>
    /// Get all Chart Data
    /// </summary>
    /// <param name="StoreId">Store Identifier</param>
    /// <param name="ct">cancellation token</param>
    /// <returns>Store Chart Data</returns>
    public async Task<ResultDto<CustomerStoresDto>> StoreDataByCustomerId(Guid CustomerId, string? type = null, CancellationToken ct = default)
    {      

        var fileterValue = "Space";

        if (type != null)
            fileterValue = type;

        var codeMaster = await _repositoryContext.CodeMasters.FirstOrDefaultAsync(x => x.Type == "CadType" && x.Value.ToLower().Trim() == fileterValue.ToLower().Trim());


        if (codeMaster == null)
        {
            var storeResult = new ResultDto<CustomerStoresDto>()
            {
                ErrorMessage = StringResources.NoResultsFound,
                IsSuccess = false
            };
            return storeResult;
        }
       

        try
        {
            var customerStores = new CustomerStoresDto();

            var allStores = _repositoryContext.Stores
           .Where(sd => sd.CustomerId == CustomerId);



            var query = (from st in _repositoryContext.Stores
                        join sd in _repositoryContext.StoreDatas on st.Id equals sd.StoreId
                        join stsp in _repositoryContext.StoreSpaces on sd.Id equals stsp.StoreDataId
                        join sp in _repositoryContext.Spaces on stsp.SpaceId equals sp.Id
                        join cat in _repositoryContext.Categories on sp.CategoryId equals cat.Id
                        join at in _repositoryContext.AreaTypes on cat.AreaTypeId equals at.Id
                        where st.CustomerId == CustomerId && sd.CadFileTypeId == codeMaster.Id && sd.StatusId == Guid.Parse(_configuration["StatusValues:StoreDataDefault"])
                        select new
                        {
                            CategoryId = cat.Id,
                            CategoryName = cat.Name,
                            AreaTypeId = at.Id,
                            AreaTypeName = at.Name,
                            SpaceId = sp.Id,
                            SpaceName = sp.Name,
                            SpaceUnit = stsp.Unit,
                            SpaceAtricles = stsp.Articles,
                            SpaceArea = stsp.Area,
                            SpacePieces = stsp.Pieces,
                            SpaceCadNumber = sp.CadServiceNumber,
                            StoreId = st.Id,
                            StoreName = st.Name,
                            StoreDataId = sd.Id,
                            StoreNumber = st.StoreNumber

                        }).ToList();


            var stores = query.GroupBy(x => x.StoreId).ToList();

            var storeItems = new List<StoreDataDto>();
            var columnList = new List<CoulmnListDto>
            {
                new CoulmnListDto { Name = "Store Number" },
                new CoulmnListDto { Name = "Store Name" }
            };


            foreach (var storeItem in stores)
            {
                    
                    var storeInfo = new StoreDataDto();

                    var areaTypeGroups = storeItem.GroupBy(x => x.AreaTypeId).ToList();

                    foreach (var item in areaTypeGroups)
                    {
                        var areaItem = new CoulmnDataDto();
                        areaItem.Value = (decimal)item.Sum(x => x.SpaceArea);

                        var areTypeItem = item.FirstOrDefault();
                        if (areTypeItem != null)
                        {

                            var areaColumn = new CoulmnListDto
                            {
                                IsParent = true,
                                Id = areTypeItem.AreaTypeId,
                                Name = areTypeItem.AreaTypeName
                            };

                            if (!columnList.Exists(x => x.Id == areaColumn.Id))
                                    columnList.Add(areaColumn);
                        }
                                        

                        var categoryGroup = item.GroupBy(x => x.CategoryId).ToList();

                        foreach (var categoryResult in categoryGroup)
                        {
                            var categoryItem = new CoulmnDataDto();
                            categoryItem.Value = (decimal)categoryResult.Sum(x => x.SpaceArea);

                            var catItem = categoryResult.FirstOrDefault();
                            if(catItem != null)
                            {

                                var categoryColumn = new CoulmnListDto
                                {
                                    IsParent = true,
                                    Id = catItem.CategoryId,
                                    Name = catItem.CategoryName
                                };

                                if (!columnList.Exists(x => x.Id == categoryColumn.Id))
                                        columnList.Add(categoryColumn);
                            }

                            foreach (var result in categoryResult)
                            {

                                    categoryItem.Id = result.CategoryId;
                                    categoryItem.Name = result.CategoryName;

                                    areaItem.Id = result.AreaTypeId;
                                    areaItem.Name = result.AreaTypeName;

                                    var spaceColumn = new CoulmnListDto();
                                    spaceColumn.Name = result.SpaceName.Trim();
                                    spaceColumn.Id = result.SpaceId;


                                storeInfo.StoreName = result.StoreName;
                                storeInfo.StoreId = result.StoreId;
                                storeInfo.StoreNumber = result.StoreNumber;

                                var spaceItem = new CoulmnDataDto
                                    {
                                            Id = result.SpaceId,
                                            Name = result.SpaceName.Trim(),
                                            Value = (decimal)result.SpaceArea,
                                    };

                                    storeInfo.CoulmnData.Add(spaceItem);

                                if (!columnList.Exists(x => x.Id == spaceColumn.Id))
                                    columnList.Add(spaceColumn);


                                }

                            storeInfo.CoulmnData.Add(categoryItem);

                        }

                    storeInfo.CoulmnData.Add(areaItem);

                  
                }
                    
                    storeItems.Add(storeInfo);
            }

            foreach (var item in allStores)
            {              

                if(!storeItems.Exists(x => x.StoreId == item.Id))
                {
                    var storeInfo = new StoreDataDto
                    {
                        StoreId = item.Id,
                        StoreName = item.Name,
                         StoreNumber = item.StoreNumber
                };

                    storeItems.Add(storeInfo) ;
                }


            }

            customerStores.StoreData = storeItems;
            customerStores.ColumnList = columnList;

            var successResponse = new ResultDto<CustomerStoresDto>
            {
                IsSuccess = true,
                Data = customerStores
            };

            return successResponse;

        }
        catch (Exception ex)
        {
            var storeResult = new ResultDto<CustomerStoresDto>()
            {
                ErrorMessage = StringResources.InvalidArgument,
                StatusCode = HttpStatusCode.InternalServerError,
                IsSuccess= false
            };
            return storeResult;
        }


    }


    #endregion

}
