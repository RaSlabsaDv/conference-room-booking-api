public sealed record CreateBookingRequest(
    Guid RoomId,
    DateTime StartTime,
    DateTime EndTime,
    List<Guid> SelectedServiceIds);