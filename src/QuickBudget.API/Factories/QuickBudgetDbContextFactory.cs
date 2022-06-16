using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using QuickBudget.Persistance;

namespace QuickBudget.API.Factories
{
    public class QuickBudgetDbContextFactory : IDesignTimeDbContextFactory<QuickBudgetDbContext>
    {
        public QuickBudgetDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
               .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
               .AddJsonFile("appsettings.json")
               .AddEnvironmentVariables()
               .Build();

            var optionsBuilder = new DbContextOptionsBuilder<QuickBudgetDbContext>();

            optionsBuilder.UseSqlServer(config.GetConnectionString("Default"), sqlServerOptionsAction: o => o.MigrationsAssembly("QuickBudget.API"));

            return new QuickBudgetDbContext(optionsBuilder.Options);
        }
    }
}
