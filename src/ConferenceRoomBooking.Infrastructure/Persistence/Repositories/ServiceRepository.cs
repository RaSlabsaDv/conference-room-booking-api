using Microsoft.EntityFrameworkCore;

public sealed class ServiceRepository(AppDbContext context) : IServiceRepository
{
    public async Task<Service?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await context.Services.FirstOrDefaultAsync(s => s.Id == id, ct);
    }

    public async Task<List<Service>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
    {
        var idList = ids?.Distinct().ToList() ?? [];

        if (idList.Count == 0)
            return [];

        return await context.Services.Where(s => idList.Contains(s.Id)).ToListAsync(ct);
    }
}