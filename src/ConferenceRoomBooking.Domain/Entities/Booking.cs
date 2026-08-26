public class Booking
{
    public Guid Id { get; private set; }
    public Guid RoomId { get; private set; }
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    public Money TotalPrice { get; private set; } = null!;
    public BookingStatus BookingStatus { get; private set; }

    private readonly List<Service> _selectedServices = [];
    public IReadOnlyCollection<Service> SelectedServices => _selectedServices;

    private Booking() {}

    public Booking(Guid roomId, DateTime startTime, DateTime endTime)
    {
        if(startTime >= endTime)
            throw new DomainException("End time must be after start time");

        Id = Guid.NewGuid();
        RoomId = roomId;
        StartTime = startTime;
        EndTime = endTime;
        BookingStatus = BookingStatus.Confirmed;
        TotalPrice = Money.Zero;
    }

    public bool OverlapsWith(DateTime otherStart, DateTime otherEnd)
    {
        return StartTime < otherEnd && otherStart < EndTime;
    }

    public void AddService(Service service)
    {
        if (BookingStatus == BookingStatus.Cancelled)
            throw new DomainException("Cannot edit cancelled booking");
        
        if (_selectedServices.Any(s => s.Id == service.Id))
            throw new DomainException($"Service '{service.Name}' is already added to this booking");

        _selectedServices.Add(service);
    }

    public void RemoveService(Guid serviceId)
    {
        if (BookingStatus == BookingStatus.Cancelled)
            throw new DomainException("Cannot edit cancelled booking");
    
        var service = _selectedServices.FirstOrDefault(s => s.Id == serviceId)
            ?? throw new DomainException("Service not found in this booking");
    
        _selectedServices.Remove(service);
    }

    public void SetTotalPrice(Money totalPrice)
    {
        TotalPrice = totalPrice ?? throw new ArgumentNullException(nameof(totalPrice));
    }

    public void Cancel()
    {
        if (BookingStatus == BookingStatus.Cancelled)
            throw new DomainException("Booking is already cancelled");

        BookingStatus = BookingStatus.Cancelled;
    }
}