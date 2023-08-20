using Data.Core;
using Demand.Domain.Models;
using Demand.Domain.Models.Enums;
using Demand.Domain.Repositories;
using FoodTotem.Infra.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FoodTotem.Infra.Repositories.Demand
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

