namespace BattleShip.Models.Entities
{
    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser<int>
    {
        public Player Player { get; set; }
    }
}
