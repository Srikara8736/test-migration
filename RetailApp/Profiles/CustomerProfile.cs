using AutoMapper;
using Retail.Data.Entities.Customers;
using Retail.Data.Entities.UserAccount;
using Retail.DTOs.Customers;

namespace RetailApp.Profiles;

/// <summary>
/// Automapper for Customer model and Entity
/// </summary>
public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<Customer, CustomerDto>().ReverseMap();
        CreateMap<Customer, CustomerResponseDto>().ReverseMap();


        CreateMap<Address, AddressDto > ().ReverseMap();

    }

}

