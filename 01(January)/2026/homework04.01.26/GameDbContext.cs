using Games.Models;
using Microsoft.EntityFrameworkCore;

namespace Games.Data;

public class GameDbContext : DbContext
{
    public DbSet<Game> Games { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite("Data Source=games.db");
    }
}