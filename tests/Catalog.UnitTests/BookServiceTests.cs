using Moq;
using Catalog.Application.DTOs;
using Catalog.Application.Services;
using Catalog.Domain.Entities;
using Catalog.Domain.Interfaces;
using Xunit;

namespace Catalog.UnitTests;

public class BookServiceTests
{
    private readonly Mock<IBookRepository> _bookRepositoryMock;
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly Mock<IEventPublisher> _eventPublisherMock;
    private readonly BookService _bookService;

    public BookServiceTests()
    {
        _bookRepositoryMock = new Mock<IBookRepository>();
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _eventPublisherMock = new Mock<IEventPublisher>();
        _bookService = new BookService(_bookRepositoryMock.Object, _categoryRepositoryMock.Object, _eventPublisherMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ValidBook_ReturnsBookDto()
    {
        var categoryId = Guid.NewGuid();
        var category = new Category { Id = categoryId, Name = "Fiction" };
        var createDto = new CreateBookDto
        {
            Title = "Test Book",
            ISBN = "978-1234567890",
            AuthorPartyId = Guid.NewGuid(),
            CategoryId = categoryId,
            TotalCopies = 5
        };

        _bookRepositoryMock.Setup(r => r.IsbnExistsAsync(createDto.ISBN, null)).ReturnsAsync(false);
        _categoryRepositoryMock.Setup(r => r.GetByIdAsync(categoryId)).ReturnsAsync(category);
        _bookRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Book>())).ReturnsAsync((Book b) => b);

        var result = await _bookService.CreateAsync(createDto);

        Assert.NotNull(result);
        Assert.Equal(createDto.Title, result.Title);
        Assert.Equal(createDto.TotalCopies, result.AvailableCopies);
        _eventPublisherMock.Verify(p => p.PublishAsync(It.IsAny<object>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAvailableCopiesAsync_ValidBook_UpdatesCopies()
    {
        var bookId = Guid.NewGuid();
        var book = new Book { Id = bookId, AvailableCopies = 5 };

        _bookRepositoryMock.Setup(r => r.GetByIdAsync(bookId)).ReturnsAsync(book);

        await _bookService.UpdateAvailableCopiesAsync(bookId, -1);

        Assert.Equal(4, book.AvailableCopies);
        _bookRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Book>()), Times.Once);
    }
}
