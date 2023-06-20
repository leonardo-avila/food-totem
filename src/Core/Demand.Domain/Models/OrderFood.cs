using Domain.Core;

namespace Demand.Domain.Models
{
    public class OrderFood : Entity
    {
        public Guid FoodId { get; private set; }
        public Food Food { get; private set; }
        public Guid OrderId { get; private set; }
        public Order Order { get; private set; }

        public OrderFood(Guid foodId, Guid orderId)
        {
            FoodId = foodId;
            OrderId = orderId;
        }

        protected OrderFood() { } // EF constructor
    }
}