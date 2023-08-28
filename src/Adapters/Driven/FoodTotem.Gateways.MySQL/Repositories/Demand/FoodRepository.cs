using FoodTotem.Demand.Domain.Models;
using FoodTotem.Demand.Domain.Models.Enums;
using FoodTotem.Demand.Domain.Repositories;
using FoodTotem.Gateways.MySQL.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FoodTotem.Gateways.MySQL.Repositories.Demand
{
	public class FoodRepository : RepositoryBase<Food>, IFoodRepository
	{
        protected readonly DemandContext Db;
        protected readonly DbSet<Food> DbSet;

        public FoodRepository(DemandContext context) : base (context)
        {
            Db = context;
            DbSet = Db.Set<Food>();
        }

        public async Task<IEnumerable<Food>> GetFoodsByCategory(FoodCategoryEnum category)
        {
            return await DbSet.Where(f => f.Category.Equals(category)).ToListAsync();
        }
    }
}

