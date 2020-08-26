namespace BattleShip.Models.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Player
    {
        [Key]
        [ForeignKey("ApplicationUser")]
        public int Id { get; set; }

        public string UserName { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public List<Field> Fields { get; set; }

        public List<PlayerGame> PlayerGames { get; set; }

        public Player()
        {
            this.PlayerGames = new List<PlayerGame>();
        }
    }
}
