public sealed class BookedService
{
    public Guid ServiceId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public Money Price { get; private set; } = null!;

    private BookedService() { } // EF Core

    public BookedService(Service service)
    {
        ServiceId = service.Id;
        Name = service.Name;
        Price = service.Price;
    }
}