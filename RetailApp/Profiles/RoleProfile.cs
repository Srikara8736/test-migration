using AutoMapper;
using Retail.Data.Entities.UserAccount;
using Retail.DTOs.Roles;

namespace RetailApp.Profiles;

/// <summary>
/// Automapper for Role model and Entity
/// </summary>
public class RoleProfile : Profile
{
    public RoleProfile()
    {
        CreateMap<Role, RoleDto>().ReverseMap();
        CreateMap<Role, RoleResponseDto>().ReverseMap();
    }
}

