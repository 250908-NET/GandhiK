using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LibaryApi.Data;

public class LibraryContextFactory : IDesignTimeDbContextFactory<LibraryContext>
{
    public LibraryContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<LibraryContext>();
        optionsBuilder.UseSqlServer("Server=localhost,1433;Database=LibraryDb;User Id=sa;Password=Passw0rd;TrustServerCertificate=True;");

        return new LibraryContext(optionsBuilder.Options);
    }
}
