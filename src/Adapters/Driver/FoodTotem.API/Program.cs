using System.Text.Json.Serialization;
using FoodTotem.Infra.Contexts;
using Microsoft.EntityFrameworkCore;

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
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Set DbContexts
builder.Services.AddDatabaseConfiguration(builder.Configuration);


// Dependency Injection
builder.Services.AddDemandServices();
builder.Services.AddIdentityServices();

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


