using RedisExample.API.Models;

namespace RedisExample.API.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAllAsync();

        Task<Product> GetByIdAsyncid(int id);
        Task<Product> CreateAsync(Product product);
    }
}
