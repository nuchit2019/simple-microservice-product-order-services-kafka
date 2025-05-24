using OrderService.Domain.Entities;
using OrderService.Application.Interfaces;
using OrderService.Infrastructure.Interfaces;
using OrderService.Domain.Entities;

namespace OrderService.Application.Services;

public class OrderProductService(IOrderProductRepository repo) : IOrderProductService
{
    public async Task UpsertProductAsync(Product product) => await repo.UpsertAsync(product);

    public async Task<IEnumerable<Product>> GetAllProductsAsync()  => await repo.GetAllAsync();
}
