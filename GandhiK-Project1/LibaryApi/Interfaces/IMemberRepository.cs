using LibaryApi.Models;

namespace LibaryApi.Interfaces
{
    public interface IMemberRepository
    {
        Task<IEnumerable<Member>> GetAllAsync();
        Task<Member?> GetByIdAsync(int id);
        Task<Member> AddAsync(Member member);
        Task<Member?> UpdateAsync(Member member);
        Task<bool> DeleteAsync(int id);
    }
}
