using Microsoft.EntityFrameworkCore;
using Catalog.Domain.Entities;
using Catalog.Domain.Interfaces;
using Catalog.Infrastructure.Data;

namespace Catalog.Infrastructure.Repositories;

public class BookRepository : IBookRepository
{
    private readonly CatalogDbContext _context;

    public BookRepository(CatalogDbContext context)
    {
        _context = context;
    }

    public async Task<Book?> GetByIdAsync(Guid id)
    {
        return await _context.Books
            .Include(b => b.Category)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        return await _context.Books
            .Include(b => b.Category)
            .ToListAsync();
    }

    public async Task<IEnumerable<Book>> SearchByTitleAsync(string title)
    {
        return await _context.Books
            .Include(b => b.Category)
            .Where(b => b.Title.Contains(title))
            .ToListAsync();
    }

    public async Task<Book> AddAsync(Book book)
    {
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        return book;
    }

    public async Task UpdateAsync(Book book)
    {
        _context.Books.Update(book);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book != null)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> IsbnExistsAsync(string isbn, Guid? excludeBookId = null)
    {
        return await _context.Books
            .AnyAsync(b => b.ISBN == isbn && (excludeBookId == null || b.Id != excludeBookId));
    }
}
