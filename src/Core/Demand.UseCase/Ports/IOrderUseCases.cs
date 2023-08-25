using Demand.UseCase.InputViewModels;
using Demand.UseCase.OutputViewModels;

namespace Demand.UseCase.Ports
{
    public interface IOrderUseCases
    {
        Task<CheckoutOrderViewModel> GetOrder(Guid id);
        Task<IEnumerable<CheckoutOrderViewModel>> GetOrders();
        Task<IEnumerable<CheckoutOrderViewModel>> GetQueuedOrders();
        Task<IEnumerable<CheckoutOrderViewModel>> GetOngoingOrders();
        Task<CheckoutOrderViewModel> UpdateOrderStatus(Guid id, string newOrderStatus);
        Task<CheckoutOrderViewModel> UpdateOrder(Guid id, OrderInputViewModel order);
        Task<CheckoutOrderViewModel> CheckoutOrder(OrderInputViewModel orderViewModel);
        Task<bool> DeleteOrder(Guid id);
        Task<CheckoutOrderViewModel> ApproveOrderPayment(Guid id);
    }
}
