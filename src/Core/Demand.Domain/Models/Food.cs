using Demand.Domain.Models.Enums;
using Domain.Core;

namespace Demand.Domain.Models
{
    public class Food : Entity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string ImageUrl { get; private set; }
        public double Price { get; private set; }
        public FoodCategoryEnum Category { get; private set; }
        
        public Food(string name, string description, string imageUrl, double price, FoodCategoryEnum category)
        {
            Name = name;
            Description = description;
            ImageUrl = imageUrl;
            Price = price;
            Category = category;
        }
        protected Food() { } // EF constructor

    }
}
