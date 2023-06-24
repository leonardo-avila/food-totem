using Demand.Application.Ports;
using Demand.Application.Services;
using Demand.Domain.Services;
using FoodTotem.Infra.Context;
using FoodTotem.Infra.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DemandServicesColletionExtensions
    {
        public static IServiceCollection AddDemandServices(
            this IServiceCollection services)
        {
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderService, OrderService>();
            

            return services;
        }
        public static void AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration) 
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddDbContext<DemandContext>(options =>
                options.UseMySQL(configuration.GetConnectionString("DefaultConnection")));
        }
    }
}
