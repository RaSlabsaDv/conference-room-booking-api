using MediatR;

public sealed class SearchAvailableRoomsQueryHandler
(
    IRoomRepository roomRepository,
    IBookingRepository bookingRepository
) : IRequestHandler<SearchAvailableRoomsQuery, IReadOnlyCollection<RoomDto>>
{
    public async Task<IReadOnlyCollection<RoomDto>> Handle(SearchAvailableRoomsQuery request, CancellationToken ct)
    {
        var rooms = await roomRepository.GetAllAsync(ct);

        var roundedStart = BookingTimeRounding.RoundStartDown(request.Start);
        var roundedEnd = BookingTimeRounding.RoundEndUp(request.End);

        var busyRooms = (await bookingRepository.GetBusyRoomIdsAsync(roundedStart, roundedEnd, ct)).ToHashSet();

        var availableRooms = rooms.Where(r => r.HasCapacityFor(request.Capacity) && !busyRooms.Contains(r.Id));

        return availableRooms.Select(RoomDto.FromDomain).ToList();
    }
}