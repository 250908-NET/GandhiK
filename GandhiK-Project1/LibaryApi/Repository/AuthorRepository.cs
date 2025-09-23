using LibraryApi.Data;
using LibraryApi.Interfaces;
using LibraryApi.Models;

namespace LibaryApi.Repository;

public class AuthorRepository : IAuthorRepository
{
    private readonly LibraryContext _context;

    public AuthorRepository(LibraryContext context)
    {
        _context = context;
    }

    public async Task<Author> AddAuthorAsync(Author author)
    {
        _context.Authors.Add(author);
        await _context.SaveChangesAsync();
        return author;
    }

    public async Task<List<Author>> GetAuthorsAsync() =>
        await _context.Authors.ToListAsync();
}