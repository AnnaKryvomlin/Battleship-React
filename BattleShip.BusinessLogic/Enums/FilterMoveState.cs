using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BattleShip.BusinessLogic.Enums
{
    public enum FilterMoveState
    {
        [Display(Name = "Все")]
        All,
        [Display(Name = "<= 40")]
        Minimum,
        [Display(Name = "41-100")]
        Medium,
        [Display(Name = "101+")]
        Maximum
    }
}
