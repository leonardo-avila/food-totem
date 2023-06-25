﻿using Demand.Application.Ports;
using Demand.Application.Services;
using Demand.Domain.Services;
using FoodTotem.Infra.Contexts;
using FoodTotem.Infra.Repositories.Demand;
using FoodTotem.Infra.Repositories.Identity;
using Identity.Application.Ports;
using Identity.Application.Services;
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
            

            return services;
        }

        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddScoped<ICustomerRepository, CustomerRepository>();

            services.AddScoped<ICustomerService, CustomerService>();

            return services;
        }

        

    }
    
}
