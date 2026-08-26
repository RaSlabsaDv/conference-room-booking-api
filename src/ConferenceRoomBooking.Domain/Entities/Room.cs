public class Room
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public int Capacity { get; private set; }
    public Money BaseHourlyRate { get; private set; } = null!;
    public bool IsDeleted { get; private set; }

    private readonly List<Service> _services = [];
    public IReadOnlyCollection<Service> Services => _services;
    

    private Room(){}

    public Room(string name, int capacity, Money baseHourlyRate)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Room name cannot be empty");
        if (capacity <= 0)
            throw new DomainException("Room capacity must be grater than zero");
        
        Id = Guid.NewGuid();
        Name = name;
        Capacity = capacity;
        BaseHourlyRate = baseHourlyRate ?? throw new ArgumentNullException(nameof(baseHourlyRate));
        IsDeleted = false;
    }

    public void UpdateCapacity(int newCapacity)
    {
        if (newCapacity <= 0)
            throw new DomainException("Room capacity must be greater than zero");
    
        Capacity = newCapacity;
    }

    public void UpdateBaseRate(Money newRate)
    {
        BaseHourlyRate = newRate ?? throw new ArgumentNullException(nameof(newRate));
    }

    public void AddService(Service service)
    {
        if (_services.Any(s => s.Name == service.Name))
            throw new DomainException($"Service '{service.Name}' is already added");
        
        _services.Add(service);
    }

    public void RemoveService(Guid serviceId)
    {
        var service = _services.FirstOrDefault(s => s.Id == serviceId)
            ?? throw new DomainException("Service not found in the room");

        _services.Remove(service);
    }

    public void MarkAsDeleted() => IsDeleted = true;

    public bool HasCapacityFor(int people) => !IsDeleted && Capacity >= people;
}