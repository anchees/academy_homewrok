using Games.Data;
using Games.Models;

using GameDbContext db = new GameDbContext();

db.Database.EnsureCreated();

if (!db.Games.Any())
{
    db.Games.AddRange(
        new Game
        {
            Name = "Minecraft",
            Studio = "Mojang",
            Style = "Sandbox",
            ReleaseDate = new DateTime(2011, 11, 18),
            GameMode = "Многопользовательский",
            SoldCopies = 300000000
        },
        new Game
        {
            Name = "The Witcher 3",
            Studio = "CD Projekt Red",
            Style = "RPG",
            ReleaseDate = new DateTime(2015, 5, 19),
            GameMode = "Однопользовательский",
            SoldCopies = 50000000
        },
        new Game
        {
            Name = "Counter-Strike 2",
            Studio = "Valve",
            Style = "Shooter",
            ReleaseDate = new DateTime(2023, 9, 27),
            GameMode = "Многопользовательский",
            SoldCopies = 0
        }
    );

    db.SaveChanges();
}

var games = db.Games.ToList();

foreach (var game in games)
{
    Console.WriteLine($"{game.Id}. {game.Name}");
    Console.WriteLine($"Студия: {game.Studio}");
    Console.WriteLine($"Стиль: {game.Style}");
    Console.WriteLine($"Дата релиза: {game.ReleaseDate.ToShortDateString()}");
    Console.WriteLine($"Режим: {game.GameMode}");
    Console.WriteLine($"Продано копий: {game.SoldCopies}");
    Console.WriteLine();
}