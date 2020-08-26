using BattleShip.BusinessLogic.Interfaces;
using BattleShip.DataAccess.Interfaces;
using BattleShip.Models.Entities;

namespace BattleShip.BusinessLogic.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IUnitOfWork db;

        public PlayerService(IUnitOfWork uow)
        {
            this.db = uow;
        }

        public void CreatePlayer(ApplicationUser user)
        {
            Player player = new Player
            {
                Id = user.Id,
                UserName = user.UserName
            };
            this.db.Players.Create(player);
            this.db.Save();
        }

        public Player FindPlayer(int id)
        {
            return this.db.Players.Get(id);
        }
    }
}