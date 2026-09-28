using Dapper;
using HomeLibrary.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace HomeLibrary.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly string _connectionString;

        public BookRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("HomeLibrary")
                ?? throw new InvalidOperationException("Connection string 'HomeLibrary' not found.");
        }

        public async Task<int> CreateBookAsync(string title, int publicationYear, string? content, IEnumerable<int> authorIds)
        {
            await using var connection = new SqlConnection(_connectionString);

            var authorIdsTable = new DataTable();
            authorIdsTable.Columns.Add("Id", typeof(int));

            foreach (var authorId in authorIds)
                authorIdsTable.Rows.Add(authorId);

            var parameters = new DynamicParameters();

            parameters.Add("@Title", title);
            parameters.Add("@PublicationYear", publicationYear);
            parameters.Add("@Content", content);
            parameters.Add("@AuthorIds", authorIdsTable.AsTableValuedParameter("dbo.IdList"));

            var bookId = await connection.QuerySingleAsync<int>("dbo.CreateBook", parameters, commandType: CommandType.StoredProcedure);

            return bookId;
        }

        public async Task<IEnumerable<Book>> GetBooksAsync(string? search)
        {
            await using var connection = new SqlConnection(_connectionString);

            const string sql = """
                    SELECT
                        b.Id,
                        b.Title,
                        b.PublicationYear,
                        b.Content,
                        a.Id,
                        a.FirstName,
                        a.LastName,
                        a.MiddleName
                    FROM dbo.Books AS b
                    LEFT JOIN dbo.BookAuthors AS ba ON ba.BookId = b.Id
                    LEFT JOIN dbo.Authors AS a ON a.Id = ba.AuthorId
                    WHERE
                        @Search IS NULL

                        OR b.Title LIKE '%' + @Search + '%'

                        OR EXISTS
                        (
                            SELECT 1
                            FROM dbo.BookAuthors searchBa
                            JOIN dbo.Authors searchA
                                ON searchA.Id = searchBa.AuthorId
                            WHERE searchBa.BookId = b.Id
                              AND
                              (
                                  searchA.FirstName LIKE '%' + @Search + '%'
                                  OR searchA.LastName LIKE '%' + @Search + '%'
                                  OR searchA.MiddleName LIKE '%' + @Search + '%'
                              )
                        )

                        OR CONVERT(NVARCHAR(MAX), b.Content)
                            LIKE '%' + @Search + '%'

                    ORDER BY b.Id;
                    """;

            var books = new Dictionary<int, Book>();

            await connection.QueryAsync<Book, Author, Book>(
                sql,
                (book, author) =>
                {
                    if (!books.TryGetValue(book.Id, out var existingBook))
                    {
                        existingBook = book;
                        existingBook.Authors = [];
                        books.Add(book.Id, existingBook);
                    }

                    if (author != null)
                        existingBook.Authors.Add(author);

                    return existingBook;
                },
                new
                {
                    Search = string.IsNullOrWhiteSpace(search)
                        ? null
                        : search.Trim()
                },
                splitOn: "Id");

            return books.Values;
        }
        
        public async Task<Book?> GetBookByIdAsync(int id)
        {
            await using var connection = new SqlConnection(_connectionString);

            const string sql = """
                        SELECT
                            b.Id,
                            b.Title,
                            b.PublicationYear,
                            b.Content,
                            a.Id,
                            a.FirstName,
                            a.LastName,
                            a.MiddleName
                        FROM dbo.Books AS b
                        LEFT JOIN dbo.BookAuthors AS ba ON ba.BookId = b.Id
                        LEFT JOIN dbo.Authors AS a ON a.Id = ba.AuthorId
                        WHERE b.Id = @Id;
                        """;

            Book? result = null;

            await connection.QueryAsync<Book, Author, Book>(
                sql,
                (book, author) =>
                {
                    if (result == null)
                    {
                        result = book;
                        result.Authors = [];
                    }

                    if (author != null)
                        result.Authors.Add(author);

                    return result;
                },
                new { Id = id },
                splitOn: "Id");

            return result;
        }

        public async Task<bool> UpdateBookAsync(int id, string title, int publicationYear, string? content, IEnumerable<int> authorIds)
        {
            await using var connection = new SqlConnection(_connectionString);

            var authorIdsTable = new DataTable();
            authorIdsTable.Columns.Add("Id", typeof(int));

            foreach (var authorId in authorIds)
                authorIdsTable.Rows.Add(authorId);

            var parameters = new DynamicParameters();

            parameters.Add("@Id", id);
            parameters.Add("@Title", title);
            parameters.Add("@PublicationYear", publicationYear);
            parameters.Add("@Content", content);
            parameters.Add("@AuthorIds", authorIdsTable.AsTableValuedParameter("dbo.IdList"));

            return await connection.QuerySingleAsync<bool>("dbo.UpdateBook", parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            await using var connection = new SqlConnection(_connectionString);

            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            return await connection.QuerySingleAsync<bool>(
                "dbo.DeleteBook",
                parameters,
                commandType: CommandType.StoredProcedure);
        }
    }
}
