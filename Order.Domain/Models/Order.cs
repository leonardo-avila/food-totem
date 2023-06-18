using Domain.Core;

namespace Demand.Domain.Models
{
    public class Order : Entity, IAggregateRoot
    {
        public string Name { get; private set; }
        public Order(string name)
        {
            Name = name;
        }

    }
}