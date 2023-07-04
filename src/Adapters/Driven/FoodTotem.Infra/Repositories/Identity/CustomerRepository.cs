using Data.Core;
using FoodTotem.Infra.Contexts;
using Identity.Application.Ports;
using Identity.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodTotem.Infra.Repositories.Identity
{
	public class CustomerRepository : ICustomerRepository
	{
        protected readonly IdentityContext Db;
        protected readonly DbSet<Customer> DbSet;

        public CustomerRepository(IdentityContext context)
        {
            Db = context;
            DbSet = Db.Set<Customer>();
        }

        public IUnitOfWork UnitOfWork => Db;

        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<Customer> GetCustomer(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task<Customer> GetCustomerByCPF(string customerCPF)
        {
            return await DbSet.AsNoTracking().FirstOrDefaultAsync(c => c.CPF.Equals(customerCPF));
        }

        public void Add(Customer customer)
        {
            DbSet.Add(customer);
        }

        public void Update(Customer customer)
        {
            DbSet.Update(customer);
        }

        public void Remove(Customer customer)
        {
            DbSet.Remove(customer);
        }

        public void Dispose()
        {
            Db.Dispose();
        }

    }
}

