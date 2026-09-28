using HomeLibrary.Models;

namespace HomeLibrary.Repository
{
    public interface IAuthorRepository
    {
        Task<IEnumerable<int>> GetExistingIdsAsync(IEnumerable<int> authorIds);
        Task<IEnumerable<Author>> GetAuthorsAsync(string? search);
        Task<int> CreateAuthorAsync(string firstName, string lastName, string? middleName);
    }
}