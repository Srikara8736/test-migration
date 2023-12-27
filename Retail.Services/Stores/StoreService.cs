using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Retail.Data.Entities.Common;
using Retail.Data.Entities.Customers;
using Retail.Data.Entities.FileSystem;
using Retail.Data.Entities.Stores;
using Retail.Data.Entities.UserAccount;
using Retail.Data.Repository;
using Retail.DTOs;
using Retail.DTOs.Customers;
using Retail.DTOs.Stores;
using Retail.DTOs.XML;
using Retail.Services.Common;
using SixLabors.ImageSharp.ColorSpaces;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.AccessControl;


namespace Retail.Services.Stores;

public class StoreService : IStoreService
{
    #region Fields

    private readonly RepositoryContext _repositoryContext;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;


    #endregion

    #region Ctor
    public StoreService(RepositoryContext repositoryContext, IMapper mapper, IConfiguration configuration, IWebHostEnvironment environment)
    {
        _repositoryContext = repositoryContext;
        _mapper = mapper;
        _configuration = configuration;
        _environment = environment;
    }
    #endregion

    #region Utilities


    public async Task<Address?> getAddressById(Guid addressId)
    { 
        return await _repositoryContext.Addresses.Where(x => x.Id == addressId).AsNoTracking().FirstOrDefaultAsync(); 
    }

    public async Task<Customer?> getCustomerById(Guid customerId)
    {
        return await _repositoryContext.Customers.Where(x => x.Id == customerId).AsNoTracking().FirstOrDefaultAsync();
    }

    public async Task<CodeMaster?> getStatusById(Guid statusId)
    {
        return await _repositoryContext.CodeMasters.Where(x => x.Id == statusId).AsNoTracking().FirstOrDefaultAsync();
    }

    public async Task<List<StoreImage>> GetStoreImagesByStoreId(Guid storeId)
    {
        if (storeId == null)
            return null;

        return await _repositoryContext.StoreImages.Where(x => x.StoreId == storeId).AsNoTracking().ToListAsync();
    }


    public async Task<Data.Entities.FileSystem.Image> GetImageById(Guid id, CancellationToken ct = default)
    {
        if (id == null)
            return null;

        return await _repositoryContext.Images.FirstOrDefaultAsync(x => x.Id == id, cancellationToken: ct);
    }

    public async Task<Data.Entities.FileSystem.Image> InsertImage(Data.Entities.FileSystem.Image image)
    {
        if (image == null)
            return null;
         await _repositoryContext.Images.AddAsync(image);
        await _repositoryContext.SaveChangesAsync();
        return image;

    }



    public async Task<bool> DeleteImage(Guid ImageId)
    {
        var ImageItem = await _repositoryContext.Images.FirstOrDefaultAsync(x => x.Id == ImageId);

        if (ImageItem == null)
            return false;

        _repositoryContext.Images.Remove(ImageItem);
        await _repositoryContext.SaveChangesAsync();
        return true;

    }

    public async Task<StoreImage> InsertStoreImage(StoreImage image)
    {
        if (image == null)
            return null;
        await _repositoryContext.StoreImages.AddAsync(image);
        await _repositoryContext.SaveChangesAsync();
        return image;


    }


    public async Task<bool> UpdateStoreActivity(Guid storeId)
    {
        if (storeId == null)
            return false;

       var store = await _repositoryContext.Stores.FirstOrDefaultAsync(x => x.Id == storeId);

        store.LastActivityDate = DateTime.UtcNow;
        await _repositoryContext.SaveChangesAsync();

        return true;


    }


    public async Task<CustomerImage> InsertCustomerImage(CustomerImage image)
    {
        if (image == null)
            return null;
        await _repositoryContext.CustomerImages.AddAsync(image);
        await _repositoryContext.SaveChangesAsync();
        return image;


    }


    /// <summary>
    /// Delete Customer Image
    /// </summary>
    /// <param name="addressDto">Address</param>
    /// <param name="ct">Cancellation Charages</param>
    /// <returns>Delete Customer Image</returns>
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


    /// <summary>
    /// Gets all Stores
    /// </summary>
    /// <param name="customerId">customerId</param>
    /// <param name="ct">cancellation token</param>
    /// <returns>Store List By customer</returns>
    public async Task<List<StoreResponseDto>> GetStoresByCustomerId(Guid customerId, CancellationToken ct = default)
    {

       var stores = await _repositoryContext.Stores.Where(x => x.CustomerId == customerId).OrderByDescending(t => t.LastActivityDate).ToListAsync();

    
        if (stores == null)
        {
            return null;
        }

        var storeResponse = _mapper.Map<List<StoreResponseDto>>(stores);
        var path = _configuration["AssetLocations:StoreImages"];


       

        foreach (var store in storeResponse)
        {
            var storeData = await _repositoryContext.StoreDatas.Where(x => x.StoreId == store.Id).ToListAsync();
            if(storeData != null)
            {
                foreach(var item in storeData)
                {
                    var storeDataVersion = new StoreDataVersion()
                    {
                        Id = item.Id,
                        Version = "Version "+ item.VersionNumber

                    };
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

            var uploadHistory = await _repositoryContext.CadUploadHistories.Where(x => x.StoreId == store.Id && x.Status).OrderByDescending(y => y.UploadOn).FirstOrDefaultAsync();

            if (uploadHistory != null)
            {
                var cadResponse = _mapper.Map<DTOs.Cad.CadUploadHistoryResponseDto>(uploadHistory);
                store.cadUploadHistory = cadResponse;
            }


            store.PdfLink = await PdfFileUrlByStoreId(store.Id);

        }


       
        return storeResponse;

    }


    public async Task<string> PdfFileUrlByStoreId(Guid storeId)
    {
        var path = string.Empty;


        var result = from at in _repositoryContext.StoreDocuments
                     join cat in _repositoryContext.Documents on at.DocumentId equals cat.Id
                     where cat.StatusId == Guid.Parse("8AE98FB5-16ED-429E-AF96-83B6CAEC15A5") && at.StoreId == storeId
                     orderby at.UploadedOn descending
                     select cat.Path;

        path = await result.FirstOrDefaultAsync();


        //var storeDocument = await _repositoryContext.StoreDocuments.Where(x => x.StoreId == storeId).Select(x=>x .DocumentId).ToListAsync();

        //if(storeDocument != null)
        //{
        //    var document = await _repositoryContext.Documents.Where(x =>  storeDocument.Contains(x.Id)  && x.StatusId == Guid.Parse("8AE98FB5-16ED-429E-AF96-83B6CAEC15A5")).FirstOrDefaultAsync();

        //    if (document != null)
        //        path = document.Path;
        //}
        return path;
    }

    /// <summary>
    /// gets all Stores
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

        var storeResponse = _mapper.Map<PagedList<StoreResponseDto>>(stores);

        var path = _configuration["AssetLocations:StoreImages"];


        foreach (var store in storeResponse)
        {
            var storeData = await _repositoryContext.StoreDatas.Where(x => x.StoreId == store.Id).ToListAsync();
            if (storeData != null)
            {
                foreach (var item in storeData)
                {
                    var storeDataVersion = new StoreDataVersion()
                    {
                        Id = item.Id,
                        Version = "Version " + item.VersionNumber

                    };
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

            var uploadHistory = await _repositoryContext.CadUploadHistories.Where(x => x.StoreId == store.Id && x.Status).OrderByDescending(y => y.UploadOn).FirstOrDefaultAsync();

            if (uploadHistory != null)
            {
                var cadResponse = _mapper.Map<DTOs.Cad.CadUploadHistoryResponseDto>(uploadHistory);
                store.cadUploadHistory = cadResponse;
            }
            store.PdfLink = await PdfFileUrlByStoreId(store.Id);

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
    /// gets all Grid Data
    /// </summary>
    /// <param name="StoreId">Store Identifier</param>
    /// <param name="ct">cancellation token</param>
    /// <returns>Store Grid Data</returns>
    public async Task<ResultDto<List<ChartGridDto>>> GetGridData(Guid StoreId, CancellationToken ct = default)
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

        var storeData = await _repositoryContext.StoreDatas.Where(x => x.StoreId == StoreId).OrderByDescending(x => x.VersionNumber).FirstOrDefaultAsync();
        if (storeData == null)
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
                         where stsp.StoreId == StoreId && stsp.StoreDataId == storeData.Id
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
                             SpacePieces = stsp.Pieces

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

            foreach(var areatype in areaTypeGrid)
            {
                foreach(var category in areatype.Categories)
                {
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
    /// gets all Chart Data
    /// </summary>
    /// <param name="StoreId">Store Identifier</param>
    /// <param name="ct">cancellation token</param>
    /// <returns>Store Chart Data</returns>
    public async Task<ResultDto<List<ChartGraphDto>>> GetChartData(Guid StoreId, CancellationToken ct = default)
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

        var storeData = await _repositoryContext.StoreDatas.Where(x => x.StoreId == StoreId).OrderByDescending(x => x.VersionNumber).FirstOrDefaultAsync();
        if (storeData == null)
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
                         where stsp.StoreId == StoreId && stsp.StoreDataId == storeData.Id
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
            areachartData.ChartCategory = "Space";
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
                chartData.ChartCategory = "Space";
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
                    spaceData.ChartCategory = "Space";
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



                chartItems.Add(chartData);
            }




            //Chart Serice & various


            var mainAreaItems = areaTypeGrid.Where(y => y.AreaType != "SalesArea");

           

            foreach (var mainItem in mainAreaItems)
            {
              

                var chartData = new ChartGraphDto();
                chartData.ChartTitle = mainItem.AreaType;
                chartData.ChartCategory = "Space";
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


    public async Task<ResultDto<List<StoreChartGraphDto>>> GetStoreChartData(Guid StoreId, CancellationToken ct = default)
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

        var storeData = await _repositoryContext.StoreDatas.Where(x => x.StoreId == StoreId).OrderByDescending(x => x.VersionNumber).FirstOrDefaultAsync();
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
                         where stsp.StoreId == StoreId && stsp.StoreDataId == storeData.Id
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
    /// gets all Drawing Grid Data
    /// </summary>
    /// <param name="StoreId">Store Identifier</param>
    /// <param name="ct">cancellation token</param>
    /// <returns>Store Grid Data</returns>
    public async Task<ResultDto<List<DrawingListResponseDto>>> GetDrawingGridData(Guid StoreId, CancellationToken ct = default)
    {

        var storeDrawingList = await _repositoryContext.DrawingLists.Where(x => x.StoreId == StoreId).ToListAsync();

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


        foreach (var store in storeDrawingResponse)
        {
            if (store.StatusId != null)
            {

                var result = _repositoryContext.CodeMasters.Where(x => x.Id == store.StatusId).FirstOrDefault();
                if (result != null)
                    store.StoreStatus = result.Value;
            }

        }

        var response = new ResultDto<List<DrawingListResponseDto>>()
        {
            IsSuccess = true,
            Data = storeDrawingResponse
        };
        return response;

    }



    /// <summary>
    /// gets all Order Grid Data
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
    /// Gets all General List Grid Data
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
    /// Inserts Store 
    /// </summary>
    /// <param name="storeDto">store</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns></returns>
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
    /// Updates Store
    /// </summary>
    /// <param name="id">Store Id</param>
    /// <param name="storeDto">Store</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns></returns>
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
    /// Delete Role
    /// </summary>
    /// <param name="id">Store Id</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns></returns>
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
    /// <returns>Store List with Pagination</returns>
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


        var updateActivityResult = await UpdateStoreActivity(storeId);

        var store = _mapper.Map<StoreResponseDto>(storeItem);
               

        var path = _configuration["AssetLocations:StoreImages"];


        var storeData = await _repositoryContext.StoreDatas.Where(x => x.StoreId == store.Id).ToListAsync();
        if (storeData != null)
        {
            foreach (var item in storeData)
            {
                var storeDataVersion = new StoreDataVersion()
                {
                    Id = item.Id,
                    Version = "Version " + item.VersionNumber

                };
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

        var uploadHistory = await _repositoryContext.CadUploadHistories.Where(x => x.StoreId == storeId && x.Status).OrderByDescending(y => y.UploadOn).FirstOrDefaultAsync();

        if(uploadHistory != null)
        {
            var cadResponse = _mapper.Map<DTOs.Cad.CadUploadHistoryResponseDto>(uploadHistory);
            store.cadUploadHistory = cadResponse;
        }

        store.PdfLink = await PdfFileUrlByStoreId(store.Id);

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
    /// <returns></returns>
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



    public async Task<ResultDto<List<ComparisionChartGraphDto>>> CompareStoreVersionData(Guid FirstStoreId, Guid FirstVersionId, Guid SecondStoreId, Guid SecondVersionId, CancellationToken ct = default)
    {



        var store = await _repositoryContext.Stores.FirstOrDefaultAsync(x => x.Id == FirstStoreId, ct);
        if (store == null)
        {
            var storeResult = new ResultDto<List<ComparisionChartGraphDto>>()
            {
                ErrorMessage = StringResources.NoResultsFound,
                StatusCode = HttpStatusCode.OK,
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
                StatusCode = HttpStatusCode.OK,
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
        
            var version1 = await GetChartDataItem(FirstStoreId, storeDataV1.Id);

            var version2 = await GetChartDataItem(SecondStoreId, storeDataV2.Id);

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
                    if (itemVersion2.chartItems.Count > 0 && item.chartItems.Count > 0)
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
                ErrorMessage = StringResources.InvalidArgument,
                StatusCode = HttpStatusCode.InternalServerError
            };
            return storeResult;
        }


    }


    public async Task<List<ChartGraphDto>> GetChartDataItem(Guid storeId, Guid storeDataId)
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
        areachartData.ChartCategory = "Space";
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
            chartData.ChartCategory = "Space";
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
                spaceData.ChartCategory = "Space";
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
            chartData.ChartCategory = "Space";
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


}
