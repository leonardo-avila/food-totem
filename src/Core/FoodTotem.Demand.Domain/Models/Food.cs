﻿using System.Text.Json.Serialization;
using FoodTotem.Demand.Domain.Models.Enums;
using FoodTotem.Domain.Core;

namespace FoodTotem.Demand.Domain.Models
{
    public class Food : Entity, IAggregateRoot
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string ImageUrl { get; private set; }
        public double Price { get; private set; }
        public FoodCategoryEnum Category { get; private set; }
        [JsonIgnore]
        public List<OrderFood>? Orders { get; private set; }

        public Food(string name, string description, string imageUrl, double price, FoodCategoryEnum category)
        {
            Name = name;
            Description = description;
            ImageUrl = imageUrl;
            Price = price;
            Category = category;
        }
        protected Food() { } // EF constructor

        public void UpdateName(string newName)
        {
            if (!string.IsNullOrEmpty(newName)) Name = newName;
        }

        public void UpdateDescription(string newDescription)
        {
            if (!string.IsNullOrEmpty(newDescription)) Description = newDescription;
        }

        public void UpdateImageUrl(string newImageUrl)
        {
            if (!string.IsNullOrEmpty(newImageUrl)) ImageUrl = newImageUrl;
        }

        public void UpdatePrice(double newPrice)
        {
            Price = newPrice;
        }

        public void UpdateCategory(FoodCategoryEnum newCategory)
        {
            Category = newCategory;
        }
    }
}
