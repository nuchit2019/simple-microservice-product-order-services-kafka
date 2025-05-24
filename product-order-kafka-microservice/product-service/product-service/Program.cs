using Confluent.Kafka;
using Microsoft.Data.SqlClient;
using ProductService.Application.Interfaces;
using ProductService.Infrastructure.Interfaces;
using ProductService.Infrastructure.Kafka;
using ProductService.Infrastructure.Repositories;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// MSSQL Connection
builder.Services.AddScoped<IDbConnection>(_ => new SqlConnection(builder.Configuration.GetConnectionString("ProductDb")));

 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// DI Layer
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IKafkaProducer>(_ =>  new KafkaProducer(builder.Configuration["Kafka:BootstrapServers"])); 

builder.Services.AddScoped<IProductService, ProductService.Application.Services.ProductService>();

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
