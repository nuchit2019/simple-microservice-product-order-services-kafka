namespace ProductService.Infrastructure.Interfaces;
using ProductService.Domain.Entities;

public interface IKafkaProducer
{
    Task PublishProductCreatedAsync(Product product);
}