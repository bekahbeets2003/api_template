using api_template.Models;

namespace api_template.Interfaces
{
    public interface IDapperDb
    {
        Task<Book> CreateBook(Book book);
        Task<Book> GetBookByIdAsync(int id);
        Task<IEnumerable<Book>> GetBooksAsync();
    }
}