using MediatR;

public sealed record SearchAvailableRoomsQuery(
    DateTime Start,
    DateTime End,
    int Capacity
) : IRequest<IReadOnlyCollection<RoomDto>>;