public interface IServiceRepository
{
    Task<Service?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<List<Service>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
}