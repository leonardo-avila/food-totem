using System.Text;
using System.Text.Json.Serialization;
using FoodTotem.API.Setup;
using FoodTotem.Gateways.MySQL.Contexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

IdentityModelEventSource.ShowPII = true;
var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true); 

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

// Add services to the container.

builder.Services.AddControllers(options =>
    {
        options.Conventions.Add(new DashRouteConvention());
    }).AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Add authentication services
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTIssuerSigningKey"]!)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false
        };
        #if DEBUG
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                // Log the token received from the request
                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (token != null)
                {
                    Console.WriteLine($"Token received: {token}");
                }

                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                // Log the claims in the token after validation
                var claims = context.Principal?.Claims.Select(c => $"{c.Type}: {c.Value}");
                Console.WriteLine($"Token validated. Claims: {string.Join(", ", claims!)}");

                return Task.CompletedTask;
            },
            OnAuthenticationFailed = context =>
            {
                // Log the authentication failure
                Console.WriteLine($"Authentication failed. Exception: {context.Exception}");

                return Task.CompletedTask;
            }
        };
        #endif
    });

// Add authorization services
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Bearer", policy =>
    {
        policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
        policy.RequireAuthenticatedUser();
    });
});

// Add Swagger services
builder.Services.AddSwaggerGen(c =>
{
    // Configure Swagger options
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Food Totem API", Version = "v1" });
    var filePath = Path.Combine(AppContext.BaseDirectory, "FoodTotem.API.xml");
    c.IncludeXmlComments(filePath);

    // Add JWT authorization to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Set DbContexts
builder.Services.AddDatabaseConfiguration(builder.Configuration);

// Dependency Injection
builder.Services.AddGatewaysServices();
builder.Services.AddIdentityServices();

var app = builder.Build();

using (var serviceScope = app.Services.CreateScope())
{
    var identityContext = serviceScope.ServiceProvider.GetService<IdentityContext>();
    identityContext!.Database.Migrate();
}

app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    await next.Invoke();
});

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c => {
    c.OAuthClientId("swagger");
    c.OAuthAppName("Swagger");
});

app.UseAuthorization();

app.MapControllers();

app.Run();


