using Microsoft.EntityFrameworkCore;

public sealed class RoomRepository(AppDbContext context) : IRoomRepository
{
    public async Task AddAsync(Room room, CancellationToken ct = default)
    {
        await context.Rooms.AddAsync(room, ct);
    }

    public async Task<List<Room>> GetAllAsync(CancellationToken ct = default)
    {
        return await context.Rooms
            .Include(r => r.Services)
            .ToListAsync(ct);
    }

    public async Task<Room?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await context.Rooms
            .Include(r => r.Services)
            .FirstOrDefaultAsync(r => r.Id == id, ct);
    }
}