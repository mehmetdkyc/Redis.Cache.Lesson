using RedisExampleApp.Models;

namespace RedisExampleApp.Repository
{
    public interface IProductRepository
    {

        Task<List<Product>> GetAsync();
        Task<Product> GetByIdAsync(int id);
        Task<Product> CreateAsync(Product product);
    }
}
