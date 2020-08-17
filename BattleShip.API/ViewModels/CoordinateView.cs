using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattleShip.API.ViewModels
{
    public class CoordinateView
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool Mark { get; set; }
        public bool HaveShip { get; set; }
    }
}
