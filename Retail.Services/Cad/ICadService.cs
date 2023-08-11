using Retail.DTOs;
using Retail.DTOs.Cad;
using Retail.DTOs.UserAccounts;

namespace Retail.Services.Cad;

public interface ICadService
{

    public List<CustomerItem> GetAllCustomer();

    public List<Store> GetStoresByCustomerNo(string customerNo);
       
}
