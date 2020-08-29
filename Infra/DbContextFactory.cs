using devboost.dronedelivery.felipe.EF.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace devboost.dronedelivery.felipe.EF
{
    public class DbContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var dbContextBuilder = new DbContextOptionsBuilder<DataContext>();

            var connectionString = configuration.GetConnectionString("grupo4devboostdronedeliveryContext");

            dbContextBuilder.UseSqlServer(connectionString);

            return new DataContext(dbContextBuilder.Options);
        }
    }
}
