using LibraryManager.Model;
using Microsoft.EntityFrameworkCore;

namespace LibraryManager.Data;

public class LibraryManagerDbContext : DbContext
{
    public DbSet<Book> Books => Set<Book>();

    public LibraryManagerDbContext(DbContextOptions<LibraryManagerDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>().ToTable("Books");
    }
}