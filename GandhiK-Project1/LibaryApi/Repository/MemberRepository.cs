using LibaryApi.Data;
using LibaryApi.Models;
using LibaryApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibaryApi.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly LibraryContext _context;

        public MemberRepository(LibraryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Member>> GetAllAsync()
        {
            return await _context.Members.ToListAsync();
        }

        public async Task<Member?> GetByIdAsync(int id)
        {
            return await _context.Members.FindAsync(id);
        }

        public async Task<Member> AddAsync(Member member)
        {
            _context.Members.Add(member);
            await _context.SaveChangesAsync();
            return member;
        }

        public async Task<Member?> UpdateAsync(Member member)
        {
            var existing = await _context.Members.FindAsync(member.Id);
            if (existing == null) return null;

            _context.Entry(existing).CurrentValues.SetValues(member);
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.Members.FindAsync(id);
            if (existing == null) return false;

            _context.Members.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
