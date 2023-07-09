using Demand.Domain.Models.Enums;
using Domain.Core;

namespace Demand.Domain.Models
{
    public class Order : Entity, IAggregateRoot
    {
        public string Customer { get; private set; }
        public OrderStatusEnum OrderStatus { get; private set; } = OrderStatusEnum.Received;
        public PaymentStatusEnum PaymentStatus { get; private set; } = PaymentStatusEnum.Pending;
        public DateTime OrderDate { get; private set; } = DateTime.Now;
        public DateTime LastStatusDate { get; private set; }
        public List<OrderFood> Combo { get; private set; } = new List<OrderFood>();
        public Order(string customer)
        {
            Customer = customer;
        }
        protected Order() { } // EF constructor

        public void AddFood(Food food, int quantity)
        {
            Combo.Add(new OrderFood(food.Id, Id, quantity));
        }

        public double GetTotal()
        {
            return Combo.Sum(x => x.Food.Price * x.Quantity);
        }

        public void SetOrderStatus(OrderStatusEnum orderStatus)
        {
            OrderStatus = orderStatus;
            LastStatusDate = DateTime.Now;
        }

        public void SetPaymentStatus(PaymentStatusEnum paymentStatus)
        {
            PaymentStatus = paymentStatus;
        }

        public void SetCombo(List<OrderFood> combo)
        {
            Combo = combo;
        }

    }
}