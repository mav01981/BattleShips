﻿using Project.BattleShip.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Project.BattleShip
{
    public class ShipService
    {
        private List<Square> compare;
        private List<Square> compareTo;

        public ShipService(List<Square> compare, List<Square> compareTo)
        {
            this.compare = compare;
            this.compareTo = compareTo;
        }

        public bool IsValidLocation(int width)
        {
            return this.compareTo
                 .Join(
                 this.compare,
                 a => new { a.Column, a.Row },
                 b => new { b.Column, b.Row },
                 (a, b) => a).Where(x => x.Occupied == Models.Status.None)
                       .ToList()
                       .Count() == width;
        }

        public void AddToBoard(int id)
        {
            this.compareTo
                 .Join(
                 this.compare,
                 a => new { a.Column, a.Row },
                 b => new { b.Column, b.Row },
                 (a, b) => a).Where(x => x.Occupied == Models.Status.None)
                 .ToList()
                     .ForEach(x =>
                     {
                         x.Occupied = Status.Yes;
                         x.ShipIdentifier = id;
                     });
        }

    }
}
