namespace BattleShip.Models.Entities
{
    using System.Collections.Generic;

    public class StatisticsRecord
    {
        public int Id { get; set; }

        public string Winner { get; set; }

        public int MoveCount { get; set; }

        public List<WinnerShip> WinnerShips { get; set; }
    }
}
