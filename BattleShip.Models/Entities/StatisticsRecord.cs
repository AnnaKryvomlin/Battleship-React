using System;
using System.Collections.Generic;
using System.Text;

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
