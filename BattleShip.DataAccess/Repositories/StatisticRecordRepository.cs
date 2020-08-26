using System;
using System.Collections.Generic;
using BattleShip.DataAccess.EF;
using BattleShip.DataAccess.Interfaces;
using BattleShip.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BattleShip.DataAccess.Repositories
{
    public class StatisticRecordRepository : IRepository<StatisticsRecord>
    {
        private readonly ApplicationDbContext db;

        public StatisticRecordRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        public void Create(StatisticsRecord item)
        {
            this.db.StatisticsRecords.Add(item);
        }

        public void Delete(int id)
        {
            StatisticsRecord item = this.db.StatisticsRecords.Find(id);
            if (item == null)
            {
                throw new Exception("Item with this Id doesn't exist");
            }

            this.db.StatisticsRecords.Remove(item);
        }

        public StatisticsRecord Get(int id)
        {
            return this.db.StatisticsRecords.Find(id);
        }

        public IEnumerable<StatisticsRecord> GetAll()
        {
            return this.db.StatisticsRecords
                .Include(sr => sr.WinnerShips);
        }

        public void Update(StatisticsRecord item)
        {
            this.db.Entry(item).State = EntityState.Modified;
        }
    }
}