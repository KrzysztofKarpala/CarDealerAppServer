using CarDealerAppServer.Api.Extensions;
using CarDealerAppServer.Application.Consumers;
using CarDealerAppServer.Application.Queries;
using CarDealerAppServer.Core.Repository;
using CarDealerAppServer.Infrastructure.DbSettings;
using CarDealerAppServer.Infrastructure.Repositories;
using CarDealerAppServer.Shared;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using StackExchange.Redis;

var root = Directory.GetCurrentDirectory();
var dotEnd = Path.Combine(root, ".env");
DotEnv.Load(dotEnd);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
#pragma warning disable CS0618
BsonDefaults.GuidRepresentationMode = GuidRepresentationMode.V3;
#pragma warning restore CS0618
BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

builder.Services.Configure<CarDatabaseSettings>(builder.Configuration.GetSection("Mongo"));
builder.Services.AddTransient<ICarRepository, CarRepository>();
builder.Services.AddTransient<ICacheService, CacheService>();

// Add redis
var conn = builder.Configuration.GetConnectionString("Redis");
builder.Services.AddSingleton<IConnectionMultiplexer>(options =>
    ConnectionMultiplexer.Connect(conn));


builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.SetDefaultEndpointNameFormatter();
    busConfigurator.AddConsumer<AddCarToCacheConsumer>();
    busConfigurator.UsingRabbitMq((IBusRegistrationContext context, IRabbitMqBusFactoryConfigurator configurator) =>
    {
        var configSection = builder.Configuration.GetSection("RabbitMqSettings");
        var settings = new RabbitMqSettings();
        configSection.Bind(settings);
        configurator.Host(settings.HostName, h =>
        {
            h.Username(settings.UserName);
            h.Password(settings.Password);
        });
        configurator.ConfigureEndpoints(context);
    });
});

builder.Services.AddMediatR(c => c.RegisterServicesFromAssemblyContaining<GetCarsQuery>());

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Set cors policy, change when production
builder.Services.AddCors(policy =>
{
    policy.AddPolicy("CorsPolicy", opt => opt
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod());
});

builder.Services.AddApiVersioning(config =>
{
    config.DefaultApiVersion = new ApiVersion(1, 0);
    config.AssumeDefaultVersionWhenUnspecified = true;
    config.ReportApiVersions = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
