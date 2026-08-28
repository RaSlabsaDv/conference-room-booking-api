using MediatR;

public sealed class GetRoomOccupancyReportQueryHandler(
    IBookingRepository bookingRepository,
    IRoomRepository roomRepository
) : IRequestHandler<GetRoomOccupancyReportQuery, RoomOccupancyReportDto>
{
    public async Task<RoomOccupancyReportDto> Handle(GetRoomOccupancyReportQuery request, CancellationToken ct)
    {
        var intervals = await bookingRepository.GetBookingIntervalsAsync(request.From, request.To, ct);
        var rooms = await roomRepository.GetAllAsync(ct);
        var roomNames = rooms.ToDictionary(r => r.Id, r => r.Name);

        var occupancy = intervals
            .GroupBy(i => i.RoomId)
            .Select(g => new RoomOccupancyItemDto(
                g.Key,
                roomNames.GetValueOrDefault(g.Key, "Unknown Room"),
                g.Sum(i => (i.EndTime - i.StartTime).TotalHours),
                g.Count()))
            .ToList();

        return new RoomOccupancyReportDto(request.From, request.To, occupancy);
    }
}