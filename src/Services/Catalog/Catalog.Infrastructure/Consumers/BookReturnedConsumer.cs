using MassTransit;
using Catalog.Application.Services;
using Library.Shared.Events;
using Microsoft.Extensions.Logging;

namespace Catalog.Infrastructure.Consumers;

public class BookReturnedConsumer : IConsumer<BookReturned>
{
    private readonly BookService _bookService;
    private readonly ILogger<BookReturnedConsumer> _logger;

    public BookReturnedConsumer(BookService bookService, ILogger<BookReturnedConsumer> logger)
    {
        _bookService = bookService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<BookReturned> context)
    {
        _logger.LogInformation("BookReturned event received for book {BookId}", context.Message.BookId);
        
        try
        {
            await _bookService.UpdateAvailableCopiesAsync(context.Message.BookId, 1);
            _logger.LogInformation("Available copies increased for book {BookId}", context.Message.BookId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing BookReturned event for book {BookId}", context.Message.BookId);
            throw;
        }
    }
}
