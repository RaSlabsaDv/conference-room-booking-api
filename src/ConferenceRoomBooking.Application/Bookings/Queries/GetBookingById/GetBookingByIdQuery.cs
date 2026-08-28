using MediatR;

public sealed record GetBookingByIdQuery(Guid BookingId) : IRequest<BookingDto>;