using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection; 
using OrderService.Application.Interfaces;
using OrderService.Domain.Entities;
using OrderService.Contracts;
using order_service.Infrastructure.Config.Models;

namespace OrderService.Infrastructure.Kafka
{
    public class KafkaProductConsumer : BackgroundService
    {
        private readonly string _broker; // ตัวแปรสำหรับเก็บค่า broker ของ Kafka
        private readonly string _topic = "product-created"; // ชื่อ topic ที่จะ subscribe
        private readonly IServiceScopeFactory _scopeFactory; // ใช้สำหรับสร้าง scope ของ DI
        private readonly ILogger<KafkaProductConsumer> _logger; // ตัว logger สำหรับบันทึก log

        public KafkaProductConsumer(IOptions<KafkaSettings> kafkaOptions, IServiceScopeFactory scopeFactory, ILogger<KafkaProductConsumer> logger)
        {
            _broker = kafkaOptions.Value.BootstrapServers; // กำหนดค่า broker จาก options
            _scopeFactory = scopeFactory; // กำหนดค่า scopeFactory
            _logger = logger; // กำหนดค่า logger
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _broker, // กำหนด broker สำหรับ consumer
                GroupId = "order-service-consumer", // กำหนด group id
                AutoOffsetReset = AutoOffsetReset.Earliest // อ่าน message ตั้งแต่ต้น
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build(); // สร้าง consumer
            consumer.Subscribe(_topic); // subscribe topic

            while (!stoppingToken.IsCancellationRequested) // วนลูปรับ message จนกว่าจะถูกยกเลิก
            {
                try
                {
                    var result = consumer.Consume(stoppingToken); // รับ message จาก Kafka
                    var productEvent = System.Text.Json.JsonSerializer.Deserialize<ProductCreatedEvent>(result.Message.Value); // แปลง message เป็น object
                    if (productEvent != null)
                    {
                        // Create a scope for DI
                        using var scope = _scopeFactory.CreateScope(); // สร้าง scope สำหรับ DI
                        var service = scope.ServiceProvider.GetRequiredService<IOrderProductService>(); // ดึง service จาก DI
                        var product = new Product(
                            productEvent.Id,
                            productEvent.Name,
                            productEvent.Description,
                            productEvent.Price,
                            productEvent.Stock,
                            productEvent.CreatedAt,
                            productEvent.UpdatedAt
                        ); // สร้าง object Product จาก event
                        await service.UpsertProductAsync(product); // เรียก service เพื่อ upsert product (Upserted = Insert (เพิ่มใหม่) หรือ Update (แก้ไข) )
                        _logger.LogInformation("Upserted product from Kafka: {Id} {Name}", product.Id, product.Name); // log ข้อมูล
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error consuming Kafka message"); // log error กรณีเกิด exception
                }
            }
        }
    }
}
