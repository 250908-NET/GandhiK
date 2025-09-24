using LibaryApi.Data;
using LibaryApi.Models;
using LibaryApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibaryApi.Repositories
{
    public class LoanRepository : ILoanRepository
    {
        private readonly LibraryContext _context;

        public LoanRepository(LibraryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Loan>> GetAllAsync()
        {
            return await _context.Loans
                .Include(l => l.Book)
                .Include(l => l.Member)
                .ToListAsync();
        }

        public async Task<Loan?> GetByIdAsync(int id)
        {
            return await _context.Loans
                .Include(l => l.Book)
                .Include(l => l.Member)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<Loan> AddAsync(Loan loan)
        {
            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();
            return loan;
        }

        public async Task<Loan?> UpdateAsync(Loan loan)
        {
            var existing = await _context.Loans.FindAsync(loan.Id);
            if (existing == null) return null;

            _context.Entry(existing).CurrentValues.SetValues(loan);
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.Loans.FindAsync(id);
            if (existing == null) return false;

            _context.Loans.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
