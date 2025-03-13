using AutoMapper;
using NetBank.Core.Application.Dtos.Account;
using NetBank.Core.Application.Dtos.Rol;
using NetBank.Core.Application.ViewModels.Beneficiare;
using NetBank.Core.Application.ViewModels.Payment;
using NetBank.Core.Application.ViewModels.Product;
using NetBank.Core.Application.ViewModels.Role;
using NetBank.Core.Application.ViewModels.Transaction;
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
                .ForMember(x => x.HasError, opt => opt.Ignore())
                .ReverseMap();


            CreateMap<RegisterRequest, SaveUserViewModel>()
                .ForMember(x => x.Error, opt => opt.Ignore())
                .ForMember(x => x.HasError, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<RolList, RolViewModel>()
                .ForMember(x => x.Error, opt => opt.Ignore())
                .ForMember(x => x.HasError, opt => opt.Ignore());

            //product -> ProductViewModel
            CreateMap<Product, ProductViewModel>()
                .ReverseMap()
                .ForMember(x => x.OriginTransactions, opt => opt.Ignore())
                .ForMember(x => x.DestinationTransactions, opt => opt.Ignore());

            CreateMap<AuthenticationResponse, UserViewModel>()
                .ReverseMap()
                .ForMember(x => x.IsVerified, opt => opt.Ignore())

                .ForMember(x => x.Error, opt => opt.Ignore())
                .ForMember(x => x.HasError, opt => opt.Ignore());

            CreateMap<AuthenticationResponse, SaveUserViewModel>()
                .ReverseMap()
                .ForMember(x => x.IsVerified, opt => opt.Ignore());

            CreateMap<UserInactivate, UserInactiveViewModel>()
                .ReverseMap();

            CreateMap<UpdateUserRequest, UpdateUserViewModel>()
                .ForMember(x => x.ConfirmPassword, opt => opt.Ignore())
                .ForMember(x => x.IsAdmin, opt => opt.Ignore())
                .ForMember(x => x.Error, opt => opt.Ignore())
                .ForMember(x => x.HasError, opt => opt.Ignore())
                .ReverseMap();


            CreateMap<Product, SaveProductViewModel>()
                .ReverseMap()
                .ForMember(x => x.DestinationTransactions, opt => opt.Ignore())
                .ForMember(x => x.OriginTransactions, opt => opt.Ignore());

            CreateMap<Transaction, SaveTransactionViewModel>()
                .ForMember(x => x.HasError, opt => opt.Ignore())
                .ForMember(x => x.Error, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(x => x.OriginProduct, opt => opt.Ignore())
                .ForMember(x => x.DestinationProduct, opt => opt.Ignore());

            //beneficiarie -> BeneficiareViewModel
            CreateMap<Beneficiarie, BeneficiareViewModel>()
                .ReverseMap();

            CreateMap<Beneficiarie, SaveBeneficiareViewModel>()
                .ReverseMap();

            CreateMap<Payment, SavePaymentViewModel>()
                .ForMember(x => x.HasError, opt => opt.Ignore())
                .ForMember(x => x.Error, opt => opt.Ignore())
                .ForMember(x => x.Accounts, opt => opt.Ignore())

               .ReverseMap()
                .ForMember(x => x.OriginProduct, opt => opt.Ignore())
                .ForMember(x => x.DestinationProduct, opt => opt.Ignore());

        }
    }
}
