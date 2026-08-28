public class Booking
{
    public Guid Id { get; private set; }
    public Guid RoomId { get; private set; }
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    public Money TotalPrice { get; private set; } = null!;
    public BookingStatus BookingStatus { get; private set; }

    private readonly List<BookedService> _selectedServices = [];
    public IReadOnlyCollection<BookedService> SelectedServices => _selectedServices;

    private Booking() {}

    public Booking(Guid roomId, DateTime startTime, DateTime endTime)
    {
        if (startTime >= endTime)
            throw new DomainException("End time must be after start time");
    
        var roundedStart = BookingTimeRounding.RoundStartDown(startTime);
        var roundedEnd = BookingTimeRounding.RoundEndUp(endTime);
    
        if (roundedStart >= roundedEnd)
            throw new DomainException("End time must be after start time");
    
        ValidateWithinAllowedHours(roundedStart, roundedEnd);
    
        Id = Guid.NewGuid();
        RoomId = roomId;
        StartTime = roundedStart;
        EndTime = roundedEnd;
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

        if (service.RoomId != RoomId)
            throw new DomainException($"Service '{service.Name}' does not belong to the booked room");

        if (_selectedServices.Any(s => s.ServiceId == service.Id))
            throw new DomainException($"Service '{service.Name}' is already added to this booking");

        _selectedServices.Add(new BookedService(service));
    }

    public void RemoveService(Guid serviceId)
    {
        if (BookingStatus == BookingStatus.Cancelled)
            throw new DomainException("Cannot edit cancelled booking");

        var service = _selectedServices.FirstOrDefault(s => s.ServiceId == serviceId)
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
    
        if (StartTime <= DateTime.UtcNow.AddHours(8))
            throw new DomainException("Cannot cancel booking less than 8 hours before start time");
    
        BookingStatus = BookingStatus.Cancelled;
    }

    private static void ValidateWithinAllowedHours(DateTime startTime, DateTime endTime)
    {
        var start = TimeOnly.FromDateTime(startTime);
        var end = TimeOnly.FromDateTime(endTime);

        if (start < PricingRules.EarliestAllowed || end > PricingRules.LatestAllowed)
            throw new DomainException(
                $"Booking must be within allowed hours ({PricingRules.EarliestAllowed}–{PricingRules.LatestAllowed})");
    }
}