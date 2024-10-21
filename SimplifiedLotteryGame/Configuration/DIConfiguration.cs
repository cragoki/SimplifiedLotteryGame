using LotteryResources.Services;
using LotteryResources.Services.Interfaces;
using LotteryResources.Storage;
using LotteryResources.Storage.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace SimplifiedLotteryGame.Configuration
{
    public static class DIConfiguration
    {
        public static void Register(IServiceCollection services)
        {
            #region Transient
            //Transient as we do not need to persist any instances, the services will act on data stored in the Data Store classes
            services.AddTransient<SimplifiedLotteryGame>();
            services.AddTransient<IPlayerService, PlayerService>();
            services.AddTransient<ITicketService, TicketService>();
            services.AddTransient<ICpuPlayerService, CpuPlayerService>();
            services.AddTransient<IGameService, GameService>();
            #endregion

            #region Singleton
            //Singleton as we will be storing the user balance data here, and it may be retrieved/updated from several places
            services.AddSingleton<IUserDataStore, UserDataStore>();
            services.AddSingleton<ITicketDataStore, TicketDataStore>();
            services.AddSingleton<IGameDataStore, GameDataStore>();
            services.AddSingleton<IRandomNumberService, RandomNumberService>();
            
            #endregion
        }
    }
}
