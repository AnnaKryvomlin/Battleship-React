﻿namespace BattleShip.BusinessLogic.Services
{
    using BattleShip.BusinessLogic.Interfaces;
    using BattleShip.DataAccess.Interfaces;
    using BattleShip.Models.Entities;

    public class PlayerService : IPlayerService
    {
        private IUnitOfWork db;

        public PlayerService(IUnitOfWork uow)
        {
            this.db = uow;
        }

        public void CreatePlayer(ApplicationUser user)
        {
            Player player = new Player();
            player.Id = user.Id;
            player.UserName = user.UserName;
            this.db.Players.Create(player);
            this.db.Save();
        }

        public Player FindPlayer(int id)
        {
            return this.db.Players.Get(id);
        }
    }
}