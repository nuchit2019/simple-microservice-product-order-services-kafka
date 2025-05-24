using OrderService.Domain.Entities;
using Dapper;
using System.Data;
using OrderService.Infrastructure.Interfaces;

namespace OrderService.Infrastructure.Repositories;

public class OrderProductRepository(IDbConnection db) : IOrderProductRepository
{ 
    public async Task<Product> UpsertAsync(Product product)
    {
 

        var updateSql = @"
                        UPDATE Products 
                        SET Name = @Name, Description = @Description, Price = @Price, Stock = @Stock, UpdatedAt = GETDATE()
                        WHERE Id = @Id;

                        IF @@ROWCOUNT = 0
                        BEGIN
                            INSERT INTO Products (Id,Name, Description, Price, Stock, CreatedAt, UpdatedAt)
                            OUTPUT INSERTED.Id, INSERTED.Name, INSERTED.Description, INSERTED.Price, INSERTED.Stock, INSERTED.CreatedAt, INSERTED.UpdatedAt
                            VALUES (@Id,@Name, @Description, @Price, @Stock, GETDATE(), GETDATE());
                        END
                        ELSE
                        BEGIN
                            SELECT Id, Name, Description, Price, Stock, CreatedAt, UpdatedAt FROM Products WHERE Id = @Id;
                        END
                        ";
        var result = await db.QuerySingleAsync<Product>(updateSql, product);
        return result;
    }

    public async Task<IEnumerable<Product>> GetAllAsync() => await db.QueryAsync<Product>("SELECT * FROM Products");
}
