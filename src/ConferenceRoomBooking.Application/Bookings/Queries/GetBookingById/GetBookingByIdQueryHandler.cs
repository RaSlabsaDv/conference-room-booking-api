using MediatR;

public sealed class GetBookingByIdQueryHandler
(
    IBookingRepository bookingRepository
) : IRequestHandler<GetBookingByIdQuery, BookingDto>
{
    public async Task<BookingDto> Handle(GetBookingByIdQuery request, CancellationToken ct)
    {
        var booking = await bookingRepository.GetByIdAsync(request.BookingId, ct)
            ?? throw new NotFoundException($"Booking {request.BookingId} not found");

        return BookingDto.FromDomain(booking);
    }
}