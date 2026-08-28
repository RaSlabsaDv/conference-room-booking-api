using MediatR;

public sealed class GetRoomByIdQueryHandler
(
    IRoomRepository roomRepository
) : IRequestHandler<GetRoomByIdQuery, RoomDto>
{
    public async Task<RoomDto> Handle(GetRoomByIdQuery request, CancellationToken ct)
    {
        var room = await roomRepository.GetByIdAsync(request.RoomId, ct)
            ?? throw new NotFoundException($"Room {request.RoomId} not found");
        
        return RoomDto.FromDomain(room);
    }
}