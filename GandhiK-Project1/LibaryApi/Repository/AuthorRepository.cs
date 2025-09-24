using LibaryApi.Data;
using LibaryApi.Models;
using LibaryApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibaryApi.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly LibraryContext _context;

        public AuthorRepository(LibraryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Author>> GetAllAsync()
        {
            return await _context.Authors.ToListAsync();
        }

        public async Task<Author?> GetByIdAsync(int id)
        {
            return await _context.Authors.FindAsync(id);
        }

        public async Task<Author> AddAsync(Author author)
        {
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
            return author;
        }

        public async Task<Author?> UpdateAsync(Author author)
        {
            var existing = await _context.Authors.FindAsync(author.Id);
            if (existing == null) return null;

            _context.Entry(existing).CurrentValues.SetValues(author);
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.Authors.FindAsync(id);
            if (existing == null) return false;

            _context.Authors.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
