using Data.Core;
using FoodTotem.Infra.Contexts;
using Identity.Application.Ports;
using Identity.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodTotem.Infra.Repositories.Identity
{
	public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
	{
        protected readonly IdentityContext Db;
        protected readonly DbSet<Customer> DbSet;

        public CustomerRepository(IdentityContext context) : base(context)
        {
            Db = context;
            DbSet = Db.Set<Customer>();
        }

        public async Task<Customer> GetCustomerByCPF(string customerCPF)
        {
            return await DbSet.AsNoTracking()
                .FirstOrDefaultAsync(c => c.CPF!.Equals(customerCPF));
        }
    }
}

