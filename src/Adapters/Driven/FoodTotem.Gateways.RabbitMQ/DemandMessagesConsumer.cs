using System.Text;
using System.Text.Json;
using FoodTotem.Identity.Domain;
using FoodTotem.Identity.UseCase.OutputViewModels;
using FoodTotem.Identity.UseCase.Ports;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client.Events;

namespace FoodTotem.Demand.Gateways.RabbitMQ
{
    public class DemandMessagesConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private ICustomerUseCase? _customerUseCases;
        public DemandMessagesConsumer(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Run(() => 
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var messenger = scope.ServiceProvider.GetRequiredService<IMessenger>();
                _customerUseCases = scope.ServiceProvider.GetRequiredService<ICustomerUseCase>();

                messenger.Consume("order-updated-event",
                    (e) => ProccessMessage(this, (BasicDeliverEventArgs)e));
            }, stoppingToken);
        }


        private void ProccessMessage(object sender, BasicDeliverEventArgs e)
        {
            var message = Encoding.UTF8.GetString(e.Body.ToArray());
            var order = JsonSerializer.Deserialize<OrderUpdateNotification>(message);
            _customerUseCases.NotifyCustomer(order!);
        }
    }
}