using MediatR;

public sealed class CreateRoomCommandHandler
(
    IRoomRepository roomRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<CreateRoomCommand, Guid>
{
    public async Task<Guid> Handle(CreateRoomCommand request, CancellationToken ct)
    {
        var room = new Room
        (
            request.Name, 
            request.Capacity, 
            new Money(request.Amount, request.Currency));

        await roomRepository.AddAsync(room, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return room.Id;
    }
}