using MediatR;

public sealed record GetAllRoomsQuery() : IRequest<IReadOnlyCollection<RoomDto>>;