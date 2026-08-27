public class Room
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public int Capacity { get; private set; }
    public Money BaseHourlyRate { get; private set; } = null!;
    public RoomStatus RoomStatus { get; private set; }

    private readonly List<Service> _services = [];
    public IReadOnlyCollection<Service> Services => _services;
    

    private Room(){}

    public Room(string name, int capacity, Money baseHourlyRate)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Room name cannot be empty");
        if (capacity <= 0)
            throw new DomainException("Room capacity must be greater than zero");
        
        Id = Guid.NewGuid();
        Name = name;
        Capacity = capacity;
        BaseHourlyRate = baseHourlyRate ?? throw new ArgumentNullException(nameof(baseHourlyRate));
        RoomStatus = RoomStatus.Active;
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

    public void AddService(string name, Money price)
    {
        if (RoomStatus == RoomStatus.Deleted)
            throw new DomainException("Cannot add service to a deleted room");
    
        var trimmedName = name.Trim();
    
        if (_services.Any(s => string.Equals(s.Name, trimmedName, StringComparison.OrdinalIgnoreCase)))
            throw new DomainException($"Service '{trimmedName}' is already added to this room");
    
        var service = new Service(Id, trimmedName, price);
        _services.Add(service);
    }

    public void RemoveService(Guid serviceId)
    {
        var service = _services.FirstOrDefault(s => s.Id == serviceId)
            ?? throw new DomainException("Service not found in the room");

        _services.Remove(service);
    }

    public void MarkAsDeleted() => RoomStatus = RoomStatus.Deleted;

    public bool HasCapacityFor(int people) => RoomStatus == RoomStatus.Active && Capacity >= people;

    public void SetUnderMaintenance()
    {
        if(RoomStatus == RoomStatus.Deleted)
            throw new DomainException("Cannot change status of deleted room");

        RoomStatus = RoomStatus.UnderMaintenance;
    }

    public void Reactivate()
    {
        if(RoomStatus == RoomStatus.Deleted)
            throw new DomainException("Cannot change status of deleted room");

        RoomStatus = RoomStatus.Active;
    }
}