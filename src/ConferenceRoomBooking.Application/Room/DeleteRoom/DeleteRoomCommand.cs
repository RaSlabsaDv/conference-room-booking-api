using MediatR;

public sealed record DeleteRoomCommand(Guid RoomId) : IRequest<DeleteRoomCommandResult>;