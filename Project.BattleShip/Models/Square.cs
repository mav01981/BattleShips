﻿using Project.BattleShip.Models;

namespace Project.BattleShip
{
    public class Square
    {
        public int Row { get; set; }
        public char Column { get; set; }
        public Status Occupied { get; set; }
        public int ShipIdentifier { get; set; }
    }
}