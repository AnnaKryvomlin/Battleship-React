using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattleShip.API.ViewModels
{
    public class GameForAccountViewModel
    {
        public int Id { get; set; }
        public int CurrentMovePlayerId { get; set; }
    }
}
