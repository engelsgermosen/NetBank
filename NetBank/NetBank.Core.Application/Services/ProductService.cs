using AutoMapper;
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
    }
}
