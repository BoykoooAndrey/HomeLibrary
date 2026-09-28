using HomeLibrary.DTO;
using HomeLibrary.Models;
using HomeLibrary.Repository;

namespace HomeLibrary.Service
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorService(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<IEnumerable<Author>> GetAuthorsAsync(string? search)
        {
            return await _authorRepository.GetAuthorsAsync(search);
        }

        public async Task<int> CreateAuthorAsync(CreateAuthorRequest request)
        {
            return await _authorRepository.CreateAuthorAsync(request.FirstName.Trim(), request.LastName.Trim(), request.MiddleName?.Trim());
        }
    }
}