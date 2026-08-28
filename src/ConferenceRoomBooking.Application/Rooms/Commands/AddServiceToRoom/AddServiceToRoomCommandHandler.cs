using MediatR;

public sealed class AddServiceToRoomCommandHandler(
    IRoomRepository roomRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<AddServiceToRoomCommand>
{
    public async Task Handle(AddServiceToRoomCommand request, CancellationToken ct)
    {
        var room = await roomRepository.GetByIdAsync(request.RoomId, ct)
            ?? throw new NotFoundException($"Room {request.RoomId} not found");

        var price = new Money(request.Price, request.Currency);
        
        room.AddService(request.Name, price);
        await unitOfWork.SaveChangesAsync(ct);
    }
}