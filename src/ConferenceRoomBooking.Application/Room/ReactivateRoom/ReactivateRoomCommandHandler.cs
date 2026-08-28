using MediatR;

public sealed class ReactivateRoomCommandHandler
(
    IRoomRepository roomRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<ReactivateRoomCommand, ReactivateRoomCommandResult>
{
    public async Task<ReactivateRoomCommandResult> Handle(ReactivateRoomCommand request, CancellationToken ct)
    {
        var room = await roomRepository.GetByIdAsync(request.RoomId, ct)
            ?? throw new NotFoundException($"Room {request.RoomId} not found");

        room.Reactivate();
        await unitOfWork.SaveChangesAsync(ct);

        return new ReactivateRoomCommandResult(room.Id, room.RoomStatus.ToString());
    }
}