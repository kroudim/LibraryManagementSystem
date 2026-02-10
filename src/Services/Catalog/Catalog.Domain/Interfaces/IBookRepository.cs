using Catalog.Domain.Entities;

namespace Catalog.Domain.Interfaces;

public interface IBookRepository
{
    Task<Book?> GetByIdAsync(Guid id);
    Task<IEnumerable<Book>> GetAllAsync();
    Task<IEnumerable<Book>> SearchByTitleAsync(string title);
    Task<Book> AddAsync(Book book);
    Task UpdateAsync(Book book);
    Task DeleteAsync(Guid id);
    Task<bool> IsbnExistsAsync(string isbn, Guid? excludeBookId = null);
}
