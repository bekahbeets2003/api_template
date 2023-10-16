using api_template.Utilities;
using Dapper;
using api_template.Models;
using System.Data.SqlClient;
using api_template.Interfaces;
using System.Data;

namespace api_template.DataRepo
{

    public class DapperDb : IDapperDb
    {
        private readonly String _connectionString;
        private readonly Utility _utility;
        private SemaphoreSlim _semaphore = new SemaphoreSlim(1);

        public DapperDb(String connectionString, Utility utility)
        {
            _connectionString = connectionString;
            _utility = utility;
        }

        public async Task<IEnumerable<Book>> GetBooksAsync()
        {
            await _semaphore.WaitAsync();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                if (conn.State != System.Data.ConnectionState.Open)
                {
                    conn.Open();
                }

                string sql = "exec GetAllBooks";
                var result = await conn.QueryAsync<Book>(sql);

                _semaphore.Release();

                return result;
            }
        }

        public async Task<Book> GetBookByIdAsync(int id)
        {
            await _semaphore.WaitAsync();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                DynamicParameters param = new DynamicParameters();
                param.Add("@p_book_id", id);

                string sproc = "GetBookById";
                var result = await conn.QueryAsync<Book>(sproc, param, null, null, CommandType.StoredProcedure);

                _semaphore.Release();

                return result.First();
            }
        }

        public async Task<Book> CreateBook(Book book)
        {
            await _semaphore.WaitAsync();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                DynamicParameters param = new DynamicParameters();
                param.Add("@p_author_id", book.author_id);
                param.Add("@p_book_name", book.book_name);
                param.Add("@p_book_description", book.book_description);
                param.Add("@p_gutenberg_id", book.gutenberg_id);

                string sproc = "CreateBook";
                var result = await conn.QueryAsync<Book>(sproc, param, null, null, CommandType.StoredProcedure);

                _semaphore.Release();

                return result.First(); //brings back created book
            }
        }

    }

}
