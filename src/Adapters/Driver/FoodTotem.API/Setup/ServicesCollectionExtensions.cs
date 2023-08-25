using Demand.UseCase.Services;
using Demand.Domain.Models;
using Demand.Domain.Models.Validators;
using Demand.Domain.Repositories;
using Demand.UseCase.Ports;
using FluentValidation;
using FoodTotem.Gateways.Repositories.Demand;
using FoodTotem.Gateways.Repositories.Identity;
using Identity.UseCase.Ports;
using Identity.UseCase.Services;
using Identity.Domain.Models;
using Identity.Domain.Models.Validators;
using Demand.Domain.Ports;
using Demand.Domain.Services;
using Identity.Domain.Ports;
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

            services.AddScoped<IOrderUseCases, OrderUseCases>();
            services.AddScoped<IFoodUseCases, FoodUseCases>();

            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IFoodService, FoodService>();

            services.AddScoped<IValidator<Food>, FoodValidator>();
            services.AddScoped<IValidator<Order>, OrderValidator>();

            return services;
        }

        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddScoped<ICustomerRepository, CustomerRepository>();

            services.AddScoped<ICustomerUseCase, CustomerUseCase>();

            services.AddScoped<ICustomerService, CustomerService>();

            services.AddScoped<IValidator<Customer>, CustomerValidator>();

            return services;
        }
    }
}
