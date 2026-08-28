using MediatR;

public sealed class GetAllRoomsQueryHandler
(
    IRoomRepository roomRepository
) : IRequestHandler<GetAllRoomsQuery, IReadOnlyCollection<RoomDto>>
{
    public async Task<IReadOnlyCollection<RoomDto>> Handle(GetAllRoomsQuery request, CancellationToken ct)
    {
        var rooms = await roomRepository.GetAllAsync(ct);

        return rooms.Select(RoomDto.FromDomain).ToList();
    }
}