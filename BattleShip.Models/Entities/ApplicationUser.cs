using Microsoft.AspNetCore.Identity;

namespace BattleShip.Models.Entities
{
    public class ApplicationUser : IdentityUser<int>
    {
        public Player Player { get; set; }
    }
}