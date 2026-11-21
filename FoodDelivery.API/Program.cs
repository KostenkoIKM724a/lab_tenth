using FoodDelivery.Application;
using FoodDelivery.Domain;
using FoodDelivery.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// In-memory repositories are singletons so test data persist while the API runs.
builder.Services.AddSingleton<IRestaurantRepository, InMemoryRestaurantRepository>();
builder.Services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();
builder.Services.AddSingleton<ILoyaltyCardRepository, InMemoryLoyaltyCardRepository>();
builder.Services.AddSingleton<CatalogService>();
builder.Services.AddSingleton<OrderService>();

var app = builder.Build();

app.MapControllers();
app.Run();
