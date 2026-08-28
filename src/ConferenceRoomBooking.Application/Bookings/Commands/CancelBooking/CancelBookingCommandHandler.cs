using MediatR;

public sealed class CancelBookingCommandHandler
(
    IBookingRepository bookingRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<CancelBookingCommand, CancelBookingResult>
{
    public async Task<CancelBookingResult> Handle(CancelBookingCommand request, CancellationToken ct)
    {
        var booking = await bookingRepository.GetByIdAsync(request.BookingId, ct)
            ?? throw new NotFoundException($"Booking {request.BookingId} not found");

        booking.Cancel();
        await unitOfWork.SaveChangesAsync(ct);

        return new CancelBookingResult(booking.Id, booking.BookingStatus.ToString());
    }
}