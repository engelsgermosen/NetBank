using AutoMapper;
using NetBank.Core.Application.Dtos.Account;
using NetBank.Core.Application.Dtos.Rol;
using NetBank.Core.Application.ViewModels.Role;
using NetBank.Core.Application.ViewModels.User;

namespace NetBank.Core.Application.Mapping
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<AuthenticationRequest, LoginViewModel>()
                .ForMember(x => x.Error, opt => opt.Ignore())
                .ForMember(x => x.HasError, opt => opt.Ignore());

            CreateMap<RegisterRequest, SaveUserViewModel>()
                .ForMember(x => x.Error, opt => opt.Ignore())
                .ForMember(x => x.HasError, opt => opt.Ignore());

            CreateMap<RolList, RolViewModel>()
                .ForMember(x => x.Error, opt => opt.Ignore())
                .ForMember(x => x.HasError, opt => opt.Ignore());
        }
    }
}
