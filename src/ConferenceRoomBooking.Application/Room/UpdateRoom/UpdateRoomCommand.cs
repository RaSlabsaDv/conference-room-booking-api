using MediatR;

public sealed record UpdateRoomCommand
(
    Guid RoomId,
    string Name,
    int Capacity,
    decimal Amount,
    string Currency = "UAH") : IRequest<UpdateRoomResult>;