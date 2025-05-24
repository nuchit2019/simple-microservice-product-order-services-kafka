using ProductService.Application.Interfaces;
using ProductService.Domain.Entities; 
using ProductService.Infrastructure.Interfaces;

namespace ProductService.Application.Services;

public class ProductService(IProductRepository repo, IKafkaProducer producer) : IProductService
{
    public async Task<Product> CreateAsync(Product product)
    {
        // เพิ่มข้อมูลสินค้าใหม่ลงในฐานข้อมูล
        var created = await repo.InsertAsync(product);

        // ส่งข้อมูลสินค้าที่ถูกสร้างใหม่ไปยัง Kafka
        await producer.PublishProductCreatedAsync(created);

        // ส่งคืนข้อมูลสินค้าที่ถูกสร้างใหม่
        return created;
    }

    public async Task<IEnumerable<Product>> GetAllAsync() => await repo.GetAllAsync();
}