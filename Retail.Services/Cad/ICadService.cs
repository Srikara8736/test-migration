using Retail.DTOs;
using Retail.DTOs.Cad;
using Retail.DTOs.UserAccounts;
using Retail.DTOs.XML;

namespace Retail.Services.Cad;

public interface ICadService
{

    public List<CustomerItem> GetAllCustomer();

    public List<Retail.Data.Entities.Stores.Store> GetStoresByCustomerNo(string customerNo);

    public Task<bool> LoadXMLData(Message message);

    public Task<ResultDto<List<CadUploadHistoryResponseDto>>> GetCadUploadHistoryByStore(Guid storeId, CancellationToken cancellationToken=default);



}
