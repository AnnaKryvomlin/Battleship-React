namespace BattleShip.BusinessLogic.Interfaces
{
    using BattleShip.Models.Entities;

    public interface IPlayerService
    {
        public void CreatePlayer(ApplicationUser user);

        Player FindPlayer(int id);
    }
}
