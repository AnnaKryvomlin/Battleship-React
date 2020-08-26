using BattleShip.BusinessLogic.Interfaces;
using BattleShip.BusinessLogic.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BattleShip.Configurations
{
    public static class ServiceConfig
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddTransient<IPlayerService, PlayerService>();
            services.AddTransient<IGameService, GameService>();
            services.AddTransient<IStatisticsService, StatisticsService>();
        }
    }
}