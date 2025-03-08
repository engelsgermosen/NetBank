using AutoMapper;
using NetBank.Core.Application.Dtos.Account;
using NetBank.Core.Application.Dtos.Rol;
using NetBank.Core.Application.ViewModels.Product;
using NetBank.Core.Application.ViewModels.Role;
using NetBank.Core.Application.ViewModels.User;
using NetBank.Core.Domain.Entities;

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

            //product -> ProductViewModel
            CreateMap<Product, ProductViewModel>()
                .ReverseMap()
                .ForMember(x => x.OriginTransactions, opt => opt.Ignore())
                .ForMember(x => x.DestinationTransactions, opt => opt.Ignore());
        }
    }
}
