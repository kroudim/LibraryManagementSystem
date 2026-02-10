using Moq;
using Reservation.Application.DTOs;
using Reservation.Application.Services;
using Reservation.Domain.Interfaces;
using Xunit;

namespace Reservation.UnitTests;

public class ReservationServiceTests
{
    private readonly Mock<IReservationRepository> _reservationRepositoryMock;
    private readonly Mock<IEventPublisher> _eventPublisherMock;
    private readonly ReservationService _reservationService;

    public ReservationServiceTests()
    {
        _reservationRepositoryMock = new Mock<IReservationRepository>();
        _eventPublisherMock = new Mock<IEventPublisher>();
        _reservationService = new ReservationService(_reservationRepositoryMock.Object, _eventPublisherMock.Object);
    }

    [Fact]
    public async Task BorrowBookAsync_ValidRequest_ReturnsReservationDto()
    {
        var borrowDto = new BorrowBookDto
        {
            BookId = Guid.NewGuid(),
            CustomerPartyId = Guid.NewGuid()
        };

        _reservationRepositoryMock
            .Setup(r => r.GetActiveByCustomerAndBookAsync(borrowDto.CustomerPartyId, borrowDto.BookId))
            .ReturnsAsync((Domain.Entities.Reservation)null!);
        _reservationRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Domain.Entities.Reservation>()))
            .ReturnsAsync((Domain.Entities.Reservation r) => r);

        var result = await _reservationService.BorrowBookAsync(borrowDto);

        Assert.NotNull(result);
        Assert.True(result.IsActive);
        Assert.Equal(borrowDto.BookId, result.BookId);
        _eventPublisherMock.Verify(p => p.PublishAsync(It.IsAny<object>()), Times.Once);
    }

    [Fact]
    public async Task BorrowBookAsync_DuplicateActiveReservation_ThrowsInvalidOperationException()
    {
        var borrowDto = new BorrowBookDto
        {
            BookId = Guid.NewGuid(),
            CustomerPartyId = Guid.NewGuid()
        };
        var existingReservation = new Domain.Entities.Reservation
        {
            Id = Guid.NewGuid(),
            IsActive = true
        };

        _reservationRepositoryMock
            .Setup(r => r.GetActiveByCustomerAndBookAsync(borrowDto.CustomerPartyId, borrowDto.BookId))
            .ReturnsAsync(existingReservation);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _reservationService.BorrowBookAsync(borrowDto));
    }

    [Fact]
    public async Task ReturnBookAsync_ActiveReservation_MarksAsCompleted()
    {
        var reservationId = Guid.NewGuid();
        var reservation = new Domain.Entities.Reservation
        {
            Id = reservationId,
            IsActive = true,
            BorrowedAt = DateTime.UtcNow.AddDays(-5)
        };

        _reservationRepositoryMock.Setup(r => r.GetByIdAsync(reservationId)).ReturnsAsync(reservation);

        var result = await _reservationService.ReturnBookAsync(reservationId);

        Assert.NotNull(result);
        Assert.False(result.IsActive);
        Assert.NotNull(result.ReturnedAt);
        _reservationRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Domain.Entities.Reservation>()), Times.Once);
        _eventPublisherMock.Verify(p => p.PublishAsync(It.IsAny<object>()), Times.Once);
    }
}
