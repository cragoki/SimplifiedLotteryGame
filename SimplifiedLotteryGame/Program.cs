using LotteryResources.Models.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SimplifiedLotteryGame.Configuration;

namespace SimplifiedLotteryGame
{
    class Program
    {
        static void Main(string[] args)
        {
            using var host = CreateHostBuilder(args).Build();
            var helloWorldService = host.Services.GetRequiredService<SimplifiedLotteryGame>();
            helloWorldService.Execute();
        }
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostContext, configBuilder) =>
                {
                    //Adding this in so appsettings can be accessed by LotteryResourcesProject
                    configBuilder.SetBasePath(AppContext.BaseDirectory)
                                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

                })
                .ConfigureServices((hostContext, services) =>
                {
                    //Register my Configuration models by mapping appsettings to them...
                    var configuration = hostContext.Configuration;
                    services.Configure<PlayerConfiguration>(configuration.GetSection("PlayerInformation"));
                    services.Configure<TicketConfiguration>(configuration.GetSection("TicketInformation"));
                    services.Configure<GameConfiguration>(configuration.GetSection("GameInformation"));
                    services.Configure<CpuConfiguration>(configuration.GetSection("CpuInformation"));

                    //Decided to keep DI Confi within the root project, this way if I was to re-use the Lottery Resources, I could selectively decide which scope to use and which services to register
                    DIConfiguration.Register(services);
                });
        }
    }
}