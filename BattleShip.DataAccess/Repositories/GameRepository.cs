using System;
using System.Collections.Generic;
using BattleShip.DataAccess.EF;
using BattleShip.DataAccess.Interfaces;
using BattleShip.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BattleShip.DataAccess.Repositories
{
    public class GameRepository : IRepository<Game>
    {
        private readonly ApplicationDbContext db;

        public GameRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        public void Create(Game item)
        {
            this.db.Games.Add(item);
        }

        public void Delete(int id)
        {
            Game item = this.db.Games.Find(id);
            if (item == null)
            {
                throw new Exception("Item with this Id doesn't exist");
            }

            this.db.Games.Remove(item);
        }

        public Game Get(int id)
        {
            var game = this.db.Games.Find(id);
            return game;
        }

        public IEnumerable<Game> GetAll()
        {
            return this.db.Games;
        }

        public void Update(Game item)
        {
            this.db.Entry(item).State = EntityState.Modified;
        }
    }
}