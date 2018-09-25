
public class Ship
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Width { get; set; }
    public int Hits { get; set; }

    public bool IsSunk
    {
        get
        {
            return Hits >= Width;
        }
    }
}

