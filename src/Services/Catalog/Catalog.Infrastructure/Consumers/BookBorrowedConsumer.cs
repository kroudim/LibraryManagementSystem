using MassTransit;
using Catalog.Application.Services;
using Library.Shared.Events;
using Microsoft.Extensions.Logging;

namespace Catalog.Infrastructure.Consumers;

public class BookBorrowedConsumer : IConsumer<BookBorrowed>
{
    private readonly BookService _bookService;
    private readonly ILogger<BookBorrowedConsumer> _logger;

    public BookBorrowedConsumer(BookService bookService, ILogger<BookBorrowedConsumer> logger)
    {
        _bookService = bookService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<BookBorrowed> context)
    {
        _logger.LogInformation("BookBorrowed event received for book {BookId}", context.Message.BookId);
        
        try
        {
            await _bookService.UpdateAvailableCopiesAsync(context.Message.BookId, -1);
            _logger.LogInformation("Available copies decreased for book {BookId}", context.Message.BookId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing BookBorrowed event for book {BookId}", context.Message.BookId);
            throw;
        }
    }
}
