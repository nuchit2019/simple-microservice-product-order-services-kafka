using ProductService.Domain.Entities;

namespace ProductService.Infrastructure.Interfaces;

public interface IProductRepository
{
    Task<Product> InsertAsync(Product product);
    Task<IEnumerable<Product>> GetAllAsync();
}