using MediatR;

public sealed record ReactivateRoomCommand(Guid RoomId) : IRequest<ReactivateRoomCommandResult>;