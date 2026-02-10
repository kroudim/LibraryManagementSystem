using Catalog.Application.DTOs;
using Catalog.Domain.Entities;
using Catalog.Domain.Interfaces;
using System.Text.Json;
using Library.Shared.Events;

namespace Catalog.Application.Services;

public class BookService
{
    private readonly IBookRepository _bookRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IEventPublisher _eventPublisher;

    public BookService(IBookRepository bookRepository, ICategoryRepository categoryRepository, IEventPublisher eventPublisher)
    {
        _bookRepository = bookRepository;
        _categoryRepository = categoryRepository;
        _eventPublisher = eventPublisher;
    }

    public async Task<BookDto> GetByIdAsync(Guid id)
    {
        var book = await _bookRepository.GetByIdAsync(id);
        if (book == null)
            throw new KeyNotFoundException($"Book with ID {id} not found");
        
        return MapToDto(book);
    }

    public async Task<IEnumerable<BookDto>> GetAllAsync()
    {
        var books = await _bookRepository.GetAllAsync();
        return books.Select(MapToDto);
    }

    public async Task<IEnumerable<BookDto>> SearchByTitleAsync(string title)
    {
        var books = await _bookRepository.SearchByTitleAsync(title);
        return books.Select(MapToDto);
    }

    public async Task<BookDto> CreateAsync(CreateBookDto dto)
    {
        if (await _bookRepository.IsbnExistsAsync(dto.ISBN))
            throw new InvalidOperationException($"ISBN {dto.ISBN} already exists");

        var category = await _categoryRepository.GetByIdAsync(dto.CategoryId);
        if (category == null)
            throw new KeyNotFoundException($"Category with ID {dto.CategoryId} not found");

        var book = new Book
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            ISBN = dto.ISBN,
            AuthorPartyId = dto.AuthorPartyId,
            CategoryId = dto.CategoryId,
            TotalCopies = dto.TotalCopies,
            AvailableCopies = dto.TotalCopies,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        book = await _bookRepository.AddAsync(book);

        book.Category = category;

        // Serialize a DTO (not the EF entity) to avoid cycles
        var payloadDto = MapToDto(book);

        await _eventPublisher.PublishAsync(new BookCreated
        {
            BookId = book.Id,
            Title = book.Title,
            ISBN = book.ISBN,
            AuthorPartyId = book.AuthorPartyId,
            CategoryId = book.CategoryId,
            TotalCopies = book.TotalCopies,
            AvailableCopies = book.AvailableCopies,
            EntityId = book.Id.ToString(),
            EntityType = "Book",
            ActionType = "Created",
            Payload = JsonSerializer.Serialize(payloadDto)
        });

        book.Category = category;
        return MapToDto(book);
    }

    public async Task<BookDto> UpdateAsync(Guid id, UpdateBookDto dto)
    {
        var book = await _bookRepository.GetByIdAsync(id);
        if (book == null)
            throw new KeyNotFoundException($"Book with ID {id} not found");

        if (await _bookRepository.IsbnExistsAsync(dto.ISBN, id))
            throw new InvalidOperationException($"ISBN {dto.ISBN} already exists");

        var category = await _categoryRepository.GetByIdAsync(dto.CategoryId);
        if (category == null)
            throw new KeyNotFoundException($"Category with ID {dto.CategoryId} not found");

        var copiesDifference = dto.TotalCopies - book.TotalCopies;
        book.Title = dto.Title;
        book.ISBN = dto.ISBN;
        book.AuthorPartyId = dto.AuthorPartyId;
        book.CategoryId = dto.CategoryId;
        book.TotalCopies = dto.TotalCopies;
        book.AvailableCopies += copiesDifference;
        book.UpdatedAt = DateTime.UtcNow;

        await _bookRepository.UpdateAsync(book);

        book.Category = category;

        // Serialize a DTO (not the EF entity) to avoid cycles
        var payloadDto = MapToDto(book);

        await _eventPublisher.PublishAsync(new BookUpdated
        {
            BookId = book.Id,
            Title = book.Title,
            ISBN = book.ISBN,
            AuthorPartyId = book.AuthorPartyId,
            CategoryId = book.CategoryId,
            TotalCopies = book.TotalCopies,
            AvailableCopies = book.AvailableCopies,
            EntityId = book.Id.ToString(),
            EntityType = "Book",
            ActionType = "Updated",
            Payload = JsonSerializer.Serialize(payloadDto)
        });

        book.Category = category;
        return MapToDto(book);
    }

    public async Task DeleteAsync(Guid id)
    {
        var book = await _bookRepository.GetByIdAsync(id);
        if (book == null)
            throw new KeyNotFoundException($"Book with ID {id} not found");

        await _bookRepository.DeleteAsync(id);

        await _eventPublisher.PublishAsync(new BookDeleted
        {
            BookId = id,
            EntityId = id.ToString(),
            EntityType = "Book",
            ActionType = "Deleted",
            Payload = JsonSerializer.Serialize(new { BookId = id })
        });
    }

    public async Task UpdateAvailableCopiesAsync(Guid bookId, int change)
    {
        var book = await _bookRepository.GetByIdAsync(bookId);
        if (book != null)
        {
            book.AvailableCopies += change;
            book.UpdatedAt = DateTime.UtcNow;
            await _bookRepository.UpdateAsync(book);
        }
    }

    private static BookDto MapToDto(Book book)
    {
        return new BookDto
        {
            Id = book.Id,
            Title = book.Title,
            ISBN = book.ISBN,
            AuthorPartyId = book.AuthorPartyId,
            CategoryId = book.CategoryId,
            CategoryName = book.Category?.Name ?? string.Empty,
            TotalCopies = book.TotalCopies,
            AvailableCopies = book.AvailableCopies,
            CreatedAt = book.CreatedAt,
            UpdatedAt = book.UpdatedAt
        };
    }
}
