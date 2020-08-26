namespace BattleShip.Configurations
{
    using BattleShip.DataAccess.Interfaces;
    using BattleShip.DataAccess.Repositories;
    using Microsoft.Extensions.DependencyInjection;

    public static class UnitOfWorkConfig
    {
        public static void ConfigureUnitOfWork(this IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }
    }
}
