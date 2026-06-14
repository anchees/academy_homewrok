using Microsoft.EntityFrameworkCore;

namespace GamesApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using GameDbContext db = new GameDbContext();

            while (true)
            {
                Console.Clear();

                Console.WriteLine("1 - Поиск игры по названию");
                Console.WriteLine("2 - Поиск игр по студии");
                Console.WriteLine("3 - Поиск по игре и студии");
                Console.WriteLine("4 - Поиск по стилю");
                Console.WriteLine("5 - Поиск по году релиза");
                Console.WriteLine("6 - Все однопользовательские игры");
                Console.WriteLine("7 - Все многопользовательские игры");
                Console.WriteLine("8 - Игра с максимальными продажами");
                Console.WriteLine("9 - Игра с минимальными продажами");
                Console.WriteLine("10 - Топ-3 продаваемых игр");
                Console.WriteLine("11 - Топ-3 непродаваемых игр");
                Console.WriteLine("12 - Добавить игру");
                Console.WriteLine("13 - Изменить игру");
                Console.WriteLine("14 - Удалить игру");
                Console.WriteLine("0 - Выход");

                Console.Write("\nВыберите пункт: ");
                int choice = Convert.ToInt32(Console.ReadLine());

                Console.Clear();

                switch (choice)
                {
                    case 1:
                    {
                        Console.Write("Название игры: ");
                        string title = Console.ReadLine();

                        var games = db.Games
                            .Where(g => g.Title.Contains(title))
                            .ToList();

                        ShowGames(games);
                        break;
                    }

                    case 2:
                    {
                        Console.Write("Название студии: ");
                        string studio = Console.ReadLine();

                        var games = db.Games
                            .Where(g => g.Studio.Contains(studio))
                            .ToList();

                        ShowGames(games);
                        break;
                    }

                    case 3:
                    {
                        Console.Write("Название игры: ");
                        string title = Console.ReadLine();

                        Console.Write("Название студии: ");
                        string studio = Console.ReadLine();

                        var games = db.Games
                            .Where(g => g.Title.Contains(title)
                                     && g.Studio.Contains(studio))
                            .ToList();

                        ShowGames(games);
                        break;
                    }

                    case 4:
                    {
                        Console.Write("Стиль: ");
                        string style = Console.ReadLine();

                        var games = db.Games
                            .Where(g => g.Style.Contains(style))
                            .ToList();

                        ShowGames(games);
                        break;
                    }

                    case 5:
                    {
                        Console.Write("Год релиза: ");
                        int year = Convert.ToInt32(Console.ReadLine());

                        var games = db.Games
                            .Where(g => g.ReleaseYear == year)
                            .ToList();

                        ShowGames(games);
                        break;
                    }

                    case 6:
                    {
                        var games = db.Games
                            .Where(g => !g.IsMultiplayer)
                            .ToList();

                        ShowGames(games);
                        break;
                    }

                    case 7:
                    {
                        var games = db.Games
                            .Where(g => g.IsMultiplayer)
                            .ToList();

                        ShowGames(games);
                        break;
                    }

                    case 8:
                    {
                        var game = db.Games
                            .OrderByDescending(g => g.SoldCopies)
                            .FirstOrDefault();

                        if (game != null)
                            ShowGame(game);

                        break;
                    }

                    case 9:
                    {
                        var game = db.Games
                            .OrderBy(g => g.SoldCopies)
                            .FirstOrDefault();

                        if (game != null)
                            ShowGame(game);

                        break;
                    }

                    case 10:
                    {
                        var games = db.Games
                            .OrderByDescending(g => g.SoldCopies)
                            .Take(3)
                            .ToList();

                        ShowGames(games);
                        break;
                    }

                    case 11:
                    {
                        var games = db.Games
                            .OrderBy(g => g.SoldCopies)
                            .Take(3)
                            .ToList();

                        ShowGames(games);
                        break;
                    }

                    case 12:
                    {
                        Console.Write("Название: ");
                        string title = Console.ReadLine();

                        Console.Write("Студия: ");
                        string studio = Console.ReadLine();

                        bool exists = db.Games.Any(g =>
                            g.Title == title &&
                            g.Studio == studio);

                        if (exists)
                        {
                            Console.WriteLine("Такая игра уже существует.");
                            break;
                        }

                        Console.Write("Стиль: ");
                        string style = Console.ReadLine();

                        Console.Write("Год релиза: ");
                        int year = Convert.ToInt32(Console.ReadLine());

                        Console.Write("Многопользовательская (true/false): ");
                        bool multiplayer = Convert.ToBoolean(Console.ReadLine());

                        Console.Write("Количество продаж: ");
                        int sold = Convert.ToInt32(Console.ReadLine());

                        Game game = new()
                        {
                            Title = title,
                            Studio = studio,
                            Style = style,
                            ReleaseYear = year,
                            IsMultiplayer = multiplayer,
                            SoldCopies = sold
                        };

                        db.Games.Add(game);
                        db.SaveChanges();

                        Console.WriteLine("Игра добавлена.");
                        break;
                    }

                    case 13:
                    {
                        Console.Write("Введите название игры: ");
                        string title = Console.ReadLine();

                        var game = db.Games
                            .FirstOrDefault(g => g.Title == title);

                        if (game == null)
                        {
                            Console.WriteLine("Игра не найдена.");
                            break;
                        }

                        Console.Write("Новое название: ");
                        game.Title = Console.ReadLine();

                        Console.Write("Новая студия: ");
                        game.Studio = Console.ReadLine();

                        Console.Write("Новый стиль: ");
                        game.Style = Console.ReadLine();

                        Console.Write("Новый год релиза: ");
                        game.ReleaseYear = Convert.ToInt32(Console.ReadLine());

                        Console.Write("Многопользовательская (true/false): ");
                        game.IsMultiplayer = Convert.ToBoolean(Console.ReadLine());

                        Console.Write("Количество продаж: ");
                        game.SoldCopies = Convert.ToInt32(Console.ReadLine());

                        db.SaveChanges();

                        Console.WriteLine("Данные обновлены.");
                        break;
                    }

                    case 14:
                    {
                        Console.Write("Название игры: ");
                        string title = Console.ReadLine();

                        Console.Write("Студия: ");
                        string studio = Console.ReadLine();

                        var game = db.Games.FirstOrDefault(g =>
                            g.Title == title &&
                            g.Studio == studio);

                        if (game == null)
                        {
                            Console.WriteLine("Игра не найдена.");
                            break;
                        }

                        Console.Write("Удалить игру? (y/n): ");
                        string answer = Console.ReadLine();

                        if (answer.ToLower() == "y")
                        {
                            db.Games.Remove(game);
                            db.SaveChanges();

                            Console.WriteLine("Игра удалена.");
                        }

                        break;
                    }

                    case 0:
                        return;
                }

                Console.WriteLine("\nНажмите Enter...");
                Console.ReadLine();
            }
        }

        static void ShowGame(Game game)
        {
            Console.WriteLine(
                $"{game.Title} | {game.Studio} | {game.Style} | " +
                $"{game.ReleaseYear} | Продано: {game.SoldCopies}");
        }

        static void ShowGames(List<Game> games)
        {
            foreach (var game in games)
            {
                ShowGame(game);
            }
        }
    }
}