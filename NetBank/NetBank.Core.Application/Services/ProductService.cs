using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NetBank.Core.Application.Interfaces.Services;
using NetBank.Core.Application.Services.Repositories;
using NetBank.Core.Application.ViewModels.Product;
using NetBank.Core.Domain.Entities;

namespace NetBank.Core.Application.Services
{
    public class ProductService : GenericService<SaveProductViewModel, ProductViewModel,Product>, IProductService
    {
        private readonly IProductRepository _repository;

        private readonly IMapper _mapper;

        public ProductService(IProductRepository repository, IMapper mapper) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<SaveProductViewModel> GetProductMainByUserId(string id)
        {
            var query = await _repository.GetQuery().Where(x => x.UserId == id && x.IsMain).FirstOrDefaultAsync();

            SaveProductViewModel viewModel = new()
            {
                Id = query.Id,
                UserId = query.UserId,
                IsMain = query.IsMain,
                Balance = query.Balance,
                CreditLimit = query.CreditLimit,
                ProductType = query.ProductType,
            };
            return viewModel;
        }
    }
}
