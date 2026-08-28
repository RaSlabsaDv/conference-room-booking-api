using MediatR;

public sealed record GetRoomByIdQuery(Guid RoomId) : IRequest<RoomDto>;