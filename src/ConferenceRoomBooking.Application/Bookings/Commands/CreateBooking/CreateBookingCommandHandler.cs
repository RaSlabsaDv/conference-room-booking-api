using MediatR;

public sealed class CreateBookingCommandHandler
(
    IRoomRepository roomRepository,
    IBookingRepository bookingRepository,
    IServiceRepository serviceRepository,
    IPricingCalculator calculator,
    IUnitOfWork unitOfWork
) : IRequestHandler<CreateBookingCommand, CreateBookingResult>
{
    public async Task<CreateBookingResult> Handle(CreateBookingCommand request, CancellationToken ct)
    {
        var room = await roomRepository.GetByIdAsync(request.RoomId, ct)
            ?? throw new NotFoundException($"Room {request.RoomId} not found");

        if (room.RoomStatus != RoomStatus.Active)
            throw new DomainException("Cannot book unactive room");

        var start = BookingTimeRounding.RoundStartDown(request.StartTime);
        var end = BookingTimeRounding.RoundEndUp(request.EndTime);

        var overlapping = await bookingRepository.GetOverlappingAsync(request.RoomId, start, end, ct);

        if (overlapping.Count != 0)
            throw new DomainException($"Room is busy in period {start}-{end}");

        var uniqueRequestedIds = request.SelectedServiceIds.Distinct().ToList();
        var services = await serviceRepository.GetByIdsAsync(uniqueRequestedIds, ct);

        if (services.Count < uniqueRequestedIds.Count)
            throw new NotFoundException("One or more selected services were not found");

        var booking = new Booking(request.RoomId, start, end);

        foreach (var service in services)
        {
            booking.AddService(service);
        }

        var totalPrice = calculator.Calculate(room, start, end, services);
        booking.SetTotalPrice(totalPrice);

        await bookingRepository.AddAsync(booking, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return CreateBookingResult.FromDomain(booking);
    }
}