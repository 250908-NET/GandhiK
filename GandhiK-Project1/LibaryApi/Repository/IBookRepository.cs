using LibaryApi.Models;

namespace LibaryApi.Repositories;

public interface IBookRepository
{
    Task<Book> AddBookAsync(Book book);
    Task<List<Book>> GetBookAsync();
}