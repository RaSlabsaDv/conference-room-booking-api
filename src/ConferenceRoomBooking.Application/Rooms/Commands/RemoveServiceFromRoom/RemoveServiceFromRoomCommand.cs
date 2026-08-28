using MediatR;

public sealed record RemoveServiceFromRoomCommand(
    Guid RoomId,
    Guid ServiceId) : IRequest;