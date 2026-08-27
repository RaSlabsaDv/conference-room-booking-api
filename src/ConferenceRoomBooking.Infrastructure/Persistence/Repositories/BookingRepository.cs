using Microsoft.EntityFrameworkCore;

public class BookingRepository(AppDbContext context) : IBookingRepository
{
    public async Task AddAsync(Booking booking, CancellationToken ct = default)
    {
        await context.Bookings.AddAsync(booking, ct);
    }

    public async Task<List<Booking>> GetAllAsync(CancellationToken ct = default)
    {
        return await context.Bookings
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<List<Guid>> GetBusyRoomIdsAsync(DateTime start, DateTime end, CancellationToken ct = default)
    {
        // Same query formula, but without a filter for a specific RoomId —
        // returns the IDs of ALL occupied rooms in a single query.

        return await context.Bookings
            .Where(b => b.BookingStatus == BookingStatus.Confirmed
                     && b.StartTime < end
                     && start < b.EndTime)
            .Select(b => b.RoomId)
            .Distinct()
            .ToListAsync(ct);
    }

    public async Task<Booking?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await context.Bookings.FirstOrDefaultAsync(b => b.Id == id, ct);
    }

    public async Task<List<Booking>> GetByRoomIdAsync(Guid roomId, CancellationToken ct = default)
    {
        return await context.Bookings
            .Where(b => b.RoomId == roomId)
            .ToListAsync(ct);
    }

    public async Task<List<Booking>> GetOverlappingAsync(Guid roomId, DateTime start, DateTime end, CancellationToken ct = default)
    {
        // Mirror Booking.OverlapsWith() — synchronize both locations
        // when changing the formula for overlapping intervals.

        return await context.Bookings
            .Where(b => b.RoomId == roomId
                     && b.BookingStatus == BookingStatus.Confirmed
                     && b.StartTime < end
                     && start < b.EndTime)
            .ToListAsync(ct);
    }
}