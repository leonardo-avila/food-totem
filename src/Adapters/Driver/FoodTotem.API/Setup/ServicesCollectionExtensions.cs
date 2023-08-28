using FoodTotem.Demand.UseCase.UseCases;
using FoodTotem.Demand.Domain.Models;
using FoodTotem.Demand.Domain.Models.Validators;
using FoodTotem.Demand.Domain.Repositories;
using FoodTotem.Demand.UseCase.Ports;
using FluentValidation;
using FoodTotem.Gateways.MySQL.Repositories.Demand;
using FoodTotem.Gateways.MySQL.Repositories.Identity;
using FoodTotem.Identity.UseCase.Ports;
using FoodTotem.Identity.UseCase.UseCases;
using FoodTotem.Identity.Domain.Models;
using FoodTotem.Identity.Domain.Models.Validators;
using FoodTotem.Demand.Domain.Ports;
using FoodTotem.Demand.Domain.Services;
using FoodTotem.Identity.Domain.Ports;
using FoodTotem.Identity.Domain.Services;
using FoodTotem.Gateways.Http;
using FoodTotem.Gateways.MercadoPago;

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

        public static IServiceCollection AddPaymentServices(this IServiceCollection services)
        {
            services.AddScoped<IHttpHandler, HttpHandler>();

            services.AddScoped<IMercadoPagoPaymentService, MercadoPagoPaymentService>();

            return services;
        }
    }
}
