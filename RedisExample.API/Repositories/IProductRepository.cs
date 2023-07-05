using RedisExample.API.Models;

namespace RedisExample.API.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();

        Task<Product> GetByIdAsyncid(int id);
        Task<Product> CreateAsync(Product product);

    }
}
