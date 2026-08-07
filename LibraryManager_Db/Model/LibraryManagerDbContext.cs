namespace LibraryManager_Db.Model;

public class LibraryManagerDbContext : DbContext
{   
    public DbSet<Book> Books { get; set; }
    
    public LibraryManagerDbContext()
    {
        
    }

    public LibraryManagerDbContext(DbContextOptions<LibraryManagerDbContext> options) : base(options)
    {
    }

}
