namespace Project.BattleShip.Models
{
    using System;
    using System.Collections.Generic;
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

                    var location = new List<Square>();

                    var StartPoint = new Square()
                    {
                        Row = random.Next(1, 10),
                        Column = this.axis[column]
                    };
                    location.Add(StartPoint);

                    bool isVertical = random.NextDouble() >= 0.5;

                    for (int i = 1; i <= ship.Width-1; i++)
                    {
                        if (isVertical)
                        {
                            location.Add(new Square()
                            {
                                Row = (StartPoint.Row + i),
                                Column = StartPoint.Column
                            });
                        }
                        else
                        {
                            location.Add(new Square()
                            {
                                Row = StartPoint.Row,
                                Column = this.axis[(this.axis.IndexOf(StartPoint.Column) + i) > maxWidth ? 1 : (this.axis.IndexOf(StartPoint.Column) + i)]
                            });
                        }
                    }

                    ShipService service = new ShipService(location, this.coordinates);

                    if (service.IsValidLocation(ship.Width))
                    {
                        service.AddToBoard(ship.Id);
                    }
                    else
                    {
                        continue;
                    }

                    result = true;
                }
            }
        }

        public Shot TakeShot(char column, int row)
        {
            var selectedPoint = this.coordinates.Where(x => x.Column == column && x.Row == row).FirstOrDefault();

            if (selectedPoint.ShipIdentifier > 0 & selectedPoint.Occupied != Status.Hit)
            {
                var selectedShip = Ships.Where(x => x.Id == selectedPoint.ShipIdentifier).FirstOrDefault();

                if (selectedShip.Width > selectedShip.Hits)
                    selectedShip.Hits++;

                if (selectedShip.IsSunk) RaiseEvent($"{selectedShip.Name} has been sunk.!");
                if (Ships.Where(x => x.IsSunk == false).Count() == 0) RaiseEvent($"Player 1 Wins. !");
            }

            selectedPoint.Occupied = Status.Hit;

            return Ships.Where(x => x.IsSunk == false).Count() == 0 ? Shot.Wins :
                Ships.Where(x => x.Id == selectedPoint.ShipIdentifier && x.IsSunk == true).Any() ? Shot.Sinks : selectedPoint.ShipIdentifier > 0 ? Shot.Hit : Shot.Miss;
        }
    }
}