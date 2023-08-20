using Demand.Application.Services;
using Demand.Domain.Models;
using Demand.Domain.Models.Validators;
using Demand.Domain.Repositories;
using Demand.Domain.Services;
using FluentValidation;
using FoodTotem.Infra.Repositories.Demand;
using FoodTotem.Infra.Repositories.Identity;
using Identity.Application.Ports;
using Identity.Application.Services;
using Identity.Domain.Models;
using Identity.Domain.Models.Validators;
using Identity.Domain.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServicesColletionExtensions
    {
        public static IServiceCollection AddDemandServices(
            this IServiceCollection services)
        {
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IFoodRepository, FoodRepository>();

            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IFoodService, FoodService>();

            services.AddScoped<IValidator<Food>, FoodValidator>();
            services.AddScoped<IValidator<Order>, OrderValidator>();

            return services;
        }

        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddScoped<ICustomerRepository, CustomerRepository>();

            services.AddScoped<ICustomerService, CustomerService>();

            services.AddScoped<IValidator<Customer>, CustomerValidator>();

            return services;
        }

        

    }
    
}
