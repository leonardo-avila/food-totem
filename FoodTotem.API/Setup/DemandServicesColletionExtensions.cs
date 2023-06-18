using Demand.Application.Ports;
using Demand.Application.Services;
using Demand.Domain.Services;
using FoodTotem.Infra.Repositories;

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

    }
}
