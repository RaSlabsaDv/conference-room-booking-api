using MediatR;

public sealed record CancelBookingCommand(Guid BookingId) : IRequest<CancelBookingResult>;