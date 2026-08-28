using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]

// Common error types
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
public sealed class RoomsController(ISender sender) : ControllerBase
{
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(RoomDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
    {
        var query = new GetRoomByIdQuery(id);
        var result = await sender.Send(query, ct);

        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<RoomDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var query = new GetAllRoomsQuery();
        var result = await sender.Send(query, ct);

        return Ok(result);
    }

    [HttpGet("search")]
    [ProducesResponseType(typeof(IReadOnlyCollection<RoomDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search
    (
        [FromQuery] DateTime start,
        [FromQuery] DateTime end,
        [FromQuery] int capacity,
        CancellationToken ct)
    {
        var query = new SearchAvailableRoomsQuery(start, end, capacity);
        var result = await sender.Send(query, ct);

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateRoomRequest request, CancellationToken ct)
    {
        var command = new CreateRoomCommand
        (
            request.Name,
            request.Capacity,
            request.Amount,
            request.Currency
        );

        var result = await sender.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result }, result);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(UpdateRoomResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRoomRequest request, CancellationToken ct)
    {
        var command = new UpdateRoomCommand
        (
            id,
            request.Name,
            request.Capacity,
            request.Amount,
            request.Currency);

        var result = await sender.Send(command, ct);

        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(DeleteRoomCommandResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
    {
        var command = new DeleteRoomCommand(id);
        var result = await sender.Send(command, ct);

        return Ok(result);
    }

    [HttpPost("{id:guid}/services")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddService([FromRoute] Guid id, [FromBody] AddServiceRequest request, CancellationToken ct)
    {
        var command = new AddServiceToRoomCommand
        (
            id,
            request.Name,
            request.Price,
            request.Currency);

        await sender.Send(command, ct);

        return NoContent();
    }

    [HttpDelete("{id:guid}/services/{serviceId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveService([FromRoute] Guid id, [FromRoute] Guid serviceId, CancellationToken ct)
    {
        var command = new RemoveServiceFromRoomCommand(id, serviceId);
        await sender.Send(command, ct);

        return NoContent();
    }

    [HttpPatch("{id:guid}/maintenance")]
    [ProducesResponseType(typeof(SetRoomUnderMaintenanceResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SetRoomUnderMaintenance([FromRoute] Guid id, CancellationToken ct)
    {
        var command = new SetRoomUnderMaintenanceCommand(id);
        var result = await sender.Send(command, ct);

        return Ok(result);
    }

    [HttpPatch("{id:guid}/reactivate")]
    [ProducesResponseType(typeof(ReactivateRoomCommandResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ReactivateRoom([FromRoute] Guid id, CancellationToken ct)
    {
        var command = new ReactivateRoomCommand(id);
        var result = await sender.Send(command, ct);

        return Ok(result);
    }
}