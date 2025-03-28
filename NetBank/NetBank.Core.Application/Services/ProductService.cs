﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NetBank.Core.Application.Dtos.Account;
using NetBank.Core.Application.Helpers;
using NetBank.Core.Application.Interfaces.Services;
using NetBank.Core.Application.Services.Repositories;
using NetBank.Core.Application.ViewModels.Product;
using NetBank.Core.Domain.Entities;
using NetBank.Core.Domain.Enums;

namespace NetBank.Core.Application.Services
{
    public class ProductService : GenericService<SaveProductViewModel, ProductViewModel,Product>, IProductService
    {
        private readonly IProductRepository _repository;

        private readonly IMapper _mapper;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly AuthenticationResponse userInSession;

        public ProductService(IProductRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            userInSession = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
        }

        public async Task<SaveProductViewModel> GetProductMainByUserId(string id)
        {
            var query = await _repository.GetQuery().Where(x => x.UserId == id && x.IsMain && !x.IsDeleted).FirstOrDefaultAsync();

            SaveProductViewModel viewModel = new()
            {
                AccountNumber = query.AccountNumber,
                UserId = query.UserId,
                IsMain = query.IsMain,
                Balance = query.Balance,
                CreditLimit = query.CreditLimit,
                ProductType = query.ProductType,
            };
            return viewModel;
        }

        public override async Task<SaveProductViewModel> CreateAsync(SaveProductViewModel viewModel)
        {
            if(viewModel != null && viewModel.AmountOwed != null)
            {
                var cuentaPrincipal = await _repository.GetQuery().Where(x => x.UserId == viewModel.UserId && x.IsMain).FirstOrDefaultAsync();

                if(cuentaPrincipal != null && cuentaPrincipal.IsMain)
                {
                    cuentaPrincipal.Balance += viewModel.AmountOwed;
                    await _repository.UpdateAsync(cuentaPrincipal, cuentaPrincipal.AccountNumber);
                }
            }

            if(viewModel != null && viewModel.CreditLimit != null)
            {
                viewModel.AmountOwed = 0;
            }

            return await base.CreateAsync(viewModel);
        }


        public async Task<List<ProductViewModel>> GetProductsByUserId(string id)
        {
            var query = await _repository.GetQuery().Where(x => x.UserId == id && !x.IsDeleted).ToListAsync();
            return _mapper.Map<List<ProductViewModel>>(query);
        }

        public async Task<bool> DeleteProduct(int id,ProductType type,string userId)
        {
            var cuenta = await _repository.GetById(id);
            cuenta.IsDeleted = true;

            switch (type)
            {
                case ProductType.CuentaAhorro:
                    var cuentaPrincipal = await _repository.GetQuery().Where(x => x.UserId == userId && x.IsMain && !x.IsDeleted).FirstOrDefaultAsync();
                    
                    cuentaPrincipal.Balance += cuenta.Balance;

                    await _repository.UpdateAsync(cuentaPrincipal, cuentaPrincipal.AccountNumber);
                    await _repository.UpdateAsync(cuenta, cuenta.AccountNumber);
                    return true;
                    
                case ProductType.CreditCard:

                    if(cuenta.AmountOwed == null || cuenta.AmountOwed == 0)
                    {
                        await _repository.UpdateAsync(cuenta, cuenta.AccountNumber);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case ProductType.Prestamo:

                    if(cuenta.AmountOwed == null || cuenta.AmountOwed == 0)
                    {
                        await _repository.UpdateAsync(cuenta, cuenta.AccountNumber);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                    
            }

            
            return false;
        }

        public async Task <(List<ProductViewModel> tarjetas,List<ProductViewModel> cuentas)> GetAccountForCashAdvance()
        {
            var query = await _repository.GetQuery().Where(x => x.UserId == userInSession.Id && !x.IsDeleted).ToListAsync();

            List<ProductViewModel> tarjetas = query
                .Where(x => x.ProductType == ProductType.CreditCard)
                .Select(x => new ProductViewModel
                {
                    AccountNumber = x.AccountNumber,
                    ProductType = ProductType.CreditCard,
                    AmountOwed = x.AmountOwed,
                    Balance = x.Balance,
                    CreditLimit = x.CreditLimit,
                    UserId = x.UserId,
                    IsMain = x.IsMain,
                })
                .ToList();

            List<ProductViewModel> cuentas = query
                .Where(x => x.ProductType == ProductType.CuentaAhorro)
                .Select(x => new ProductViewModel
                {
                    AccountNumber = x.AccountNumber,
                    ProductType = ProductType.CuentaAhorro,
                    AmountOwed = x.AmountOwed,
                    Balance = x.Balance,
                    CreditLimit = x.CreditLimit,
                    UserId = x.UserId,
                    IsMain = x.IsMain,
                })
                .ToList();
            
            return (tarjetas,cuentas);
        }

       

        //Agrege un metodo

        public async Task<ProductViewModel> GetProductByAccountNumber(int accountNumber)
        {
            try
            {
                // Obtener un solo producto
                var product = await _repository.GetQuery()
                                               .Where(x => x.AccountNumber == accountNumber && !x.IsDeleted)
                                               .FirstOrDefaultAsync();
                return _mapper.Map<ProductViewModel>(product);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<ProductViewModel>> GetCuentasAhorrosByUserId(string id)
        {
            var query = await _repository.GetQuery().Where(x => x.UserId == id && x.ProductType == ProductType.CuentaAhorro && !x.IsDeleted).ToListAsync();
            return _mapper.Map<List<ProductViewModel>>(query);
        }

        public async Task<List<ProductViewModel>> GetCreditCardsByUserId(string id)
        {
            var query = await _repository.GetQuery().Where(x => x.UserId == id && x.ProductType == ProductType.CreditCard && !x.IsDeleted).ToListAsync();
            return _mapper.Map<List<ProductViewModel>>(query);
        }

        public async Task<List<ProductViewModel>> GetLoandsByUserId(string id)
        {
            var query = await _repository.GetQuery().Where(x => x.UserId == id && x.ProductType == ProductType.Prestamo && !x.IsDeleted).ToListAsync();
            return _mapper.Map<List<ProductViewModel>>(query);
        }
    }
}
