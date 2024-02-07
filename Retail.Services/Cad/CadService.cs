using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Retail.Data.Entities.Common;
using Retail.Data.Entities.Customers;
using Retail.Data.Entities.Stores;
using Retail.Data.Repository;
using Retail.DTOs;
using Retail.DTOs.Cad;
using Retail.DTOs.Customers;
using Retail.DTOs.Master;
using Retail.DTOs.XML;
using Retail.Services.Common;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Net;
using System.Numerics;
using System.Security.AccessControl;

namespace Retail.Services.Cad;

public class CadService : ICadService
{
    #region Fields

    private readonly RepositoryContext _repositoryContext;
    private readonly IMapper _mapper;

    #endregion


    #region Ctor
    public CadService(RepositoryContext repositoryContext, IMapper mapper)
    {
        _repositoryContext = repositoryContext;
        _mapper = mapper;
    }

    #endregion



    /// <summary>
    /// Get All Customers
    /// </summary>
    /// <returns> All Customer Informations</returns>
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

    /// <summary>
    /// Get Stores By Customer Number
    /// </summary>
    /// <param name="customerNo">customerNo</param>
    /// <returns> Store Information</returns>
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

    /// <summary>
    /// Get Status By Name
    /// </summary>
    /// <param name="name">Status Name</param>
    /// <returns> Status Information</returns>
    public async Task<CodeMaster?> GetCodeMasterByName(string name)
    {
        return await _repositoryContext.CodeMasters.FirstOrDefaultAsync(x => x.Type == "CadType" && x.Value == name);
    }


    /// <summary>
    /// Load Space / Department List Zip XML data
    /// </summary>
    /// <param name="message">Deserialized XML Model</param>
    /// <param name="storeId">Store Identifier</param>
    /// <param name="type">Store Type</param>
    /// <returns> True / False of Loading XMl Data Status</returns>
    public async Task<(bool status,Guid? storeDataId)> LoadXMLData(Message message, Guid storeId, string type, Guid UploadHistoryId)
    {

        //var storeInfo = await StoreInfoManagement(message);
        var storeInfo = await _repositoryContext.Stores.FirstOrDefaultAsync(x => x.Id == storeId);
        var cadType = await GetCodeMasterByName(type);
        if (storeInfo != null && cadType != null)
        {
            var storeData = await InsertStoreData(storeInfo.Id,cadType.Id);

            var areaTypeGroup = await StoreAreaTypeGroupManagement(message);
            var areaType = await StoreAreaTypeManagement(message);
            var category = await StoreCategoryManagement(message);

            if (storeData == null)
                return (false, null);

            var storeSpace = await StoreSpaceManagement(message, storeInfo.Id, storeData.Id, cadType.Id, UploadHistoryId);
            return (true, storeData.Id);


        }
        return (false,null);

    }


    #region Store Info Section


    /// <summary>
    /// Create Store 
    /// </summary>
    /// <param name="message">Deserialized XML Model</param>
    /// <returns> Store Information</returns>
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

    /// <summary>
    /// Insert Store model
    /// </summary>
    /// <param name="Info">Store Info Model</param>
    /// <returns> Store Information</returns>
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

        //await InsertStoreData(storeItem.Id);

        return storeItem;

    }


    /// <summary>
    /// Get Store Info By Name & Number
    /// </summary>
    /// <param name="storeInfo">Store Info</param>
    /// <param name="storeNumber">Store Number</param>
    /// <returns> Store Information</returns>
    public async Task<Retail.Data.Entities.Stores.Store?> GetStore(string storeInfo, string storeNumber)
    {
        return await _repositoryContext.Stores.Where(x => x.Name.ToLower().Trim() == storeInfo.ToLower().Trim() && x.StoreNumber == storeNumber).FirstOrDefaultAsync();
    }

    #endregion

    #region Store Data Section

    /// <summary>
    /// Inser Store Data with version Number
    /// </summary>
    /// <param name="storeId">Store Identifier</param>
    /// <param name="typeId">XML Type</param>
    /// <returns> Store Data Information</returns>
    public async Task<Retail.Data.Entities.Stores.StoreData> InsertStoreData(Guid storeId, Guid typeId)
    {
        int versionNo = 1;
        var storeData = await _repositoryContext.StoreDatas.Where(x => x.StoreId == storeId && x.CadFileTypeId == typeId).OrderByDescending(x => x.VersionNumber).FirstOrDefaultAsync();

        if (storeData != null)
            versionNo = storeData.VersionNumber + 1;

        var storeDataItem = new Retail.Data.Entities.Stores.StoreData
        {
            StoreId = storeId,
            VersionNumber = versionNo,
            CreatedOn = DateTime.UtcNow,
            StatusId = new Guid("6E9EC88C-3537-11EE-BE56-0242AC120002"),
            CadFileTypeId = typeId,
            VersionName = "Version"

        };

        await _repositoryContext.StoreDatas.AddAsync(storeDataItem);
        await _repositoryContext.SaveChangesAsync();

        return storeDataItem;

    }

    #endregion

    #region Area Type Group Section

    /// <summary>
    /// Insert / Get Area Group with version Number
    /// </summary>
    /// <param name="message">Deserialize XMl Model</param>
    /// <returns> Area Type Group Items</returns>
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


    /// <summary>
    /// Insert Area Group with version Number
    /// </summary>
    /// <param name="areaTypeGroup">Area Type Group Name</param>
    /// <returns> Area Type Group </returns>
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


    /// <summary>
    /// Get Area Group By Name
    /// </summary>
    /// <param name="areaTypeGroupName">Area Type Group Name</param>
    /// <returns> Area Type Group </returns>
    public async Task<Retail.Data.Entities.Stores.AreaTypeGroup?> GetAreaTypeGroup(string areaTypeGroupName)
    {
        return await _repositoryContext.AreaTypeGroups.Where(x => x.Name.ToLower().Trim() == areaTypeGroupName.ToLower().Trim()).FirstOrDefaultAsync();
    }

    #endregion

    #region Area Type Section

    /// <summary>
    /// Insert / Get Area Type with version Number
    /// </summary>
    /// <param name="message">Deserialize XMl Model</param>
    /// <returns> Area Type Items</returns>
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

    /// <summary>
    /// Insert Area Type with version Number
    /// </summary>
    /// <param name="areaTypeGroup">Area Type Name</param>
    /// <returns> Area Type</returns>
    public async Task<Retail.Data.Entities.Stores.AreaType?> InsertAreaType(string areaTypeGroup)
    {
        if (areaTypeGroup == "")
            return null;

        var areaTypeItem = new Retail.Data.Entities.Stores.AreaType
        {
            Name = areaTypeGroup
        };

        await _repositoryContext.AreaTypes.AddAsync(areaTypeItem);
        await _repositoryContext.SaveChangesAsync();

        return areaTypeItem;

    }


    /// <summary>
    /// Get Area Type By Name
    /// </summary>
    /// <param name="areaType">Area Type</param>
    /// <returns> Area Type</returns>
    public async Task<Retail.Data.Entities.Stores.AreaType?> GetAreaType(DTOs.XML.AreaType areaType)
    {
        return await _repositoryContext.AreaTypes.Where(x => x.Name.ToLower().Trim() == areaType.Name.ToLower().Trim()).FirstOrDefaultAsync();
    }

    #endregion

    #region Category Section

    /// <summary>
    ///  Get List of Category
    /// </summary>
    /// <param name="message">Deserialize Category Model</param>
    /// <returns>Category Information</returns>
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


    /// <summary>
    /// Get Category By Name
    /// </summary>
    /// <param name="category">Category Model</param>
    /// <returns> Category</returns>
    public async Task<Retail.Data.Entities.Stores.Category?> GetCategory(DTOs.XML.Category category)
    {
        return await _repositoryContext.Categories.Where(x => x.Name.ToLower().Trim() == category.Name.ToLower().Trim() && x.CategoryId.ToLower().Trim() == category.Id.ToLower().Trim()).FirstOrDefaultAsync();
    }


    /// <summary>
    /// Save Category Item
    /// </summary>
    /// <param name="category">Category Model</param>
    /// <returns>Category Information</returns>
    public async Task<Retail.Data.Entities.Stores.Category> InserCategoryItem(Retail.Data.Entities.Stores.Category category)
    {

        await _repositoryContext.Categories.AddAsync(category);
        await _repositoryContext.SaveChangesAsync();


        return category;

    }

    /// <summary>
    /// Insert Category Item
    /// </summary>
    /// <param name="category">Category Model</param>
    /// <returns>Category Information</returns>
    public async Task<Retail.Data.Entities.Stores.Category> InsertCategory(DTOs.XML.Category category)
    {
        var areaType = await GetAreaType(category.AreaType);
        var category_Item = new Retail.Data.Entities.Stores.Category();

        if (areaType != null)
        {
            var guid = areaType.Id;
            var categoryItem = new Retail.Data.Entities.Stores.Category
            {
                Name = category.Name,
                CategoryId = category.Id,
                CadServiceNumber = category.Number,
                AreaTypeId = guid,

            };
            await _repositoryContext.Categories.AddAsync(categoryItem);
            await _repositoryContext.SaveChangesAsync();


            category_Item = categoryItem;

        }

        else
        {
            var areaTypeItem = await InsertAreaType(category.AreaType.Name);
            if (areaTypeItem != null)
            {
                var categoryItem = new Retail.Data.Entities.Stores.Category
                {
                    Name = category.Name,
                    CategoryId = category.Id,
                    CadServiceNumber = category.Number,
                    AreaTypeId = areaTypeItem.Id,

                };

                await _repositoryContext.Categories.AddAsync(categoryItem);
                await _repositoryContext.SaveChangesAsync();


                category_Item = categoryItem;

            }
        }


        return category_Item;
    }


    /// <summary>
    /// Insert Store Category Area Type Group
    /// </summary>
    /// <param name="areaType">Store Category Area Type Group Model</param>
    /// <param name="storeId">Store Identifier</param>
    /// <param name="categoryId">Category Identifier</param>
    /// <param name="spaceId">Space Identifier</param>
    /// <returns>Category Information</returns>
    public async Task<List<Retail.Data.Entities.Stores.StoreCategoryAreaTypeGroup>> InsertStoreCategoryAreaTypeGroup(DTOs.XML.AreaType areaType, Guid storeId, Guid categoryId, Guid spaceId)
    {
        var storeCatareaTypeGroups = new List<Retail.Data.Entities.Stores.StoreCategoryAreaTypeGroup>();
        foreach (var areaTypeGroup in areaType.AreaTypeGroups.AreaTypeGroup)
        {

            var areaTypeGroupItem = await GetAreaTypeGroup(areaTypeGroup);

            var spaceItem = await _repositoryContext.Spaces.Where(x => x.Id == spaceId).FirstOrDefaultAsync();

            if (areaTypeGroupItem != null && spaceItem != null)
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


    public async Task<CadStoreCategory> AddCadStoreCategory(Guid storeId, Guid storeDataId, Guid cadTypeId, Guid uploadHistoryId, Guid categoryId)
    {
        var cadStoreCategory = new CadStoreCategory()
        {
            CadTypeId= cadTypeId,
            CreatedDate= DateTime.UtcNow,
            StoreDataId = storeDataId,
            StoreId = storeId,
            UploadHistoryId = uploadHistoryId,
            CategoryId = categoryId
        };

        await _repositoryContext.CadStoreCategories.AddAsync(cadStoreCategory);
        await _repositoryContext.SaveChangesAsync();

        return cadStoreCategory;
    }


    public async Task<CadStoreSpace> AddCadStoreStore(Guid storeId, Guid storeDataId, Guid cadTypeId, Guid uploadHistoryId, Guid spaceId)
    {
        var cadStoreSpace = new CadStoreSpace()
        {
            CadTypeId = cadTypeId,
            CreatedDate = DateTime.UtcNow,
            StoreDataId = storeDataId,
            StoreId = storeId,
            UploadHistoryId = uploadHistoryId,
            SpaceId = spaceId
        };

        await _repositoryContext.CadStoreSpaces.AddAsync(cadStoreSpace);
        await _repositoryContext.SaveChangesAsync();

        return cadStoreSpace;
    }



    /// <summary>
    /// Manage Space Section
    /// </summary>
    /// <param name="message">Deserailze XMl Model</param>
    /// <param name="storeId">Store Identifier</param>
    /// <param name="storeDataId">Store Data Identifier</param>
    /// <param name="cadTypeId">Cad Type Identifier</param>
    /// <returns>List of space Information</returns>
    public async Task<List<Retail.Data.Entities.Stores.Space>> StoreSpaceManagement(Message message, Guid storeId, Guid storeDataId,Guid cadTypeId, Guid uploadHistoryId)
    {

        var spaceList = new List<Retail.Data.Entities.Stores.Space>();
        
        var categoryItems = message.Data.CadSpaces.Category;
        if (categoryItems != null)
        {
            foreach (var category in categoryItems)
            {
                var catergoryItem = await GetCategory(category);

                if (catergoryItem != null)
                    _ = await AddCadStoreCategory(storeId, storeDataId, cadTypeId, uploadHistoryId, catergoryItem.Id);



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

                                var storeSpace = await InsertStoreSpace(spaceitem, storeId, storeDataId, catergoryItem.Id, item.Id, cadTypeId);
                                if (storeSpace != null)
                                {
                                    var spaceDto = await AddCadStoreStore(storeId, storeDataId, cadTypeId, uploadHistoryId, item.Id);
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
                                    var storeSpace = await InsertStoreSpace(spaceitem, storeId, storeDataId, catergoryItem.Id, spaceResponse.Id, cadTypeId);

                                    if (storeSpace != null)
                                    {
                                        var spaceDto = await AddCadStoreStore(storeId, storeDataId, cadTypeId, uploadHistoryId, spaceResponse.Id);

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


    /// <summary>
    /// Get Space By Name
    /// </summary>
    /// <param name="space">Space Model Name</param>
    /// <returns>space Information</returns>
    public async Task<Retail.Data.Entities.Stores.Space?> GetSpace(DTOs.XML.Space space)
    {
        return await _repositoryContext.Spaces.Where(x => x.Name.ToLower().Trim() == space.Name.ToLower().Trim()).FirstOrDefaultAsync();
    }

    /// <summary>
    /// Insert Space Item
    /// </summary>
    /// <param name="space">Space Model</param>
    /// <param name="categoryId">Category Identifier</param>
    /// <returns>space Information</returns>
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

    /// <summary>
    /// Insert Store Space Item
    /// </summary>
    /// <param name="space">Space Model</param>
    /// <param name="storeId">Store Identifier</param>
    /// <param name="storeDataId">Store Data Identifier</param>
    /// <param name="categoryId">Category Identifier</param>
    /// <param name="spaceId">Space Identifier</param>
    /// <param name="cadTypeId">Cad type Identifier</param>
    /// <returns>Store space Information</returns>
    public async Task<Retail.Data.Entities.Stores.StoreSpace> InsertStoreSpace(DTOs.XML.Space space, Guid storeId, Guid storeDataId, Guid categoryId, Guid spaceId,Guid cadTypeId)
    {

        var storeSpaceItem = new Retail.Data.Entities.Stores.StoreSpace
        {
            Unit = space.Unit,
            Pieces = decimal.Parse(space.Pieces, CultureInfo.InvariantCulture),
            Area = decimal.Parse(space.Area, CultureInfo.InvariantCulture),
            Articles = decimal.Parse(space.Articles, CultureInfo.InvariantCulture),
            CategoryId = categoryId,
            SpaceId = spaceId,
            StoreId = storeId,
            StoreDataId = storeDataId,
            CadFileTypeId = cadTypeId

        };
        await _repositoryContext.StoreSpaces.AddAsync(storeSpaceItem);
        await _repositoryContext.SaveChangesAsync();


        return storeSpaceItem;

    }

    /// <summary>
    /// Get Store Space Item
    /// </summary>
    /// <param name="storeId">Store Identifier</param>
    /// <param name="storeDataId">Store Data Identifier</param>
    /// <param name="categoryId">Category Identifier</param>
    /// <param name="spaceId">Space Identifier</param>
    /// <returns>space Information</returns>
    public async Task<Retail.Data.Entities.Stores.StoreSpace?> GetStoreSpace(Guid storeId, Guid categoryId, Guid spaceId, Guid storeDataId)
    {

        var storeSpace = await _repositoryContext.StoreSpaces.Where(x => x.StoreId == storeId && x.CategoryId == categoryId && x.SpaceId == spaceId && x.StoreDataId == storeDataId).FirstOrDefaultAsync();

        return storeSpace;

    }



    #endregion


    #region Drawing List

    /// <summary>
    /// Load Drawing XMl Data
    /// </summary>
    /// <param name="storeId">Store Identifier</param>
    /// <param name="messageData">Store Data Identifier</param>
    /// <returns>Drawing Type Information</returns>
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
                var status = await _repositoryContext.CodeMasters.Where(x => x.Value == statusProperty.PropertyValue && x.Type == "drawing").FirstOrDefaultAsync();
                if (status != null)
                    drawingListItem.StatusId = status.Id;
                else
                {
                    var statusItem = await CreateStatus(statusProperty.PropertyValue, "drawing");
                    if (statusItem != null)
                        drawingListItem.StatusId = statusItem.Id;
                }
            }

        }



        await _repositoryContext.DrawingLists.AddAsync(drawingListItem);
        await _repositoryContext.SaveChangesAsync();


        return drawingListItem;

    }


    /// <summary>
    /// Create Status
    /// </summary>
    /// <param name="value">Status Name</param>
    /// <param name="type">Status Type</param>
    /// <returns>Status Information</returns>
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
            Order = order

        };


        await _repositoryContext.CodeMasters.AddAsync(statusItem);
        await _repositoryContext.SaveChangesAsync();

        return statusItem;
    }


    #endregion


    /// <summary>
    /// Cad Upload History By Store
    /// </summary>
    /// <param name="storeId">Store Identifier</param>
    /// <param name="cancellationToken">Cancellation Token</param>
    /// <returns>List of Upload History Information</returns>
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

        var uploadHistory = await _repositoryContext.CadUploadHistories.Where(x => x.StoreId == storeId && x.Status).OrderByDescending(y => y.UploadOn).ToListAsync();

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


    /// <summary>
    /// Insert Document
    /// </summary>
    /// <param name="Name">Document Name</param>
    /// <param name="path">Document path</param>
    /// <param name="typeGuid">Document Type</param>
    /// <returns>Document Information</returns>
    public async Task<Retail.Data.Entities.FileSystem.Document> InsertDocument(string Name, string path, Guid typeGuid)
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


    /// <summary>
    /// Insert Store Document
    /// </summary>
    /// <param name="storeId">Store Document Identifier</param>
    /// <param name="documentId">Document Indetifier</param>
    /// <returns>Store Document Information</returns>
    public async Task<Retail.Data.Entities.Stores.StoreDocument> InsertStoreDocument(Guid storeId, Guid documentId,Guid? storeDataId)
    {
        var docItem = new Retail.Data.Entities.Stores.StoreDocument
        {
            DocumentId = documentId,
            StoreId = storeId,
            UploadedOn = DateTime.UtcNow,
            StoreDataId = storeDataId

        };

        await _repositoryContext.StoreDocuments.AddAsync(docItem);
        await _repositoryContext.SaveChangesAsync();

        return docItem;

    }


    /// <summary>
    /// Insert Cad Upload History for Store
    /// </summary>
    /// <param name="storeId">Store Identifier</param>
    /// <param name="fileName">File Name</param>
    /// <returns> Upload History Information</returns>
    public async Task<Retail.Data.Entities.Cad.CadUploadHistory> InsertCadUploadHistory(Guid storeId, string fileName)
    {
        var docItem = new Retail.Data.Entities.Cad.CadUploadHistory
        {
            Name = fileName,
            UploadId = "fileUpload",
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


    #region Order List


    /// <summary>
    /// Load Package Data From XMl - Order
    /// </summary>
    /// <param name="storeId">Store Identifier</param>
    /// <param name="orderMandatory">Mandatory Attributes of Order data</param>
    /// <returns>Order Package Information</returns>
    public async Task<Retail.Data.Entities.Stores.PackageData> LoadPackageData(Guid storeId, OrderMandatoryPoperties orderMandatory)
    {
        try
        {
            var packageData = new Retail.Data.Entities.Stores.PackageData();
            packageData.StoreId = storeId;
            packageData.StatusId = Guid.Parse("2DB5BA54-04C3-46EC-A47D-597D67108EAF");


            if (orderMandatory.CadPackageName != null)
                packageData.PackageName = orderMandatory.CadPackageName.Value;


            if (orderMandatory.Createdby != null)
                packageData.CreatedBy = orderMandatory.Createdby.Value;

            if (orderMandatory.ArchicadFile != null)
                packageData.FileName = orderMandatory.ArchicadFile.Value;

            if (orderMandatory.CreationDate != null)
                packageData.CreatedDate = orderMandatory.CreationDate.Value != null ? DateTime.Parse(orderMandatory.CreationDate.Value) : null;


            await _repositoryContext.PackageDatas.AddAsync(packageData);
            await _repositoryContext.SaveChangesAsync();

            return packageData;
        }
        catch(Exception ex)
        {
            return null;
        }




    }


    /// <summary>
    /// Load Order Type XML Data
    /// </summary>
    /// <param name="storeId">Store Identifier</param>
    /// <param name="orderMessage">Deserialize Model</param>
    /// <returns>True / False status of XML load</returns>
    public async Task<bool> LoadOrderListData(Guid storeId, OrderMessageBlock orderMessage)
    {
        try
        {
            var packgeDate = await LoadPackageData(storeId, orderMessage.MandatoryPoperties);
            if (packgeDate == null)
                return false;

            var messageDataItems = orderMessage.Messages.MessageData;

            if (messageDataItems.Count > 0)
            {
                foreach (var messageData in messageDataItems)
                {
                    var orderListItem = new Retail.Data.Entities.Stores.OrderList
                    {
                        StoreId = storeId,
                        PackageDataId = packgeDate.Id
                    };



                    var articleNumberData = messageData.Properties.Where(x => x.PropertyName.ToLower() == "article number").FirstOrDefault();
                    if (articleNumberData != null)
                    {
                        orderListItem.ArticleNumber = articleNumberData.PropertyValue;
                    }

                    var quantityData = messageData.Properties.Where(x => x.PropertyName.ToLower() == "quantity").FirstOrDefault();
                    if (quantityData != null)
                    {
                        orderListItem.Quantity = quantityData.PropertyValue != string.Empty ? Int32.Parse(quantityData.PropertyValue) : 0;
                    }

                    var nameData = messageData.Properties.Where(x => x.PropertyName.ToLower() == "name").FirstOrDefault();
                    if (nameData != null)
                    {
                        orderListItem.Name = nameData.PropertyValue;
                    }

                    var cadData = messageData.Properties.Where(x => x.PropertyName.ToLower() == "cad data").FirstOrDefault();
                    if (cadData != null)
                    {
                        orderListItem.CadData = cadData.PropertyValue;
                    }


                    var producerData = messageData.Properties.Where(x => x.PropertyName.ToLower() == "producer").FirstOrDefault();
                    if (producerData != null)
                    {
                        orderListItem.Producer = producerData.PropertyValue;
                    }

                    var priceData = messageData.Properties.Where(x => x.PropertyName.ToLower() == "price").FirstOrDefault();
                    if (priceData != null)
                    {
                        orderListItem.Price = priceData.PropertyValue != string.Empty ? decimal.Parse(priceData.PropertyValue) : 0;
                    }


                    var sumData = messageData.Properties.Where(x => x.PropertyName.ToLower() == "sum").FirstOrDefault();
                    if (sumData != null)
                    {
                        orderListItem.Sum = sumData.PropertyValue != string.Empty ? decimal.Parse(sumData.PropertyValue) : 0;
                    }


                    //var statusProperty = messageData.Properties.Where(x => x.PropertyName.ToString().ToLower() == "status").FirstOrDefault();

                    //if (statusProperty != null)
                    //{
                    //    var status = await _repositoryContext.CodeMasters.Where(x => x.Value == statusProperty.PropertyValue && x.Type == "drawing").FirstOrDefaultAsync();
                    //    if (status != null)
                    //        drawingListItem.StatusId = status.Id;
                    //    else
                    //    {
                    //        var statusItem = await CreateStatus(statusProperty.PropertyValue, "drawing");
                    //        if (statusItem != null)
                    //            drawingListItem.StatusId = statusItem.Id;
                    //    }
                    //}

                    await _repositoryContext.OrderLists.AddAsync(orderListItem);
                    await _repositoryContext.SaveChangesAsync();



                }

            }

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }


    #endregion


}