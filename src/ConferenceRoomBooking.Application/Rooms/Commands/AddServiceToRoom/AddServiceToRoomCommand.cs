using MediatR;

public sealed record AddServiceToRoomCommand
(
    Guid RoomId,
    string Name,
    decimal Price,
    string Currency = "UAH") : IRequest;