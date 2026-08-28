using MediatR;

public sealed record SetRoomUnderMaintenanceCommand(Guid RoomId) : IRequest<SetRoomUnderMaintenanceResult>;