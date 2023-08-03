using AutoMapper;
using Retail.Data.Entities.UserAccount;
using Retail.DTOs.UserAccounts;

namespace RetailApp.Profiles;

/// <summary>
/// Automapper for User model and Entity
/// </summary>
public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<User, UserResponseDto>().ReverseMap();        
    }
}

