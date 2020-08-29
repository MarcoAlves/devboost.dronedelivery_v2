using devboost.dronedelivery.felipe.DTO.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace devboost.dronedelivery.felipe.EF.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<Pedido> Pedido { get; set; }

        public DbSet<Drone> Drone { get; set; }

        public DbSet<PedidoDrone> PedidoDrones { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
            var connectionString = configuration.GetConnectionString("grupo4devboostdronedeliveryContext");

            optionsBuilder
                .UseSqlServer(connectionString);
        }

    }
}
