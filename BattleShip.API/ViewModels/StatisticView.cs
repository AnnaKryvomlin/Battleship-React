using System.Collections.Generic;

namespace BattleShip.API.ViewModels
{
    public class StatisticView
    {
        public string Winner { get; set; }

        public int MoveCount { get; set; }

        public int WinnerShipsCount { get; set; }

        public List<StatisticsShipViewModel> WinnerShips { get; set; }
    }
}