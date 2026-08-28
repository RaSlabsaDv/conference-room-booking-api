using MediatR;

public sealed class DeleteRoomCommandHandler
(
    IRoomRepository roomRepository,
    IBookingRepository bookingRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<DeleteRoomCommand, DeleteRoomCommandResult>
{
    public async Task<DeleteRoomCommandResult> Handle(DeleteRoomCommand request, CancellationToken ct)
    {
        var room = await roomRepository.GetByIdAsync(request.RoomId, ct)
            ?? throw new NotFoundException($"Room {request.RoomId} not found");

        var hasActiveBooking = await bookingRepository.HasActiveFutureBookingsAsync(room.Id, ct);

        if (hasActiveBooking)
            throw new DomainException("Cannot delete a room with active booking");

        room.MarkAsDeleted();
        await unitOfWork.SaveChangesAsync(ct);

        return new DeleteRoomCommandResult(room.Id, room.RoomStatus.ToString());
    }
}