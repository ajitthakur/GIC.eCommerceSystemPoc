using GIC.UserService.Data.Context;
using GIC.UserService.Infrastructure;
using GIC.UserService.Models;
using GIC.UserService.Utilities;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddResponseCompression(options =>
//{
//    options.EnableForHttps = true;
//});

builder.Host.UseSerilog((context, configuration) => 
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddDbContext<UserDbContext>(o => o.UseInMemoryDatabase("Users"));
builder.Services.Configure<KafkaOption>(builder.Configuration.GetSection(KafkaOption.Key));

////Add services to the container
//Data Services
builder.Services.AddDataService();

//Kafka Event register
builder.Services.AddKafkaEventPublishService();
builder.Services.AddKafkaEventConsumeService();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty;
    });

//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
