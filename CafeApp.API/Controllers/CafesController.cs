using CafeApp.Application.Cafes.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("cafes")]
public class CafesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CafesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<CafeDto>>> GetCafes([FromQuery] string? location)
    {
        var result = await _mediator.Send(new GetCafesQuery { Location = location });

        // Return empty list if invalid location (handled naturally by query)
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateCafeDto dto)
    {
        var id = await _mediator.Send(new CreateCafeCommand { Cafe = dto });
        return CreatedAtAction(nameof(GetCafes), new { id }, id); // optionally return location header
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] UpdateCafeDto dto)
    {
        try
        {
            await _mediator.Send(new UpdateCafeCommand { Cafe = dto });
            return NoContent(); // 204 No Content
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        try
        {
            await _mediator.Send(new DeleteCafeCommand { CafeId = id });
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
