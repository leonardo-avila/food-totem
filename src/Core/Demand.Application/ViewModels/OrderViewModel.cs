using Demand.Domain.Models;
using Demand.Domain.Models.Enums;

namespace Demand.Application.ViewModels
{
	public class OrderViewModel
	{
		public string Customer { get; set; }
		public OrderStatusEnum OrderStatus { get; set; }
		public PaymentStatusEnum PaymentStatus { get; set; }
		public DateTime OrderDate { get; set; }
		public DateTime LastStatusDate { get; set; }
		public List<OrderFood> Combo { get; set; }
	}
}

