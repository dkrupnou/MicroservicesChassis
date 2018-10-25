using MicroservicesChassis.Logging;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace MicroservicesChassis.Playground
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseLogging();
    }
}
