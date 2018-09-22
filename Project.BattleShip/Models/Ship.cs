namespace Project.BattleShip.Models
{
    public abstract class Ship
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual int Width { get; set; }
        public int Hits { get; set; }

        public bool IsSunk
        {
            get
            {
                return Hits >= Width;
            }
        }
    }
}
