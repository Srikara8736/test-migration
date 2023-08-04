using Retail.Data.Entities.UserAccount;
using Retail.DTOs.Roles;
using AutoMapper;
using Retail.Data.Entities.Stores;
using Retail.DTOs.Stores;

namespace RetailApp.Profiles;

public class StoreProfile : Profile
{
    public StoreProfile()
    {
        CreateMap<Store, StoreDto>().ReverseMap();
        CreateMap<Store, StoreResponseDto>().ReverseMap();
    }
}
