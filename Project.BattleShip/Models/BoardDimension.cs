namespace Project.BattleShip.Models
{
    public class BoardDimension : IBoardDimension
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public BoardDimension(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}
