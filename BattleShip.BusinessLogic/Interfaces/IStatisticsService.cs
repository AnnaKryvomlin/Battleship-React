using BattleShip.BusinessLogic.Enums;
using BattleShip.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BattleShip.BusinessLogic.Interfaces
{
    public interface IStatisticsService
    {
        void AddFinishedGame(int gameId, int playerId);
        List<StatisticsRecord> GetStatisticsRecords(string name, bool onlyIntactShips, SortState sortOrder = SortState.NameAsc, FilterMoveState filterMoveState = FilterMoveState.All);
    }
}
