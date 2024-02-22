using Microsoft.EntityFrameworkCore;

namespace ToDoGrpc;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    public DbSet<ToDoItem> ToDoItems => Set<ToDoItem>();
}