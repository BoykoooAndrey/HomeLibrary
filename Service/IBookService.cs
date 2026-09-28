using HomeLibrary.DTO;
using HomeLibrary.Models;

namespace HomeLibrary.Service
{
    public interface IBookService
    {
        Task<int> CreateBookAsync(CreateBookRequest request);
        Task<IEnumerable<Book>> GetBooksAsync(string? search);
        Task<Book> GetBookByIdAsync(int id);
        Task UpdateBookAsync(int id, UpdateBookRequest request);
        Task DeleteBookAsync(int id);
    }
}
