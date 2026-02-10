using Reservation.Application.DTOs;
using Reservation.Domain.Interfaces;
using System.Text.Json;
using Library.Shared.Events;

namespace Reservation.Application.Services;

public class ReservationService
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IEventPublisher _eventPublisher;

    public ReservationService(IReservationRepository reservationRepository, IEventPublisher eventPublisher)
    {
        _reservationRepository = reservationRepository;
        _eventPublisher = eventPublisher;
    }

    public async Task<ReservationDto> GetByIdAsync(Guid id)
    {
        var reservation = await _reservationRepository.GetByIdAsync(id);
        if (reservation == null)
            throw new KeyNotFoundException($"Reservation with ID {id} not found");
        
        return MapToDto(reservation);
    }

    public async Task<IEnumerable<ReservationDto>> GetAllAsync()
    {
        var reservations = await _reservationRepository.GetAllAsync();
        return reservations.Select(MapToDto);
    }

    public async Task<IEnumerable<ReservationDto>> GetActiveReservationsAsync()
    {
        var reservations = await _reservationRepository.GetActiveReservationsAsync();
        return reservations.Select(MapToDto);
    }

    public async Task<IEnumerable<ReservationDto>> GetByCustomerIdAsync(Guid customerPartyId)
    {
        var reservations = await _reservationRepository.GetByCustomerIdAsync(customerPartyId);
        return reservations.Select(MapToDto);
    }

    public async Task<ReservationDto> BorrowBookAsync(BorrowBookDto dto)
    {
        var existingReservation = await _reservationRepository.GetActiveByCustomerAndBookAsync(
            dto.CustomerPartyId, dto.BookId);
        
        if (existingReservation != null)
            throw new InvalidOperationException("Customer already has an active reservation for this book");

        var reservation = new Domain.Entities.Reservation
        {
            Id = Guid.NewGuid(),
            BookId = dto.BookId,
            CustomerPartyId = dto.CustomerPartyId,
            BorrowedAt = DateTime.UtcNow,
            IsActive = true
        };

        reservation = await _reservationRepository.AddAsync(reservation);

        await _eventPublisher.PublishAsync(new BookBorrowed
        {
            ReservationId = reservation.Id,
            BookId = reservation.BookId,
            CustomerPartyId = reservation.CustomerPartyId,
            BorrowedAt = reservation.BorrowedAt,
            EntityId = reservation.Id.ToString(),
            EntityType = "Reservation",
            ActionType = "BookBorrowed",
            Payload = JsonSerializer.Serialize(reservation)
        });

        return MapToDto(reservation);
    }

    public async Task<ReservationDto> ReturnBookAsync(Guid reservationId)
    {
        var reservation = await _reservationRepository.GetByIdAsync(reservationId);
        if (reservation == null)
            throw new KeyNotFoundException($"Reservation with ID {reservationId} not found");

        if (!reservation.IsActive)
            throw new InvalidOperationException("Reservation is already completed");

        reservation.ReturnedAt = DateTime.UtcNow;
        reservation.IsActive = false;

        await _reservationRepository.UpdateAsync(reservation);

        await _eventPublisher.PublishAsync(new BookReturned
        {
            ReservationId = reservation.Id,
            BookId = reservation.BookId,
            CustomerPartyId = reservation.CustomerPartyId,
            ReturnedAt = reservation.ReturnedAt.Value,
            EntityId = reservation.Id.ToString(),
            EntityType = "Reservation",
            ActionType = "BookReturned",
            Payload = JsonSerializer.Serialize(reservation)
        });

        return MapToDto(reservation);
    }

    private static ReservationDto MapToDto(Domain.Entities.Reservation reservation)
    {
        return new ReservationDto
        {
            Id = reservation.Id,
            BookId = reservation.BookId,
            CustomerPartyId = reservation.CustomerPartyId,
            BorrowedAt = reservation.BorrowedAt,
            ReturnedAt = reservation.ReturnedAt,
            IsActive = reservation.IsActive
        };
    }
}
