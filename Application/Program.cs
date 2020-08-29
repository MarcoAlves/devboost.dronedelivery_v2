using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace devboost.dronedelivery.felipe
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class Program
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
