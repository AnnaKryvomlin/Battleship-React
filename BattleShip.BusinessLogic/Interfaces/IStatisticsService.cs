namespace BattleShip.BusinessLogic.Interfaces
{
    using System.Collections.Generic;
    using BattleShip.BusinessLogic.Enums;
    using BattleShip.Models.Entities;

    public interface IStatisticsService
    {
        void AddFinishedGame(int gameId, int playerId);

        List<StatisticsRecord> GetStatisticsRecords(string name, bool onlyIntactShips, SortState sortOrder = SortState.NameAsc, FilterMoveState filterMoveState = FilterMoveState.All);
    }
}
