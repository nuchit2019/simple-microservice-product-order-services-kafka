using ProductService.Domain.Entities;
using Dapper;
using System.Data;
using ProductService.Infrastructure.Interfaces;

namespace ProductService.Infrastructure.Repositories;

public class ProductRepository(IDbConnection db) : IProductRepository
{
    public async Task<Product> InsertAsync(Product product)
    {
        var now = DateTime.UtcNow;
        var sql = @"
        INSERT INTO Products (Name, Description, Price, Stock)
        OUTPUT INSERTED.Id
        VALUES (@Name, @Description, @Price, @Stock);
    ";

        var parameters = new
        {
            product.Name,
            product.Description,
            product.Price,
            product.Stock
           
        };

        var id = await db.ExecuteScalarAsync<int>(sql, parameters);
        return product with { Id = id, CreatedAt = now, UpdatedAt = now };
    }

    public async Task<IEnumerable<Product>> GetAllAsync()  => await db.QueryAsync<Product>("SELECT * FROM Products");
}
