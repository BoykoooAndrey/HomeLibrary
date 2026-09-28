using HomeLibrary.DTO;
using HomeLibrary.Models;

namespace HomeLibrary.Service
{
    public interface IAuthorService
    {
        Task<IEnumerable<Author>> GetAuthorsAsync(string? search);

        Task<int> CreateAuthorAsync(CreateAuthorRequest request);
    }
}