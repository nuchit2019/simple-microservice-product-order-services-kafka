using ProductService.Domain.Entities;

namespace ProductService.Application.Interfaces;

public interface IProductService
{
    Task<Product> CreateAsync(Product product);
    Task<IEnumerable<Product>> GetAllAsync();
}