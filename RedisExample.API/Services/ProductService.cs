using RedisExample.API.Models;
using RedisExample.API.Repositories;

namespace RedisExample.API.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<Product> CreateAsync(Product product)
        {
            return await _repository.CreateAsync(product);
            
        }

        public async Task<List<Product>> GetAllAsync()
        {
           return await _repository.GetAllAsync();
        }

        public async Task<Product> GetByIdAsyncid(int id)
        {
            return await _repository.GetByIdAsyncid(id);
        }
    }
}
