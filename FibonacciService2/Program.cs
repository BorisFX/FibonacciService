using FibonacciService2.Infrastructure.Queue;
using FibonacciService2.Infrastructure.Settings;
using FibonacciService2.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var settings = new AppSettings();
builder.Configuration.Bind(settings);
builder.Services.AddSingleton(settings);
// Add services to the container.

builder.Services.AddScoped<ICalculatorService, CalculatorService>();
builder.Services.AddScoped<IMessageQueueService, RabbitMQService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Fibonacci API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();
