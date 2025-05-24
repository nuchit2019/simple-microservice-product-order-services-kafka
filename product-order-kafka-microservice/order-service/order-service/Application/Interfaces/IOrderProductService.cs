using OrderService.Domain.Entities;

namespace OrderService.Application.Interfaces;

public interface IOrderProductService
{
    Task UpsertProductAsync(Product product);
    Task<IEnumerable<Product>> GetAllProductsAsync();
}