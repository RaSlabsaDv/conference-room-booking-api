public interface IBookingRepository
{
    Task<Booking?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<List<Booking>> GetByRoomIdAsync(Guid roomId, CancellationToken ct = default);
    Task<List<Booking>> GetOverlappingAsync(Guid roomId, DateTime start, DateTime end, CancellationToken ct = default);
    Task<List<Guid>> GetBusyRoomIdsAsync(DateTime start, DateTime end, CancellationToken ct = default);
    Task AddAsync(Booking booking, CancellationToken ct = default);
    Task<List<Booking>> GetAllAsync(CancellationToken ct = default);
    Task<bool> HasActiveFutureBookingsAsync(Guid roomId, CancellationToken ct = default);
    Task<Decimal> GetRevenueAsync(DateTime from, DateTime to, CancellationToken ct = default);
    Task<int> GetBookingsCountAsync(DateTime from, DateTime to, CancellationToken ct = default);
    Task<List<(Guid RoomId, DateTime StartTime, DateTime EndTime)>> GetBookingIntervalsAsync(
        DateTime from, DateTime to, CancellationToken ct = default);
}