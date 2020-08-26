namespace BattleShip.Models.Entities
{
    using System.Collections.Generic;

    public class Ship
    {
        public int Id { get; set; }

        public int Size { get; set; }

        public List<Coordinate> Coordinates { get; set; }

        public int? FieldId { get; set; }

        public Field Field { get; set; }
    }
}
