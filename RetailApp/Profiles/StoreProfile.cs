using Retail.Data.Entities.UserAccount;
using Retail.DTOs.Roles;
using AutoMapper;
using Retail.Data.Entities.Stores;
using Retail.DTOs.Stores;
using Retail.Data.Entities.Common;

namespace RetailApp.Profiles;

public class StoreProfile : Profile
{
    public StoreProfile()
    {
        CreateMap<Store, StoreDto>().ReverseMap();
        CreateMap<Store, StoreResponseDto>().ReverseMap();

        CreateMap<DrawingList, DrawingListDto>()
              .ForMember(dest => dest.Pno, opt => opt.MapFrom(src => src.DrawingListId))
             .ReverseMap();
        CreateMap<DrawingList, DrawingListResponseDto>()
            .ForMember(dest => dest.Pno, opt => opt.MapFrom(src => src.DrawingListId))
            .ReverseMap();

        CreateMap<CodeMaster, StoreStatusResponseDto>()
            .ForMember(dest =>dest.Name, opt => opt.MapFrom(src => src.Value))
            .ReverseMap();

        CreateMap<OrderList, OrderListDto>().ReverseMap();
        CreateMap<PackageData, PackageDataDto>().ReverseMap();
    }
}
