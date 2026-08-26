public class Service
{
    public Guid Id { get; private set; }
    public Guid RoomId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public Money Price { get; private set; } = null!;

    private Service() {}

    public Service(Guid roomId, string name, Money price)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Service name cannot be empty");
        if (price is null)
            throw new ArgumentNullException(nameof(price));
        if (price.Amount <= 0)
            throw new DomainException("Service price must be greater than zero");
        
        Id = Guid.NewGuid();
        Name = name;
        Price = price;
    }
}