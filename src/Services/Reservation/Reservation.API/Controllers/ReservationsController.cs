using Microsoft.AspNetCore.Mvc;
using Reservation.Application.DTOs;
using Reservation.Application.Services;

namespace Reservation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    private readonly ReservationService _reservationService;
    private readonly ILogger<ReservationsController> _logger;

    public ReservationsController(ReservationService reservationService, ILogger<ReservationsController> logger)
    {
        _reservationService = reservationService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReservationDto>>> GetAll()
    {
        try
        {
            var reservations = await _reservationService.GetAllAsync();
            return Ok(reservations);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all reservations");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<ReservationDto>>> GetActive()
    {
        try
        {
            var reservations = await _reservationService.GetActiveReservationsAsync();
            return Ok(reservations);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting active reservations");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ReservationDto>> GetById(Guid id)
    {
        try
        {
            var reservation = await _reservationService.GetByIdAsync(id);
            return Ok(reservation);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting reservation {ReservationId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("customer/{customerPartyId}")]
    public async Task<ActionResult<IEnumerable<ReservationDto>>> GetByCustomerId(Guid customerPartyId)
    {
        try
        {
            var reservations = await _reservationService.GetByCustomerIdAsync(customerPartyId);
            return Ok(reservations);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting reservations for customer {CustomerPartyId}", customerPartyId);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost("borrow")]
    public async Task<ActionResult<ReservationDto>> BorrowBook([FromBody] BorrowBookDto dto)
    {
        try
        {
            var reservation = await _reservationService.BorrowBookAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = reservation.Id }, reservation);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error borrowing book");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost("return/{reservationId}")]
    public async Task<ActionResult<ReservationDto>> ReturnBook(Guid reservationId)
    {
        try
        {
            var reservation = await _reservationService.ReturnBookAsync(reservationId);
            return Ok(reservation);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error returning book");
            return StatusCode(500, "Internal server error");
        }
    }
}
