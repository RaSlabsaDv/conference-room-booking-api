using MediatR;

public sealed class UpdateRoomCommandHandler
(
    IRoomRepository roomRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<UpdateRoomCommand, UpdateRoomResult>
{
    public async Task<UpdateRoomResult> Handle(UpdateRoomCommand request, CancellationToken ct)
    {
        var room = await roomRepository.GetByIdAsync(request.RoomId, ct)
            ?? throw new NotFoundException($"Room {request.RoomId} not found");

        room.Rename(request.Name);
        room.UpdateCapacity(request.Capacity);
        room.UpdateBaseRate(new Money(request.Amount, request.Currency));

        await unitOfWork.SaveChangesAsync(ct);

        return new UpdateRoomResult
        (
            room.Id,
            room.Name,
            room.Capacity,
            room.BaseHourlyRate.Amount,
            room.BaseHourlyRate.Currency
        );
    }
}