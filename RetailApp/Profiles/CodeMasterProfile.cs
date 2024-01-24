using AutoMapper;
using Retail.Data.Entities.Common;
using Retail.Data.Entities.UserAccount;
using Retail.DTOs.Master;
using Retail.DTOs.Roles;

namespace RetailApp.Profiles;

/// <summary>
/// Automapper for Status model and Entity
/// </summary>
public class CodeMasterProfile : Profile
{
    public CodeMasterProfile()
    {
        CreateMap<CodeMaster, CodeMasterDto>().ReverseMap();
        CreateMap<CodeMaster, CodeMasterResponseDto>().ReverseMap();


        CreateMap<CustomerCodemaster, CustomerCodeMasterDto>().ReverseMap();
        CreateMap<CustomerCodemaster, CustomerCodeMasterResponseDto>().ReverseMap();
    }
}

