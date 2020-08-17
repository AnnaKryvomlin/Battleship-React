using BattleShip.BusinessLogic.Enums;
using BattleShip.BusinessLogic.Interfaces;
using BattleShip.DataAccess.Interfaces;
using BattleShip.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleShip.BusinessLogic.Services
{
    public class StatisticsService : IStatisticsService
    {
        private IUnitOfWork db;

        public StatisticsService(IUnitOfWork uow)
        {
            this.db = uow;
        }

        public void AddFinishedGame(int gameId, int playerId)
        {
            StatisticsRecord record = new StatisticsRecord();
            record.Winner = this.db.Players.Get(playerId).UserName;
            record.MoveCount = this.db.Moves.GetAll().Where(m => m.GameId == gameId).Count();
            this.db.StatisticsRecords.Create(record);
            this.db.Save();
            var ships = this.db.Ships.GetAll().Where(s => s.Field.GameId == gameId && s.Field.PlayerId == playerId).ToList();
            foreach (var ship in ships)
            {
                int injuredCells = ship.Coordinates.Where(c => c.Mark).Count();
                // if ship isn't killed add it to winner's ships
                if (injuredCells != ship.Size)
                {
                    WinnerShip winnerShip = new WinnerShip();
                    winnerShip.Size = ship.Size;
                    winnerShip.InjuredCells = injuredCells;
                    winnerShip.StatisticsRecordId = record.Id;
                    this.db.WinnerShips.Create(winnerShip);
                }
            }
            this.db.Save();
        }

        public List<StatisticsRecord> GetStatisticsRecords(string name, bool onlyIntactShips, SortState sortOrder, FilterMoveState filterMoveState)
        {
            var statisticsRecords = this.db.StatisticsRecords.GetAll();
            if (!String.IsNullOrEmpty(name))
            {
                statisticsRecords = statisticsRecords.Where(sr => sr.Winner == name);
            }

            if(onlyIntactShips)
            {
                // Find only games where ships don't contain injured cells
                statisticsRecords = statisticsRecords.Where(sr => sr.WinnerShips.Where(ws => ws.InjuredCells > 0).Count() == 0);
            }

            switch (filterMoveState)
            {
                case FilterMoveState.Minimum:
                    statisticsRecords = statisticsRecords.Where(sr => sr.MoveCount <= 40);
                    break;
                case FilterMoveState.Medium:
                    statisticsRecords = statisticsRecords.Where(sr => sr.MoveCount > 40 && sr.MoveCount <= 100);
                    break;
                case FilterMoveState.Maximum:
                    statisticsRecords = statisticsRecords.Where(sr => sr.MoveCount > 100);
                    break;
            }

            switch (sortOrder)
            {
                case SortState.NameAsc:
                    statisticsRecords = statisticsRecords.OrderBy(sr => sr.Winner);
                    break;
                case SortState.NameDesc:
                    statisticsRecords = statisticsRecords.OrderByDescending(sr => sr.Winner);
                    break;
                case SortState.MoveCountAsc:
                    statisticsRecords = statisticsRecords.OrderBy(sr => sr.MoveCount);
                    break;
                case SortState.MoveCountDesc:
                    statisticsRecords = statisticsRecords.OrderByDescending(sr => sr.MoveCount);
                    break;
                case SortState.ShipCountAsc:
                    statisticsRecords = statisticsRecords.OrderBy(sr=>sr.WinnerShips.Count());
                    break;
                case SortState.ShipCountDesc:
                    statisticsRecords = statisticsRecords.OrderByDescending(sr => sr.WinnerShips.Count());
                    break;
            }

            return statisticsRecords.ToList();
        }
    }
}
