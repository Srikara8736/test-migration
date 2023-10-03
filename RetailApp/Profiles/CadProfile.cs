using AutoMapper;
using Retail.Data.Entities.UserAccount;
using Retail.DTOs.Roles;

namespace RetailApp.Profiles;

public class CadProfile : Profile
{
    public CadProfile()
    {
        CreateMap<Role, RoleDto>().ReverseMap();
        CreateMap<Role, RoleResponseDto>().ReverseMap();
    }

}
