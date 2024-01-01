using Retail.DTOs;
using Retail.DTOs.Cad;
using Retail.DTOs.UserAccounts;
using Retail.DTOs.XML;
namespace Retail.Services.Cad;

public interface ICadService
{
    /// <summary>
    /// Get All Customers
    /// </summary>
    /// <returns> All Customer Informations</returns>
    public List<CustomerItem> GetAllCustomer();

    /// <summary>
    /// Get Stores By Customer Number
    /// </summary>
    /// <param name="customerNo">customerNo</param>
    /// <returns> Store Information</returns>
    public List<DTOs.Cad.Store> GetStoresByCustomerNo(string customerNo);

    /// <summary>
    /// Load Space / Department List Zip XML data
    /// </summary>
    /// <param name="message">Deserialized XML Model</param>
    /// <param name="storeId">Store Identifier</param>
    /// <param name="type">Store Type</param>
    /// <returns> True / False of Loading XMl Data Status</returns>
    public Task<bool> LoadXMLData(Message message, Guid storeId,string type);


    /// <summary>
    /// Load Drawing XMl Data
    /// </summary>
    /// <param name="storeId">Store Identifier</param>
    /// <param name="messageData">Store Data Identifier</param>
    /// <returns>Drawing Type Information</returns>
    Task<Retail.Data.Entities.Stores.DrawingList> LoadDrawingData(Guid storeId, MessageData messageData);


    /// <summary>
    /// Cad Upload History By Store
    /// </summary>
    /// <param name="storeId">Store Identifier</param>
    /// <param name="cancellationToken">Cancellation Token</param>
    /// <returns>List of Upload History Information</returns>
    public Task<ResultDto<List<CadUploadHistoryResponseDto>>> GetCadUploadHistoryByStore(Guid storeId, CancellationToken cancellationToken=default);

    /// <summary>
    /// Insert Document
    /// </summary>
    /// <param name="Name">Document Name</param>
    /// <param name="path">Document path</param>
    /// <param name="typeGuid">Document Type</param>
    /// <returns>Document Information</returns>
    Task<Retail.Data.Entities.FileSystem.Document> InsertDocument(string Name, string path, Guid typeGuid);


    /// <summary>
    /// Insert Store Document
    /// </summary>
    /// <param name="storeId">Store Document Identifier</param>
    /// <param name="documentId">Document Indetifier</param>
    /// <returns>Store Document Information</returns>
    Task<Retail.Data.Entities.Stores.StoreDocument> InsertStoreDocument(Guid storeId, Guid documentId);


    /// <summary>
    /// Insert Cad Upload History for Store
    /// </summary>
    /// <param name="storeId">Store Identifier</param>
    /// <param name="fileName">File Name</param>
    /// <returns> Upload History Information</returns>
    Task<Retail.Data.Entities.Cad.CadUploadHistory> InsertCadUploadHistory(Guid storeId, string fileName);


    /// <summary>
    /// Load Order Type XML Data
    /// </summary>
    /// <param name="storeId">Store Identifier</param>
    /// <param name="orderMessage">Deserialize Model</param>
    /// <returns>True / False status of XML load</returns>
    Task<bool> LoadOrderListData(Guid storeId, OrderMessageBlock orderMessage);
}
