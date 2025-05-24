using Microsoft.Data.SqlClient;
using order_service.Infrastructure.Config.Models;
using OrderService.Application.Interfaces;
using OrderService.Application.Services;
using OrderService.Infrastructure.Interfaces;
using OrderService.Infrastructure.Kafka;
using OrderService.Infrastructure.Repositories;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Config section
builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("Kafka"));

// Infra
builder.Services.AddScoped<IDbConnection>(_ => new SqlConnection(builder.Configuration.GetConnectionString("OrderDb")));

// Repository
builder.Services.AddScoped<IOrderProductRepository, OrderProductRepository>();

// Service
builder.Services.AddScoped<IOrderProductService, OrderProductService>();

// Kafka Consumer (Hosted Service)
builder.Services.AddHostedService<KafkaProductConsumer>(); 


// Swagger/Controller/MinimalAPI
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
