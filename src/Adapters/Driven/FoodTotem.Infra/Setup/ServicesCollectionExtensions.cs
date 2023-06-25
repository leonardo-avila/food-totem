using FoodTotem.Infra.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class ServicesCollectionExtensions
	{
		public static void AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            var connectionString = configuration.GetConnectionString("DefaultConnection")!;

            services.AddDbContext<DemandContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            services.AddDbContext<IdentityContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
        }
    }

    public class DemandContextFactory : IDesignTimeDbContextFactory<DemandContext>
    {
        private readonly string _connectionString = "Server=localhost;Port=3306;Database=foodtotem;Uid=user;Pwd=uSeRpAsSwOrD;";

        public DemandContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DemandContext>();
            optionsBuilder.UseMySql(_connectionString, ServerVersion.AutoDetect(_connectionString));

            return new DemandContext(optionsBuilder.Options);
        }
    }
    public class IdentityContextFactory : IDesignTimeDbContextFactory<IdentityContext>
    {
        private readonly string _connectionString = "Server=localhost;Port=3306;Database=foodtotem;Uid=user;Pwd=uSeRpAsSwOrD;";

        public IdentityContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<IdentityContext>();
            optionsBuilder.UseMySql(_connectionString, ServerVersion.AutoDetect(_connectionString));

            return new IdentityContext(optionsBuilder.Options);
        }
    }
}

