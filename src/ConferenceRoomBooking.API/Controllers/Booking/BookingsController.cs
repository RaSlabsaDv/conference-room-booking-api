using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]

// Common error types
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
public sealed class BookingsController(ISender sender) : ControllerBase
{
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(BookingDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
    {
        var query = new GetBookingByIdQuery(id);
        var result = await sender.Send(query, ct);

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateBookingResult), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateBookingRequest request, CancellationToken ct)
    {
        var command = new CreateBookingCommand
        (
            request.RoomId, 
            request.StartTime,
            request.EndTime,
            request.SelectedServiceIds ?? []);

        var result = await sender.Send(command, ct);

        return CreatedAtAction(nameof(GetById), new { id = result.BookingId }, result);
    }

    [HttpPost("{id:guid}/cancel")]
    [ProducesResponseType(typeof(CancelBookingResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Cancel([FromRoute] Guid id, CancellationToken ct)
    {
        var command = new CancelBookingCommand(id);
        var result = await sender.Send(command, ct);

        return Ok(result);
    }
}