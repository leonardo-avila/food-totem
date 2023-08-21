using Demand.Domain.Models;
using Demand.Domain.Models.Enums;

namespace Demand.Application.ViewModels
{
	public class CheckoutOrderViewModel
	{
        public string Customer { get; set; }
        public OrderStatusEnum OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }
        public string Total { get; set; }
        public List<OrderFood> Combo { get; set; }
    
	}
}

