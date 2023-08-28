using System.Text.Json.Serialization;
using FoodTotem.Gateways.MySQL.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Food Totem API", Version = "v1" });
    var filePath = Path.Combine(AppContext.BaseDirectory, "FoodTotem.API.xml");
    c.IncludeXmlComments(filePath);
});

// Set DbContexts
builder.Services.AddDatabaseConfiguration(builder.Configuration);

// Dependency Injection
builder.Services.AddDemandServices();
builder.Services.AddIdentityServices();

builder.Services.AddPaymentServices();

var app = builder.Build();

using (var serviceScope = app.Services.CreateScope())
{
    var demandContext = serviceScope.ServiceProvider.GetService<DemandContext>();
    demandContext!.Database.Migrate();
    var identityContext = serviceScope.ServiceProvider.GetService<IdentityContext>();
    identityContext!.Database.Migrate();
}

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();


