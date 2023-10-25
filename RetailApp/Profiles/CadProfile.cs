using AutoMapper;
using Retail.Data.Entities.Cad;
using Retail.Data.Entities.UserAccount;
using Retail.DTOs.Cad;
using Retail.DTOs.Roles;

namespace RetailApp.Profiles;

public class CadProfile : Profile
{
    public CadProfile()
    {
        CreateMap<CadUploadHistory, CadUploadHistoryDto>().ReverseMap();
        CreateMap<CadUploadHistory, CadUploadHistoryResponseDto>().ReverseMap();
    }

}
