using BattleShip.DataAccess.EF;
using BattleShip.DataAccess.Interfaces;
using BattleShip.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BattleShip.DataAccess.Repositories
{
    public class WinnerShipRepository : IRepository<WinnerShip>
    {
        private ApplicationDbContext db;

        public WinnerShipRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        public void Create(WinnerShip item)
        {
            this.db.WinnerShips.Add(item);
        }

        public void Delete(int id)
        {
            WinnerShip item = this.db.WinnerShips.Find(id);
            if (item == null)
                throw new Exception("Item with this Id doesn't exist");
            this.db.WinnerShips.Remove(item);
        }

        public WinnerShip Get(int id)
        {
            return this.db.WinnerShips.Find(id);
        }

        public IEnumerable<WinnerShip> GetAll()
        {
            return this.db.WinnerShips;
        }

        public void Update(WinnerShip item)
        {
            this.db.Entry(item).State = EntityState.Modified;
        }
    }
}
