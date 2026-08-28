using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]

// Common error types
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
public sealed class ReportsController(ISender sender) : ControllerBase
{
    [HttpGet("revenue")]
    [ProducesResponseType(typeof(RevenueReportDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Revenue([FromQuery] DateTime from, [FromQuery] DateTime to, CancellationToken ct)
    {
        var query = new GetRevenueReportQuery(from, to);
        var result = await sender.Send(query, ct);

        return Ok(result);
    }

    [HttpGet("occupancy")]
    [ProducesResponseType(typeof(RoomOccupancyReportDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Occupancy([FromQuery] DateTime from, [FromQuery] DateTime to, CancellationToken ct)
    {
        var query = new GetRoomOccupancyReportQuery(from, to);
        var result = await sender.Send(query, ct);

        return Ok(result);
    }
}