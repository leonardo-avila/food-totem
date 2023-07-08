using Data.Core;
using Demand.Application.Ports;
using Demand.Domain.Models;
using Demand.Domain.Models.Enums;
using FoodTotem.Infra.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FoodTotem.Infra.Repositories.Demand
{
	public class FoodRepository : IFoodRepository
	{
        protected readonly DemandContext Db;
        protected readonly DbSet<Food> DbSet;

        public FoodRepository(DemandContext context)
        {
            Db = context;
            DbSet = Db.Set<Food>();
        }

        public IUnitOfWork UnitOfWork => Db;

        public async Task<IEnumerable<Food>> GetFoods()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<Food> GetFood(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task<IEnumerable<Food>> GetFoodsByCategory(FoodCategoryEnum category)
        {
            return await DbSet.Where(f => f.Category.Equals(category)).ToListAsync();
        }

        public Task<bool> AddFood(Food food)
        {
            DbSet.Add(food);
            return UnitOfWork.Commit();
        }

        public Task<bool> UpdateFood(Food food)
        {
            DbSet.Update(food);
            return UnitOfWork.Commit();
        }

        public Task<bool> RemoveFood(Food food)
        {
            DbSet.Remove(food);
            return UnitOfWork.Commit();
        }

        public void Dispose()
        {
            Db.Dispose();
        }
    }
}

