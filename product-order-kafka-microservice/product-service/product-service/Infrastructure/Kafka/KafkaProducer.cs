using Confluent.Kafka;
using ProductService.Domain.Entities;
using ProductService.Contracts;
using System.Text.Json;
using ProductService.Infrastructure.Interfaces;

namespace ProductService.Infrastructure.Kafka;

public class KafkaProducer : IKafkaProducer
{
    private readonly string _broker;
    private readonly string _topic = "product-created";
    private readonly IProducer<Null, string> _producer;

    public KafkaProducer(string kafkaBroker)
    {
        _broker = kafkaBroker;
        var config = new ProducerConfig { BootstrapServers = kafkaBroker };
        _producer = new ProducerBuilder<Null, string>(config).Build();
    }

    public async Task PublishProductCreatedAsync(Product product)
    {
        var @event = new ProductCreatedEvent(product.Id, product.Name, product.Description, product.Price);
        var json = JsonSerializer.Serialize(@event);
        await _producer.ProduceAsync(_topic, new Message<Null, string> { Value = json });
    }
}
