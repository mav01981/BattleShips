using Project.BattleShip.Models;
using System.Collections.Generic;

namespace Project.BattleShip
{
    public class Board
    {
        public List<Coordinates> coordinates { get; set; }
        public string axis => "ABCDEFGHIJK";

        public Board(IBoardDimension size)
        {
            coordinates = new List<Coordinates>();

            for (int i = 1; i <= size.Height; i++)
            {
                for (int j = 0; j < axis.Length-1 ; j++)
                {
                    coordinates.Add(new Coordinates() { Column = axis[j], Row = i });
                }
            }
        }
    }
}
