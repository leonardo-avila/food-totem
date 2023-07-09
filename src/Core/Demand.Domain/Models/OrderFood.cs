using System.Text.Json.Serialization;
using Domain.Core;

namespace Demand.Domain.Models
{
    public class OrderFood : Entity
    {
        [JsonIgnore]
        public Guid FoodId { get; private set; }
        public Food Food { get; private set; }
        [JsonIgnore]
        public Guid OrderId { get; private set; }
        [JsonIgnore]
        public Order Order { get; private set; }
        public int Quantity { get; private set; }

        public OrderFood(Guid foodId, int quantity)
        {
            FoodId = foodId;
            Quantity = quantity;
        }

        protected OrderFood() { } // EF constructor
    }
}