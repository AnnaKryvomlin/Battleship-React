namespace BattleShip.BusinessLogic.Enums
{
    using System.ComponentModel.DataAnnotations;

    public enum FilterMoveState
    {
        [Display(Name = "Все")]
        All,
        [Display(Name = "<= 40")]
        Minimum,
        [Display(Name = "41-100")]
        Medium,
        [Display(Name = "101+")]
        Maximum,
    }
}
