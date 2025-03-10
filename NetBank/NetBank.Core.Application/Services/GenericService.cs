using AutoMapper;
using NetBank.Core.Application.Interfaces.Services;
using NetBank.Core.Application.Services.Repositories;

namespace NetBank.Core.Application.Services
{
    public class GenericService<SaveViewModel, ViewModel, Entity> : IGenericService<SaveViewModel, ViewModel, Entity>
        where SaveViewModel : class
        where ViewModel : class
        where Entity : class
    {

        private readonly IGenericRepository<Entity> _repository;

        private readonly IMapper _mapper;

        public GenericService(IGenericRepository<Entity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public virtual async Task<SaveViewModel> CreateAsync(SaveViewModel viewModel)
        {
            Entity entity = _mapper.Map<Entity>(viewModel); 
            entity = await _repository.AddAsync(entity);
            return _mapper.Map<SaveViewModel>(entity);
        }

        public virtual async Task DeleteAsync(int id)
        {
            Entity entity = await _repository.GetById(id);
            await _repository.Delete(entity);
        }

        public virtual async Task<List<ViewModel>> GetAllAsync()
        {
            List<Entity> entities = await _repository.GetAllAsync();
            return _mapper.Map<List<ViewModel>>(entities);
        }

        public virtual async Task<SaveViewModel> GetByIdSaveViewModel(int id)
        {
            Entity entity = await _repository.GetById(id);
            return _mapper.Map<SaveViewModel>(entity);
        }

        public virtual async Task<ViewModel> GetByIdViewModel(int id)
        {
            Entity entity = await _repository.GetById(id);
            return _mapper.Map<ViewModel>(entity);
        }

        public virtual async Task<SaveViewModel> UpdateAsync(SaveViewModel viewModel, int id)
        {
            Entity entity = _mapper.Map<Entity>(viewModel);
            entity = await _repository.UpdateAsync(entity, id);
            return _mapper.Map<SaveViewModel>(entity);
        }
    }
}
