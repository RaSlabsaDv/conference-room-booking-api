using MediatR;

public sealed class RemoveServiceFromRoomCommandHandler(
    IRoomRepository roomRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<RemoveServiceFromRoomCommand>
{
    public async Task Handle(RemoveServiceFromRoomCommand request, CancellationToken ct)
    {
        var room = await roomRepository.GetByIdAsync(request.RoomId, ct)
            ?? throw new NotFoundException($"Room {request.RoomId} not found");

        room.RemoveService(request.ServiceId);
        await unitOfWork.SaveChangesAsync(ct);
    }
}