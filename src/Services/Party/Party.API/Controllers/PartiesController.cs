using Microsoft.AspNetCore.Mvc;
using Party.Application.DTOs;
using Party.Application.Services;

namespace Party.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PartiesController : ControllerBase
{
    private readonly PartyService _partyService;
    private readonly ILogger<PartiesController> _logger;

    public PartiesController(PartyService partyService, ILogger<PartiesController> logger)
    {
        _partyService = partyService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PartyDto>>> GetAll()
    {
        try
        {
            var parties = await _partyService.GetAllAsync();
            return Ok(parties);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all parties");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PartyDto>> GetById(Guid id)
    {
        try
        {
            var party = await _partyService.GetByIdAsync(id);
            return Ok(party);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting party {PartyId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<ActionResult<PartyDto>> Create([FromBody] CreatePartyDto dto)
    {
        try
        {
            var party = await _partyService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = party.Id }, party);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating party");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<PartyDto>> Update(Guid id, [FromBody] UpdatePartyDto dto)
    {
        try
        {
            var party = await _partyService.UpdateAsync(id, dto);
            return Ok(party);
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
            _logger.LogError(ex, "Error updating party {PartyId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        try
        {
            await _partyService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting party {PartyId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost("{id}/roles")]
    public async Task<ActionResult> AssignRole(Guid id, [FromBody] AssignRoleDto dto)
    {
        try
        {
            await _partyService.AssignRoleAsync(id, dto.RoleId);
            return NoContent();
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
            _logger.LogError(ex, "Error assigning role to party {PartyId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("{id}/roles/{roleId}")]
    public async Task<ActionResult> RemoveRole(Guid id, Guid roleId)
    {
        try
        {
            await _partyService.RemoveRoleAsync(id, roleId);
            return NoContent();
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
            _logger.LogError(ex, "Error removing role from party {PartyId}", id);
            return StatusCode(500, "Internal server error");
        }
    }
}

[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly PartyService _partyService;
    private readonly ILogger<RolesController> _logger;

    public RolesController(PartyService partyService, ILogger<RolesController> logger)
    {
        _partyService = partyService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoleDto>>> GetAll()
    {
        try
        {
            var roles = await _partyService.GetRolesAsync();
            return Ok(roles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all roles");
            return StatusCode(500, "Internal server error");
        }
    }
}
