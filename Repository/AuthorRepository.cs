using Dapper;
using HomeLibrary.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace HomeLibrary.Repository
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly string _connectionString;

        public AuthorRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("HomeLibrary")
                ?? throw new InvalidOperationException(
                    "Connection string 'HomeLibrary' not found.");
        }

        public async Task<IEnumerable<int>> GetExistingIdsAsync(IEnumerable<int> authorIds)
        {
            await using var connection = new SqlConnection(_connectionString);

            var idsTable = new DataTable();
            idsTable.Columns.Add("Id", typeof(int));

            foreach (var id in authorIds)
                idsTable.Rows.Add(id);

            const string sql = """
                SELECT a.Id
                FROM dbo.Authors a
                JOIN @AuthorIds ids ON ids.Id = a.Id;
                """;

            var parameters = new DynamicParameters();

            parameters.Add("@AuthorIds", idsTable.AsTableValuedParameter("dbo.IdList"));

            return await connection.QueryAsync<int>(sql, parameters);
        }
        public async Task<IEnumerable<Author>> GetAuthorsAsync(string? search)
        {
            await using var connection = new SqlConnection(_connectionString);

            const string sql = """
                SELECT
                    Id,
                    FirstName,
                    LastName,
                    MiddleName
                FROM dbo.Authors
                WHERE
                    @Search IS NULL
                    OR FirstName LIKE '%' + @Search + '%'
                    OR LastName LIKE '%' + @Search + '%'
                    OR MiddleName LIKE '%' + @Search + '%'
                ORDER BY LastName, FirstName, MiddleName;
                """;

            return await connection.QueryAsync<Author>(sql,
                new
                {
                    Search = string.IsNullOrWhiteSpace(search) ? null : search.Trim()
                });
        }

        public async Task<int> CreateAuthorAsync(string firstName, string lastName, string? middleName)
        {
            await using var connection = new SqlConnection(_connectionString);

            const string sql = """
                INSERT INTO dbo.Authors
                    (FirstName, LastName, MiddleName)
                VALUES
                    (@FirstName, @LastName, @MiddleName);

                SELECT CONVERT(INT, SCOPE_IDENTITY());
                """;

            return await connection.QuerySingleAsync<int>(sql,
                new
                {
                    FirstName = firstName,
                    LastName = lastName,
                    MiddleName = middleName
                });
        }
    }
}