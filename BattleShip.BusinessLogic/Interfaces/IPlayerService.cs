using BattleShip.Models.Entities;

namespace BattleShip.BusinessLogic.Interfaces
{
    public interface IPlayerService
    {
        public void CreatePlayer(ApplicationUser user);

        Player FindPlayer(int id);
    }
}