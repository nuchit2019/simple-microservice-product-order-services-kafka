using OrderService.Domain.Entities;

namespace OrderService.Infrastructure.Interfaces;

public interface IOrderProductRepository
{
    Task<Product> UpsertAsync(Product product);
    Task<IEnumerable<Product>> GetAllAsync();
}
