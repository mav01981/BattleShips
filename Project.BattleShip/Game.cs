namespace Project.BattleShip.Models
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    public class Game : Board, IGame
    {
        public delegate void ShipEvent(string message);
        public event ShipEvent RaiseEvent;

        public List<Ship> Ships { get; }

        public Game(IBoardDimension size) : base(size)
        {
            Ships = new List<Ship>();
            Ships.Add(new BattleShip() { Id = 1, Name = "HMSBattleship", Width = 5 });
            Ships.Add(new Destroyer() { Id = 2, Name = "HMSDestroyer", Width = 4 });
            Ships.Add(new Destroyer() { Id = 3, Name = "HMSDestroyer1", Width = 4 });

            AddShipsToBoard(Ships, size.Width, size.Height);
        }

        public void AddShipsToBoard(IEnumerable<Ship> ships, int maxWidth, int maxHeight)
        {
            foreach (var ship in ships)
            {
                bool result = false;

                while (result == false)
                {
                    Random random = new Random();
                    int row = random.Next(0, 10);
                    int column = random.Next(this.axis.Length);
                    bool isVertical = random.Next(100) % 2 == 0; //1 for Horizontal

                    if (this.coordinates.Where(x => x.Row == row && x.Column == this.axis[column] && x.Occupied == Status.None).Any())
                    {
                        if (isVertical)
                        {
                            if (this.coordinates.Where(x => x.Column == this.axis[column] && x.Row >= row && x.Row <= row - (ship.Width - 1) && x.Occupied == Status.None).Count() == ship.Width)
                            {
                                this.coordinates
                                    .Where(x => x.Column == this.axis[column] && x.Row >= row && x.Row <= row - (ship.Width - 1) && x.Occupied == Status.None)
                                        .ToList()
                                         .ForEach(x =>
                                         {
                                             x.Occupied = Status.Yes;
                                             x.ShipIdentifier = ship.Id;
                                         });
                            }
                            else if (this.coordinates.Where(x => x.Column == this.axis[column] && x.Row >= row && x.Row <= row + (ship.Width - 1) && x.Occupied == Status.None).Count() == ship.Width)
                            {
                                this.coordinates
                                   .Where(x => x.Column == this.axis[column] && x.Row >= row && x.Row <= row + (ship.Width - 1) && x.Occupied == Status.None)
                                   .ToList()
                                    .ForEach(x =>
                                    {
                                        x.Occupied = Status.Yes;
                                        x.ShipIdentifier = ship.Id;
                                    });
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            var index = (column + (ship.Width - 1)) > maxWidth ? 1 : (column + (ship.Width - 1)) - 1;

                            if (this.coordinates.Where(x => x.Column >= this.axis[column] && x.Column <= this.axis[index] && x.Row == row && x.Occupied == Status.None).Count() == ship.Width)
                            {
                                this.coordinates.Where(x => x.Column >= this.axis[column] && x.Column <= this.axis[index] && x.Row == row && x.Occupied == Status.None)
                                        .ToList()
                                       .ForEach(x =>
                                       {
                                           x.Occupied = Status.Yes;
                                           x.ShipIdentifier = ship.Id;
                                       });
                            }
                            else if (this.coordinates.Where(x => x.Column >= this.axis[index] && x.Column <= this.axis[column] && x.Row == row && x.Occupied == Status.None).Count() == ship.Width)
                            {
                                this.coordinates
                                      .Where(x => x.Column >= this.axis[index] && x.Column <= this.axis[column] && x.Row == row && x.Occupied == Status.None)
                                        .ToList()
                                       .ForEach(x =>
                                       {
                                           x.Occupied = Status.Yes;
                                           x.ShipIdentifier = ship.Id;
                                       });
                            }
                            else
                            {
                                continue;
                            }
                        }

                        result = true;
                    }
                }
            }
        }

        public Shot TakeShot(char column, int row)
        {
            var selectedPoint = this.coordinates.Where(x => x.Column == column && x.Row == row).FirstOrDefault();

            if (selectedPoint.Occupied == Status.Yes  && selectedPoint.ShipIdentifier > 0)
            {
                var selectedShip = Ships.Where(x => x.Id == selectedPoint.ShipIdentifier).FirstOrDefault();

                if (selectedShip != null)
                    selectedShip.Hits++;

                if (selectedShip.IsSunk) RaiseEvent($"{selectedShip.Name} has been sunk.!");
                if (Ships.Where(x => x.IsSunk == false).Count() == 0) RaiseEvent($"Player 1 Wins.!");
            }

            selectedPoint.Occupied = Status.Hit;

            return  selectedPoint.ShipIdentifier > 0 ? Shot.Hit : Shot.Miss;
        }
    }
}