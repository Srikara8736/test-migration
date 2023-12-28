﻿using Retail.DTOs;
using Retail.DTOs.Cad;
using Retail.DTOs.UserAccounts;
using Retail.DTOs.XML;
namespace Retail.Services.Cad;

public interface ICadService
{

    public List<CustomerItem> GetAllCustomer();

    public List<DTOs.Cad.Store> GetStoresByCustomerNo(string customerNo);

    public Task<bool> LoadXMLData(Message message, Guid storeId,string type);
    Task<Retail.Data.Entities.Stores.DrawingList> LoadDrawingData(Guid storeId, MessageData messageData);

    public Task<ResultDto<List<CadUploadHistoryResponseDto>>> GetCadUploadHistoryByStore(Guid storeId, CancellationToken cancellationToken=default);


    Task<Retail.Data.Entities.FileSystem.Document> InsertDocument(string Name, string path, Guid typeGuid);

    Task<Retail.Data.Entities.Stores.StoreDocument> InsertStoreDocument(Guid storeId, Guid documentId);

    Task<Retail.Data.Entities.Cad.CadUploadHistory> InsertCadUploadHistory(Guid storeId, string fileName);

    Task<bool> LoadOrderListData(Guid storeId, OrderMessageBlock orderMessage);
}
