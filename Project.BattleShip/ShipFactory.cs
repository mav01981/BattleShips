public class ShipFactory
{
    public enum ShipType
    {
        Battleship,
        Destroyer
    }

    public static Ship CreateShip(ShipType type, string shipName)
    {
        Ship ship = null;

        switch (type)
        {
            case ShipType.Battleship:
                return ship = new Ship()
                {
                    Name = shipName,
                    Width = 5,
                    Hits = 0,
                };
            case ShipType.Destroyer:
                return ship = new Ship()
                {
                    Name = shipName,
                    Width = 4,
                    Hits = 0,
                };
        }

        return ship;
    }
}

