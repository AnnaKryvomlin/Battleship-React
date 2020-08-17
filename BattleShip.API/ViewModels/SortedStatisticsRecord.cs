using BattleShip.BusinessLogic.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattleShip.API.ViewModels
{
    public class SortedStatisticsRecord
    {
        public SortState NameSort { get; private set; }
        public SortState MoveCountSort { get; private set; }
        public SortState ShipCountSort { get; private set; }
        public SortState Current { get; private set; }

        public SortedStatisticsRecord(SortState sortOrder)
        {
            this.NameSort = sortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            this.MoveCountSort = sortOrder == SortState.MoveCountAsc ? SortState.MoveCountDesc : SortState.MoveCountAsc;
            this.ShipCountSort = sortOrder == SortState.ShipCountAsc ? SortState.ShipCountDesc : SortState.ShipCountAsc;
            this.Current = sortOrder;
        }
    }
}
