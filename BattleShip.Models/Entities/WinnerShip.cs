using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace BattleShip.Models.Entities
{
    public class WinnerShip
    {
        public int Id { get; set; }
        public int? InjuredCells { get; set; }
        public int Size { get; set; }

        public int StatisticsRecordId { get; set; }
        public StatisticsRecord StatisticsRecord { get; set; }
    }
}
