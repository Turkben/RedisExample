
using Microsoft.EntityFrameworkCore;
using RedisExample.API.Models;
using RedisExample.Cache;
using StackExchange.Redis;
using System.Text.Json;

namespace RedisExample.API.Repositories
{
    public class ProductRepositoryWithCache : IProductRepository
    {
        private const string productKey = "productCaches";
        private readonly IProductRepository _productRepository;
        private readonly RedisRepository _redisRepository;
        private readonly IDatabase _cacheDb;
        public ProductRepositoryWithCache(IProductRepository productRepository, RedisRepository redisRepository)
        {
            _productRepository = productRepository;
            _redisRepository = redisRepository;
            _cacheDb = _redisRepository.GetDatabase(1);
        }
        public async Task<Product> CreateAsync(Product product)
        {
            var newProduct = await _productRepository.CreateAsync(product);
            if (await _cacheDb.KeyExistsAsync(productKey))
            {
                await _cacheDb.HashSetAsync(productKey, newProduct.Id, JsonSerializer.Serialize(newProduct));
            }           
            return newProduct;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            if (!await _cacheDb.KeyExistsAsync(productKey))
            {
                return await LoadToCacheFromDbAsync();
            }

            var products = new List<Product>();

            var cachedProducts = await _cacheDb.HashGetAllAsync(productKey);
            foreach (var item in cachedProducts.ToList())
            {
                var product = JsonSerializer.Deserialize<Product>(item.Value);
                products.Add(product);
            }

            return products;
        }

        public async Task<Product> GetByIdAsyncid(int id)
        {
            if (await _cacheDb.KeyExistsAsync(productKey))
            {
                var product = await _cacheDb.HashGetAsync(productKey, id);
                return product.HasValue ? JsonSerializer.Deserialize<Product>(product) : null;
            }

            var products = await LoadToCacheFromDbAsync();
            return products.FirstOrDefault(x => x.Id == id);
        }

        private async Task<List<Product>> LoadToCacheFromDbAsync()
        {
            var products = await _productRepository.GetAllAsync();
            products.ForEach(product =>
            {
                _cacheDb.HashSetAsync(productKey, product.Id, JsonSerializer.Serialize(product));
            });
            return products;

        }
    }
}
