using Retail.Data.Entities.Customers;
using Retail.Data.Repository;
using Retail.DTOs.Cad;

namespace Retail.Services.Cad;

public class CadService : ICadService
{
    private readonly RepositoryContext _repositoryContext;

    public CadService( RepositoryContext repositoryContext)
    {
        _repositoryContext= repositoryContext;
    }

    public List<CustomerItem> GetAllCustomer()
    {
        var customers = _repositoryContext.Customers.Where(x => x.IsDeleted == false).ToList();
        
        var customerList = new List<CustomerItem>();

        foreach(var customer in customers)
        {
            var item = new CustomerItem
            {
                Id= customer.Id,
                Code = customer.Id.ToString(),
                Name = customer.Name,
                Email = customer.Email,
                Contact = customer.PhoneNumber
            };



            customerList.Add(item);
        }

        return customerList;

    }


    public List<Store> GetStoresByCustomerNo(string customerNo)
    {
        var stores = _repositoryContext.Stores.Where(x => x.CustomerId == new Guid(customerNo)).ToList();

        var storeList = new List<Store>();


        foreach (var store in stores)
        {
            var item = new Store
            {
                Name = store.Name ,
                No = store.StoreNumber,
                Id = store.Id
               
            };

            storeList.Add(item);
        }

        return storeList;
    }
}
