using System.Collections.Generic;

namespace BattleShip.Models.Entities
{
    public class StatisticsRecord
    {
        public int Id { get; set; }

        public string Winner { get; set; }

        public int MoveCount { get; set; }

        public List<WinnerShip> WinnerShips { get; set; }
    }
}