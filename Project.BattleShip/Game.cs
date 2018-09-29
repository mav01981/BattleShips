namespace Project.BattleShip.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Game
    {
        public delegate void ShipEvent(string message);
        public event ShipEvent RaiseEvent;

        public List<Ship> Ships { get; }

        public Board board {  get;}

        public Game(IBoardDimension size) 
        {
            this.board = new Board(size);

            Ships = new List<Ship>();
            Ships.Add(ShipFactory.CreateShip(ShipFactory.ShipType.Battleship, "HMS Barham"));
            Ships.Add(ShipFactory.CreateShip(ShipFactory.ShipType.Destroyer, "Hobart-class destroyer"));
            Ships.Add(ShipFactory.CreateShip(ShipFactory.ShipType.Destroyer, "Kashin-class destroyer"));

            AddShipsToBoard(Ships, size.Width, size.Height);
        }

        public void AddShipsToBoard(IEnumerable<Ship> ships, int maxWidth, int maxHeight)
        {
            for (int index = 1; index <= ships.Count(); index++)
            {
                bool result = false;

                while (result == false)
                {
                    Random random = new Random();
                    int row = random.Next(0, 10);
                    int column = random.Next(this.board.axis.Length);

                    var location = new List<Square>();

                    var StartPoint = new Square()
                    {
                        Row = random.Next(1, 10),
                        Column = this.board.axis[column],
                        ShipIdentifier = index

                    };
                    Ships[index - 1].Id = index;
                    location.Add(StartPoint);

                    bool isVertical = random.NextDouble() >= 0.5;

                    for (int i = 1; i <= Ships[index - 1].Width - 1; i++)
                    {
                        if (isVertical)
                        {
                            location.Add(new Square()
                            {
                                Row = (StartPoint.Row + i),
                                Column = StartPoint.Column,
                                ShipIdentifier = i
                            });
                        }
                        else
                        {
                            location.Add(new Square()
                            {
                                Row = StartPoint.Row,
                                Column = this.board.axis[(this.board.axis.IndexOf(StartPoint.Column) + i) > maxWidth ? 1 : (this.board.axis.IndexOf(StartPoint.Column) + i)],
                                ShipIdentifier = i
                            });
                        }
                    }

                    Boardhelper service = new Boardhelper(location, this.board.coordinates);

                    if (service.IsValidLocation(Ships[index - 1].Width))
                    {
                        service.AddToBoard(Ships[index - 1].Id);
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
            var selectedPoint = this.board.coordinates.Where(x => x.Column == column && x.Row == row).FirstOrDefault();

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