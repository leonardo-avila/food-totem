using FluentValidation;
using FoodTotem.Gateways.MySQL.Repositories.Identity;
using FoodTotem.Identity.UseCase.Ports;
using FoodTotem.Identity.UseCase.UseCases;
using FoodTotem.Identity.Domain.Models;
using FoodTotem.Identity.Domain.Models.Validators;
using FoodTotem.Identity.Domain.Ports;
using FoodTotem.Identity.Domain.Services;
using FoodTotem.Gateways.Http;
using FoodTotem.Gateways.Catalog.Services;
using FoodTotem.Gateways.Demand.Services;
using FoodTotem.Gateways.Payment.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServicesColletionExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddScoped<ICustomerRepository, CustomerRepository>();

            services.AddScoped<ICustomerUseCase, CustomerUseCase>();

            services.AddScoped<ICustomerService, CustomerService>();

            services.AddScoped<IValidator<Customer>, CustomerValidator>();

            return services;
        }

        public static IServiceCollection AddGatewaysServices(this IServiceCollection services)
        {
            services.AddScoped<IHttpHandler, HttpHandler>();
            services.AddScoped<ICatalogService, CatalogService>();
            services.AddScoped<IDemandServices, DemandServices>();
            services.AddScoped<IPaymentServices, PaymentServices>();

            return services;
        }
    }
}
