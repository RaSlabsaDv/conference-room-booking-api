using MediatR;

public sealed record CreateBookingCommand(
    Guid RoomId,
    DateTime StartTime,
    DateTime EndTime,
    List<Guid> SelectedServiceIds
) : IRequest<CreateBookingResult>;