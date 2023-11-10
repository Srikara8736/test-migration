using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Retail.Data.Entities.Common;
using Retail.Data.Entities.Customers;
using Retail.Data.Entities.Stores;
using Retail.Data.Repository;
using Retail.DTOs;
using Retail.DTOs.Cad;
using Retail.DTOs.Customers;
using Retail.DTOs.XML;
using Retail.Services.Common;
using System.ComponentModel;
using System.Net;
using System.Numerics;

namespace Retail.Services.Cad;

public class CadService : ICadService
{
    private readonly RepositoryContext _repositoryContext;
    private readonly IMapper _mapper;

    public CadService(RepositoryContext repositoryContext, IMapper mapper)
    {
        _repositoryContext = repositoryContext;
        _mapper = mapper;
    }

    public List<CustomerItem> GetAllCustomer()
    {
        var customers = _repositoryContext.Customers.Where(x => x.IsDeleted == false).ToList();

        var customerList = new List<CustomerItem>();

        foreach (var customer in customers)
        {
            var item = new CustomerItem
            {
                Id = customer.Id,
                Code = customer.Id.ToString(),
                Name = customer.Name,
                Email = customer.Email,
                Contact = customer.PhoneNumber
            };



            customerList.Add(item);
        }

        return customerList;

    }


    public List<DTOs.Cad.Store> GetStoresByCustomerNo(string customerNo)
    {
        var stores = _repositoryContext.Stores.Where(x => x.CustomerId == new Guid(customerNo)).ToList();

        var storeList = new List<DTOs.Cad.Store>();


        foreach (var store in stores)
        {
            var item = new DTOs.Cad.Store
            {
                Name = store.Name,
                StoreNumber = store.StoreNumber,
                Id = store.Id

            };

            storeList.Add(item);
        }

        return storeList;
    }

    public async Task<bool> LoadXMLData(Message message, Guid storeId)
    {

        //var storeInfo = await StoreInfoManagement(message);
        var storeInfo =await _repositoryContext.Stores.Where(x => x.Id == storeId).FirstOrDefaultAsync();
        if (storeInfo != null)
        {
             var storeData = await InsertStoreData(storeInfo.Id);

            var areaTypeGroup = await StoreAreaTypeGroupManagement(message);
            var areaType = await StoreAreaTypeManagement(message);
            var category = await StoreCategoryManagement(message);

            if(storeData != null)
            {
                var storeSpace = await StoreSpaceManagement(message, storeInfo.Id, storeData.Id);
            }
                
        }
        return true;

    }


    #region Store Info Section

    public async Task<Retail.Data.Entities.Stores.Store> StoreInfoManagement(Message message)
    {


        var cadStore = message.Info;
        if (cadStore == null)
            return null;

        cadStore.StoreName.Value = "Store 1";

        var storeItem = await _repositoryContext.Stores.Where(x => x.Name.ToLower().Trim() == cadStore.StoreName.Value.ToLower().Trim()).FirstOrDefaultAsync();

        if (storeItem != null)
            return storeItem;
        else
            return await InsertStore(cadStore);

    }


    public async Task<Retail.Data.Entities.Stores.Store> InsertStore(Info cadStore)
    {
        var storeItem = new Retail.Data.Entities.Stores.Store
        {
            //Name = cadStore.StoreName.Value
            Name = "Store 1",
            CustomerId = new Guid("0476E42C-D794-11ED-AFA1-0242AC120002"),
            StatusId = new Guid("6E9ECA58-3537-11EE-BE56-0242AC120002"),
            AddressId = new Guid("B8D888BE-9C80-4056-49D5-08DB9416203D"),
            StoreNumber = "AB101",
        };

        await _repositoryContext.Stores.AddAsync(storeItem);
        await _repositoryContext.SaveChangesAsync();

        await InsertStoreData(storeItem.Id);

        return storeItem;

    }


    public async Task<Retail.Data.Entities.Stores.Store> GetStore(string storeInfo, string storeNumber)
    {
        return await _repositoryContext.Stores.Where(x => x.Name.ToLower().Trim() == storeInfo.ToLower().Trim() && x.StoreNumber == storeNumber).FirstOrDefaultAsync();
    }

    #endregion

    #region Store Data Section

    public async Task<Retail.Data.Entities.Stores.StoreData> InsertStoreData(Guid storeId)
    {
        int versionNo = 1;
        var storeData = await _repositoryContext.StoreDatas.Where(x => x.StoreId == storeId).OrderByDescending(x => x.VersionNumber).FirstOrDefaultAsync();
        
        if (storeData != null) 
            versionNo = storeData.VersionNumber +1;
        var storeDataItem = new Retail.Data.Entities.Stores.StoreData
        {
            StoreId = storeId,
            VersionNumber = versionNo,
            CreatedOn = DateTime.UtcNow,
            StatusId = new Guid("6E9EC88C-3537-11EE-BE56-0242AC120002")

        };

        await _repositoryContext.StoreDatas.AddAsync(storeDataItem);
        await _repositoryContext.SaveChangesAsync();

        return storeDataItem;

    }

    #endregion

    #region Area Type Group Section

    public async Task<List<Retail.Data.Entities.Stores.AreaTypeGroup>> StoreAreaTypeGroupManagement(Message message)
    {

        var areaTypeGroupList = new List<Retail.Data.Entities.Stores.AreaTypeGroup>();

        var areaTypeGroups = message.Data.AreaTypeGroupList.Group;
        if (areaTypeGroups != null)
        {
            foreach (var areaTypeGroup in areaTypeGroups)
            {
                var item = await _repositoryContext.AreaTypeGroups.Where(x => x.Name.ToLower().Trim() == areaTypeGroup.ToLower().Trim()).FirstOrDefaultAsync();
                if (item != null)
                    areaTypeGroupList.Add(item);
                else
                {
                    var areaTypeGroupResponse = await InsertAreaTypeGroup(areaTypeGroup);
                    areaTypeGroupList.Add(areaTypeGroupResponse);

                }


            }

        }
        return areaTypeGroupList;


    }


    public async Task<Retail.Data.Entities.Stores.AreaTypeGroup> InsertAreaTypeGroup(string areaTypeGroup)
    {
        var areaTypeGroupItem = new Retail.Data.Entities.Stores.AreaTypeGroup
        {
            Name = areaTypeGroup
        };

        await _repositoryContext.AreaTypeGroups.AddAsync(areaTypeGroupItem);
        await _repositoryContext.SaveChangesAsync();

        return areaTypeGroupItem;

    }

    public async Task<Retail.Data.Entities.Stores.AreaTypeGroup> GetAreaTypeGroup(string areaTypeGroupName)
    {
        return await _repositoryContext.AreaTypeGroups.Where(x => x.Name.ToLower().Trim() == areaTypeGroupName.ToLower().Trim()).FirstOrDefaultAsync();
    }

    #endregion

    #region Area Type Section

    public async Task<List<Retail.Data.Entities.Stores.AreaType>> StoreAreaTypeManagement(Message message)
    {

        var areaTypeList = new List<Retail.Data.Entities.Stores.AreaType>();

        var areaTypes = message.Data.AreaTypeList.Type;
        if (areaTypes != null)
        {
            foreach (var areaType in areaTypes)
            {
                var item = await _repositoryContext.AreaTypes.Where(x => x.Name.ToLower().Trim() == areaType.ToLower().Trim()).FirstOrDefaultAsync();
                if (item != null)
                    areaTypeList.Add(item);
                else
                {
                    var areaTypeResponse = await InsertAreaType(areaType);
                    areaTypeList.Add(areaTypeResponse);

                }


            }

        }
        return areaTypeList;


    }


    public async Task<Retail.Data.Entities.Stores.AreaType> InsertAreaType(string areaTypeGroup)
    {
        var areaTypeItem = new Retail.Data.Entities.Stores.AreaType
        {
            Name = areaTypeGroup
        };

        await _repositoryContext.AreaTypes.AddAsync(areaTypeItem);
        await _repositoryContext.SaveChangesAsync();

        return areaTypeItem;

    }

    public async Task<Retail.Data.Entities.Stores.AreaType> GetAreaType(DTOs.XML.AreaType areaType)
    {
        return await _repositoryContext.AreaTypes.Where(x => x.Name.ToLower().Trim() == areaType.Name.ToLower().Trim()).FirstOrDefaultAsync();
    }

    #endregion

    #region Category Section

    public async Task<List<Retail.Data.Entities.Stores.Category>> StoreCategoryManagement(Message message)
    {

        var categoryList = new List<Retail.Data.Entities.Stores.Category>();

        var categoryItems = message.Data.CadSpaces.Category;
        if (categoryItems != null)
        {
            foreach (var category in categoryItems)
            {
                var item = await GetCategory(category);
                if (item != null)
                    categoryList.Add(item);
                else
                {
                    var areaTypeResponse = await InsertCategory(category);
                    categoryList.Add(areaTypeResponse);

                }


            }

        }
        return categoryList;


    }

    public async Task<Retail.Data.Entities.Stores.Category> GetCategory(DTOs.XML.Category category)
    {
        return await _repositoryContext.Categories.Where(x => x.Name.ToLower().Trim() == category.Name.ToLower().Trim() && x.CategoryId.ToLower().Trim() == category.Id.ToLower().Trim()).FirstOrDefaultAsync();
    }

    public async Task<Retail.Data.Entities.Stores.Category> InsertCategory(DTOs.XML.Category category)
    {
        var areaType = await GetAreaType(category.AreaType);


        var categoryItem = new Retail.Data.Entities.Stores.Category
        {
            Name = category.Name,
            CategoryId = category.Id,
            CadServiceNumber = category.Number,
            AreaTypeId = areaType.Id
        };

        await _repositoryContext.Categories.AddAsync(categoryItem);
        await _repositoryContext.SaveChangesAsync();


        return categoryItem;

    }



    public async Task<List<Retail.Data.Entities.Stores.StoreCategoryAreaTypeGroup>> InsertStoreCategoryAreaTypeGroup(DTOs.XML.AreaType areaType, Guid storeId, Guid categoryId, Guid spaceId)
    {
        var storeCatareaTypeGroups = new List<Retail.Data.Entities.Stores.StoreCategoryAreaTypeGroup>();
        foreach (var areaTypeGroup in areaType.AreaTypeGroups.AreaTypeGroup)
        {

            var areaTypeGroupItem = await GetAreaTypeGroup(areaTypeGroup);

            var spaceItem = await _repositoryContext.Spaces.Where(x => x.Id == spaceId).FirstOrDefaultAsync();

            if(areaTypeGroupItem != null && spaceItem != null)
            {
                var storeCategoryItem = new Retail.Data.Entities.Stores.StoreCategoryAreaTypeGroup
                {
                    StoreId = storeId,
                    CategoryId = categoryId,
                    AreaTypeGroupId = areaTypeGroupItem.Id,
                    SpaceId = spaceItem.Id
                };

                await _repositoryContext.StoreCategoryAreaTypeGroups.AddAsync(storeCategoryItem);
                await _repositoryContext.SaveChangesAsync();

                storeCatareaTypeGroups.Add(storeCategoryItem);
            }


        }



        return storeCatareaTypeGroups;

    }

    #endregion


    #region Space Section

    public async Task<List<Retail.Data.Entities.Stores.Space>> StoreSpaceManagement(Message message,Guid storeId, Guid storeDataId)
    {

        var spaceList = new List<Retail.Data.Entities.Stores.Space>();

        //message.Info.StoreName.Value = "Store 1";
        //message.Info.StoreNo.Value = "AB101";

        //var storeItem = await GetStore(message.Info.StoreName.Value, message.Info.StoreNo.Value);

        //var storeData = await _repositoryContext.StoreDatas.Where(x => x.StoreId == storeItem.Id).OrderByDescending(x => x.VersionNumber).FirstOrDefaultAsync();

            var categoryItems = message.Data.CadSpaces.Category;
            if (categoryItems != null)
            {
                foreach (var category in categoryItems)
                {
                    var catergoryItem = await GetCategory(category);
                    if (category.Spaces != null)
                    {

                        foreach (var spaceitem in category.Spaces.Space)
                        {
                            var item = await GetSpace(spaceitem);
                            if (item != null)
                            {



                                var storeSpaceItem = await GetStoreSpace(storeId, catergoryItem.Id, item.Id, storeDataId);
                                if (storeSpaceItem == null)
                                {

                                    var storeSpace = await InsertStoreSpace(spaceitem, storeId, storeDataId, catergoryItem.Id, item.Id);
                                    if (storeSpace != null)
                                    {
                                        _ = await InsertStoreCategoryAreaTypeGroup(category.AreaType, storeSpace.StoreId, storeSpace.CategoryId, item.Id);
                                    }
                                }

                                spaceList.Add(item);





                            }

                            else
                            {

                                if (catergoryItem != null)
                                {
                                    var spaceResponse = await InsertSpace(spaceitem, catergoryItem.Id);
                                    spaceList.Add(spaceResponse);

                                    if (spaceResponse != null)
                                    {
                                        var storeSpace = await InsertStoreSpace(spaceitem, storeId, storeDataId, catergoryItem.Id, spaceResponse.Id);

                                        if (storeSpace != null)
                                        {
                                            _ = await InsertStoreCategoryAreaTypeGroup(category.AreaType, storeSpace.StoreId, storeSpace.CategoryId, storeSpace.Id);
                                        }


                                    }

                                }


                            }



                        }

                    }



                }

            }

      

        return spaceList;


    }

    public async Task<Retail.Data.Entities.Stores.Space> GetSpace(DTOs.XML.Space space)
    {
        return await _repositoryContext.Spaces.Where(x => x.Name.ToLower().Trim() == space.Name.ToLower().Trim()).FirstOrDefaultAsync();
    }

    public async Task<Retail.Data.Entities.Stores.Space> InsertSpace(DTOs.XML.Space space, Guid categoryId)
    {

        var spaceItem = new Retail.Data.Entities.Stores.Space
        {
            Name = space.Name,
            CadServiceNumber = Int32.Parse(space.Number),
            CategoryId = categoryId

        };

        await _repositoryContext.Spaces.AddAsync(spaceItem);
        await _repositoryContext.SaveChangesAsync();

        return spaceItem;

    }


    public async Task<Retail.Data.Entities.Stores.StoreSpace> InsertStoreSpace(DTOs.XML.Space space, Guid storeId, Guid storeDataId, Guid categoryId, Guid spaceId)
    {

        var storeSpaceItem = new Retail.Data.Entities.Stores.StoreSpace
        {
            Unit = space.Unit,
            Pieces = decimal.Parse(space.Pieces),
            Area = decimal.Parse(space.Area),
            Articles = decimal.Parse(space.Articles),
            CategoryId = categoryId,
            SpaceId = spaceId,
            StoreId = storeId,
            StoreDataId = storeDataId

        };
        await _repositoryContext.StoreSpaces.AddAsync(storeSpaceItem);
        await _repositoryContext.SaveChangesAsync();


        return storeSpaceItem;

    }



    public async Task<Retail.Data.Entities.Stores.StoreSpace> GetStoreSpace(Guid storeId, Guid categoryId, Guid spaceId, Guid storeDataId)
    {

        var storeSpace = await _repositoryContext.StoreSpaces.Where(x => x.StoreId == storeId && x.CategoryId == categoryId && x.SpaceId == spaceId && x.StoreDataId == storeDataId).FirstOrDefaultAsync();

        return storeSpace;

    }



    #endregion


    #region Drawing List

    public async Task<Retail.Data.Entities.Stores.DrawingList> LoadDrawingData(Guid storeId, MessageData messageData)
    {
        var drawingListItem = new Retail.Data.Entities.Stores.DrawingList();

        drawingListItem.StoreId = storeId;

        if (messageData.Properties != null)
        {
          
            var propertyName = messageData.Properties.Where(x => x.PropertyName.ToLower() == "name").FirstOrDefault();
            if (propertyName != null)
            {
                drawingListItem.Name = propertyName.PropertyValue;
            }

            var propertyId = messageData.Properties.Where(x => x.PropertyName.ToLower() == "pno").FirstOrDefault();
            if (propertyId != null)
            {
                drawingListItem.DrawingListId = propertyId.PropertyValue;
            }

            var startDateProperty = messageData.Properties.Where(x => x.PropertyName.ToLower() == "startdate").FirstOrDefault();
            if (startDateProperty != null)
            {
                drawingListItem.StartDate = DateTime.Parse(startDateProperty.PropertyValue);
            }

            var dateProperty = messageData.Properties.Where(x => x.PropertyName.ToLower() == "date").FirstOrDefault();
            if (dateProperty != null)
            {
                drawingListItem.StartDate = DateTime.Parse(dateProperty.PropertyValue);
            }


            var revProperty = messageData.Properties.Where(x => x.PropertyName.ToLower() == "rev").FirstOrDefault();
            if (revProperty != null)
            {
                drawingListItem.Rev = revProperty.PropertyValue;
            }

            var noProperty = messageData.Properties.Where(x => x.PropertyName.ToLower() == "no").FirstOrDefault();
            //if (noProperty != null)
            //{
            //    drawingListItem.No = Int32.Parse(noProperty.PropertyValue);
            //}


            var noteProperty = messageData.Properties.Where(x => x.PropertyName.ToLower() == "note").FirstOrDefault();
            if (noteProperty != null)
            {
                drawingListItem.Note = noteProperty.PropertyValue;
            }

            var signProperty = messageData.Properties.Where(x => x.PropertyName.ToLower() == "sign").FirstOrDefault();
            if (signProperty != null)
            {
                drawingListItem.Sign = signProperty.PropertyValue;
            }

            var statusProperty = messageData.Properties.Where(x => x.PropertyName.ToString().ToLower() == "status").FirstOrDefault();

            if (statusProperty != null) 
            {
                var status = await _repositoryContext.CodeMasters.Where(x => x.Value == statusProperty.PropertyValue && x.Type =="drawing").FirstOrDefaultAsync();
                if (status != null)
                    drawingListItem.StatusId = status.Id;
                else
                {
                    var statusItem = await CreateStatus(statusProperty.PropertyValue, "drawing");
                    if(statusItem != null)
                        drawingListItem.StatusId = statusItem.Id;
                }
             }

        }



        await _repositoryContext.DrawingLists.AddAsync(drawingListItem);
        await _repositoryContext.SaveChangesAsync();


        return drawingListItem;
        
    }

    public async Task<CodeMaster> CreateStatus(string value, string type)
    {
        int order = 1;
        var lastItem = await _repositoryContext.CodeMasters.Where(x => x.Type.ToLower() == type).OrderByDescending(x => x.Order).FirstOrDefaultAsync();
        if (lastItem != null)
            order = lastItem.Order + 1;

        var statusItem = new CodeMaster()
        {
            Type = type,
            Value = value,
            Order= order

        };


        await _repositoryContext.CodeMasters.AddAsync(statusItem);
        await _repositoryContext.SaveChangesAsync();

        return statusItem;
    }


    #endregion


    public async Task<ResultDto<List<CadUploadHistoryResponseDto>>> GetCadUploadHistoryByStore(Guid storeId, CancellationToken cancellationToken = default)
    {

        if (storeId == null)
        {
            var cadResult = new ResultDto<List<CadUploadHistoryResponseDto>>
            {
                ErrorMessage = StringResources.InvalidArgument,
                StatusCode = HttpStatusCode.BadRequest
            };
            return cadResult;
        }

        var uploadHistory = await _repositoryContext.CadUploadHistories.Where(x => x.StoreId == storeId && x.Status).OrderByDescending(y => y.UploadOn) .ToListAsync();

        if (uploadHistory.Count <= 0)            
        {
            var customerResult = new ResultDto<List<CadUploadHistoryResponseDto>>
            {
                IsSuccess = true,
                ErrorMessage = StringResources.NoResultsFound,
                StatusCode = HttpStatusCode.OK
            };
            return customerResult;
        }
        var cadResponse = _mapper.Map<List<CadUploadHistoryResponseDto>>(uploadHistory);

        var response = new ResultDto<List<CadUploadHistoryResponseDto>>
        {
            IsSuccess = true,
            Data = cadResponse
        };

        return response;


    }

    #region Store Document

    public async Task<Retail.Data.Entities.FileSystem.Document> InsertDocument(string Name, string path,Guid typeGuid)
    {
        var docItem = new Retail.Data.Entities.FileSystem.Document
        {
            Name = Name,
            Path = path,          
            StatusId = typeGuid

        };

        await _repositoryContext.Documents.AddAsync(docItem);
        await _repositoryContext.SaveChangesAsync();

        return docItem;

    }

    public async Task<Retail.Data.Entities.Stores.StoreDocument> InsertStoreDocument(Guid storeId, Guid documentId)
    {
        var docItem = new Retail.Data.Entities.Stores.StoreDocument
        {
            DocumentId = documentId,
            StoreId= storeId,
            UploadedOn = DateTime.UtcNow,

        };

        await _repositoryContext.StoreDocuments.AddAsync(docItem);
        await _repositoryContext.SaveChangesAsync();

        return docItem;

    }


    public async Task<Retail.Data.Entities.Cad.CadUploadHistory> InsertCadUploadHistory(Guid storeId, string fileName)
    {
        var docItem = new Retail.Data.Entities.Cad.CadUploadHistory
        {   
            Name = fileName,
            UploadId ="fileUpload",
            StoreId = storeId,
            Status = true,
            UploadOn = DateTime.UtcNow,
            CreatedOn = DateTime.UtcNow

        };

        await _repositoryContext.CadUploadHistories.AddAsync(docItem);
        await _repositoryContext.SaveChangesAsync();

        return docItem;

    }


    #endregion

}
