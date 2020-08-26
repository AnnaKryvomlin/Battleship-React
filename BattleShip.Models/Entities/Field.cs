namespace BattleShip.Models.Entities
{
    using System.Collections.Generic;

    public class Field
    {
        public int Id { get; set; }

        public int? GameId { get; set; }

        public Game Game { get; set; }

        public int PlayerId { get; set; }

        public Player Player { get; set; }

        public List<Coordinate> Coordinates { get; set; }

        public List<Ship> Ships { get; set; }
    }
}
