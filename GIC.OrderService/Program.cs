using GIC.OrderService.Data.Context;
using GIC.OrderService.Models;
using GIC.OrderService.Utilities;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddDbContext<OrderDbContext>(o => o.UseInMemoryDatabase("Orders"));
builder.Services.Configure<KafkaOption>(builder.Configuration.GetSection(KafkaOption.Key));

////Add services to the container
//Data Services
builder.Services.AddDataService();

//Kafka Event register
builder.Services.AddKafkaEventPublishService();
builder.Services.AddKafkaEventConsumeService();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
