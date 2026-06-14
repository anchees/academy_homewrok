namespace Games.Models;

public class Game
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Studio { get; set; }

    public string Style { get; set; }

    public DateTime ReleaseDate { get; set; }

    public string GameMode { get; set; }

    public int SoldCopies { get; set; }
}