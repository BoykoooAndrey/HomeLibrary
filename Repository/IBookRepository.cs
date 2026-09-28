using HomeLibrary.Models;

namespace HomeLibrary.Repository
{
    public interface IBookRepository
    {
        Task<int> CreateBookAsync(string title, int publicationYear, string? content, IEnumerable<int> authorIds);
        Task<IEnumerable<Book>> GetBooksAsync(string? search);

        Task<Book?> GetBookByIdAsync(int id);

        Task<bool> UpdateBookAsync(int id, string title, int publicationYear, string? content, IEnumerable<int> authorIds);

        Task<bool> DeleteBookAsync(int id);
    }
}
