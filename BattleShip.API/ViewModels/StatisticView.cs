namespace BattleShip.API.ViewModels
{
    using System.Collections.Generic;

    public class StatisticView
    {
        public string Winner { get; set; }

        public int MoveCount { get; set; }

        public int WinnerShipsCount { get; set; }

        public List<StatisticsShipViewModel> WinnerShips { get; set; }
    }
}
