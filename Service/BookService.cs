using HomeLibrary.DTO;
using HomeLibrary.Exceptions;
using HomeLibrary.Models;
using HomeLibrary.Repository;
using System.Xml;
using System.Xml.Linq;

namespace HomeLibrary.Service
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;

        public BookService(IBookRepository bookRepository, IAuthorRepository authorRepository)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
        }

        async Task<int> IBookService.CreateBookAsync(CreateBookRequest request)
        {
            if (request.AuthorIds.Count != request.AuthorIds.Distinct().Count())
                throw new RequestValidationException("AuthorIds must not contain duplicates.");

            if (!string.IsNullOrWhiteSpace(request.Content))
            {
                try
                {
                    XDocument.Parse(request.Content);
                }
                catch (XmlException)
                {
                    throw new RequestValidationException("Content must contain valid XML.");
                }
            }

            var existingAuthorIds = (await _authorRepository.GetExistingIdsAsync(request.AuthorIds)).ToHashSet();

            var missingAuthorIds = request.AuthorIds.Where(id => !existingAuthorIds.Contains(id)).ToList();

            if (missingAuthorIds.Count > 0)
                throw new AuthorNotFoundException(missingAuthorIds);

            return await _bookRepository.CreateBookAsync(request.Title, request.PublicationYear, request.Content, request.AuthorIds);
        }

        public async Task<IEnumerable<Book>> GetBooksAsync(string? search)
        {
            return await _bookRepository.GetBooksAsync(search);
        }

        public async Task<Book> GetBookByIdAsync(int id)
        {
            var book = await _bookRepository.GetBookByIdAsync(id);

            if (book == null)
                throw new BookNotFoundException(id);

            return book;
        }

        public async Task UpdateBookAsync(int id, UpdateBookRequest request)
        {
            if (request.AuthorIds.Count != request.AuthorIds.Distinct().Count())
                throw new RequestValidationException("AuthorIds must not contain duplicates.");

            if (!string.IsNullOrWhiteSpace(request.Content))
            {
                try
                {
                    XDocument.Parse(request.Content);
                }
                catch (XmlException)
                {
                    throw new RequestValidationException("Content must contain valid XML.");
                }
            }

            var existingAuthorIds = (await _authorRepository.GetExistingIdsAsync(request.AuthorIds)).ToHashSet();

            var missingAuthorIds = request.AuthorIds.Where(id => !existingAuthorIds.Contains(id)).ToList();

            if (missingAuthorIds.Count > 0)
                throw new AuthorNotFoundException(missingAuthorIds);

            var updated = await _bookRepository.UpdateBookAsync(id, request.Title, request.PublicationYear, request.Content, request.AuthorIds);

            if (!updated)
                throw new BookNotFoundException(id);
        }

        public async Task DeleteBookAsync(int id)
        {
            var deleted = await _bookRepository.DeleteBookAsync(id);

            if (!deleted)
                throw new BookNotFoundException(id);
        }
    }
}
