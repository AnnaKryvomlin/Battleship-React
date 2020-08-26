using BattleShip.DataAccess.Interfaces;
using BattleShip.DataAccess.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace BattleShip.Configurations
{
    public static class UnitOfWorkConfig
    {
        public static void ConfigureUnitOfWork(this IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }
    }
}