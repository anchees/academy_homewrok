using Microsoft.EntityFrameworkCore;

using GameDb db = new GameDb();

db.Database.Migrate();

while (true)
{
    Console.Clear();
    Console.WriteLine("1 - Показать все игры");
    Console.WriteLine("2 - Добавить игру");
    Console.WriteLine("3 - Добавить студию");
    Console.WriteLine("4 - Количество одиночных игр");
    Console.WriteLine("5 - Количество многопользовательских игр");
    Console.WriteLine("6 - Топ-5 игр по стилю");
    Console.WriteLine("7 - Полная информация об игре");
    Console.WriteLine("8 - Удалить студию");
    Console.WriteLine("0 - Выход");

    Console.Write("Выбор: ");
    string choice = Console.ReadLine();

    Console.Clear();

    if (choice == "1")
    {
        var games = db.Games.Include(g => g.Studio).ToList();

        foreach (var game in games)
        {
            Console.WriteLine($"{game.Name} | {game.Style} | {game.Year} | {game.Studio.Name}");
        }
    }
    else if (choice == "2")
    {
        Console.Write("Название игры: ");
        string name = Console.ReadLine();

        bool exists = db.Games.Any(g => g.Name == name);

        if (exists)
        {
            Console.WriteLine("Такая игра уже есть.");
        }
        else
        {
            Console.Write("Стиль: ");
            string style = Console.ReadLine();

            Console.Write("Год релиза: ");
            int year = Convert.ToInt32(Console.ReadLine());

            Console.Write("Продано копий: ");
            int sold = Convert.ToInt32(Console.ReadLine());

            Console.Write("Многопользовательская? true/false: ");
            bool multiplayer = Convert.ToBoolean(Console.ReadLine());

            Console.Write("Название студии: ");
            string studioName = Console.ReadLine();

            var studio = db.Studios.FirstOrDefault(s => s.Name == studioName);

            if (studio == null)
            {
                Console.WriteLine("Студия не найдена. Сначала добавьте студию.");
            }
            else
            {
                Game game = new Game
                {
                    Name = name,
                    Style = style,
                    Year = year,
                    SoldCopies = sold,
                    IsMultiplayer = multiplayer,
                    StudioId = studio.Id
                };

                db.Games.Add(game);
                db.SaveChanges();

                Console.WriteLine("Игра добавлена.");
            }
        }
    }
    else if (choice == "3")
    {
        Console.Write("Название студии: ");
        string name = Console.ReadLine();

        bool exists = db.Studios.Any(s => s.Name == name);

        if (exists)
        {
            Console.WriteLine("Такая студия уже есть.");
        }
        else
        {
            Console.Write("Страна: ");
            string country = Console.ReadLine();

            Console.Write("Город: ");
            string city = Console.ReadLine();

            Studio studio = new Studio
            {
                Name = name,
                Country = country,
                City = city
            };

            db.Studios.Add(studio);
            db.SaveChanges();

            Console.WriteLine("Студия добавлена.");
        }
    }
    else if (choice == "4")
    {
        int count = db.Games.Count(g => !g.IsMultiplayer);
        Console.WriteLine($"Количество однопользовательских игр: {count}");
    }
    else if (choice == "5")
    {
        int count = db.Games.Count(g => g.IsMultiplayer);
        Console.WriteLine($"Количество многопользовательских игр: {count}");
    }
    else if (choice == "6")
    {
        Console.Write("Введите стиль: ");
        string style = Console.ReadLine();

        var games = db.Games
            .Where(g => g.Style == style)
            .OrderByDescending(g => g.SoldCopies)
            .Take(5)
            .ToList();

        foreach (var game in games)
        {
            Console.WriteLine($"{game.Name} | {game.Style} | Продано: {game.SoldCopies}");
        }
    }
    else if (choice == "7")
    {
        Console.Write("Введите название игры: ");
        string name = Console.ReadLine();

        var game = db.Games
            .Include(g => g.Studio)
            .FirstOrDefault(g => g.Name == name);

        if (game == null)
        {
            Console.WriteLine("Игра не найдена.");
        }
        else
        {
            Console.WriteLine($"Название: {game.Name}");
            Console.WriteLine($"Стиль: {game.Style}");
            Console.WriteLine($"Год: {game.Year}");
            Console.WriteLine($"Продано: {game.SoldCopies}");
            Console.WriteLine($"Мультиплеер: {game.IsMultiplayer}");
            Console.WriteLine($"Студия: {game.Studio.Name}");
            Console.WriteLine($"Страна студии: {game.Studio.Country}");
            Console.WriteLine($"Город студии: {game.Studio.City}");
        }
    }
    else if (choice == "8")
    {
        Console.Write("Название студии: ");
        string name = Console.ReadLine();

        var studio = db.Studios.FirstOrDefault(s => s.Name == name);

        if (studio == null)
        {
            Console.WriteLine("Студия не найдена.");
        }
        else
        {
            Console.Write("Удалить студию? y/n: ");
            string answer = Console.ReadLine();

            if (answer == "y")
            {
                db.Studios.Remove(studio);
                db.SaveChanges();

                Console.WriteLine("Студия удалена.");
            }
        }
    }
    else if (choice == "0")
    {
        break;
    }

    Console.WriteLine("\nНажмите Enter...");
    Console.ReadLine();
}

class Game
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Style { get; set; }
    public int Year { get; set; }
    public int SoldCopies { get; set; }
    public bool IsMultiplayer { get; set; }

    public int StudioId { get; set; }
    public Studio Studio { get; set; }
}

class Studio
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Country { get; set; }
    public string City { get; set; }

    public List<Game> Games { get; set; } = new List<Game>();
}

class GameDb : DbContext
{
    public DbSet<Game> Games { get; set; }
    public DbSet<Studio> Studios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite("Data Source=games.db");
    }
}