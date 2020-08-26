using System.Collections.Generic;
using BattleShip.BusinessLogic.Enums;
using BattleShip.Models.Entities;

namespace BattleShip.BusinessLogic.Interfaces
{
    public interface IStatisticsService
    {
        void AddFinishedGame(int gameId, int playerId);

        List<StatisticsRecord> GetStatisticsRecords(string name, bool onlyIntactShips, SortState sortOrder = SortState.NameAsc, FilterMoveState filterMoveState = FilterMoveState.All);
    }
}