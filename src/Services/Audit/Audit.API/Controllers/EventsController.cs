using Microsoft.AspNetCore.Mvc;
using Audit.Application.DTOs;
using Audit.Application.Services;

namespace Audit.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly AuditEventService _auditEventService;
    private readonly ILogger<EventsController> _logger;

    public EventsController(AuditEventService auditEventService, ILogger<EventsController> logger)
    {
        _auditEventService = auditEventService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<AuditEventDto>>> GetEvents([FromQuery] int page = 1, [FromQuery] int pageSize = 50)
    {
        try
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 50;
            
            var result = await _auditEventService.GetEventsAsync(page, pageSize);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting events");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("party/{partyId}")]
    public async Task<ActionResult<List<AuditEventDto>>> GetEventsByPartyId(Guid partyId)
    {
        try
        {
            var events = await _auditEventService.GetEventsByPartyIdAsync(partyId);
            return Ok(events);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting events for party {PartyId}", partyId);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("book/{bookId}")]
    public async Task<ActionResult<List<AuditEventDto>>> GetEventsByBookId(Guid bookId)
    {
        try
        {
            var events = await _auditEventService.GetEventsByBookIdAsync(bookId);
            return Ok(events);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting events for book {BookId}", bookId);
            return StatusCode(500, "Internal server error");
        }
    }
}
