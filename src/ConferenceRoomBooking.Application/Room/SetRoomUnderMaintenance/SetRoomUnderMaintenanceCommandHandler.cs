using MediatR;

public sealed class SetRoomUnderMaintenanceCommandHandler
(
    IRoomRepository roomRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<SetRoomUnderMaintenanceCommand, SetRoomUnderMaintenanceResult>
{
    public async Task<SetRoomUnderMaintenanceResult> Handle(SetRoomUnderMaintenanceCommand request, CancellationToken ct)
    {
        var room = await roomRepository.GetByIdAsync(request.RoomId, ct)
            ?? throw new NotFoundException($"Room {request.RoomId} not found");

        room.SetUnderMaintenance();
        await unitOfWork.SaveChangesAsync(ct);

        return new SetRoomUnderMaintenanceResult(room.Id, room.RoomStatus.ToString());
    }
}