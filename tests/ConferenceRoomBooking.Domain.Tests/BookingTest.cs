using FluentAssertions;

public class BookingTests
{
    [Fact]
    public void Constructor_WhenEndTimeBeforeStartTime_ThrowsDomainException()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var startTime = new DateTime(2026, 9, 1, 14, 0, 0, DateTimeKind.Utc);
        var endTime = new DateTime(2026, 9, 1, 10, 0, 0, DateTimeKind.Utc);

        // Act
        var act = () => new Booking(roomId, startTime, endTime);

        // Assert
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Constructor_WithValidTimes_CreatesBookingSuccessfully()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var startTime = new DateTime(2026, 9, 1, 10, 0, 0, DateTimeKind.Utc);
        var endTime = new DateTime(2026, 9, 1, 14, 0, 0, DateTimeKind.Utc);

        // Act
        var act = () => new Booking(roomId, startTime, endTime);

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void Constructor_WhenStartTimeBeforeAllowedHours_ThrowsDomainException()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var startTime = new DateTime(2026, 9, 1, 3, 0, 0, DateTimeKind.Utc);
        var endTime = new DateTime(2026, 9, 1, 8, 0, 0, DateTimeKind.Utc);

        // Act
        var act = () => new Booking(roomId, startTime, endTime);

        // Assert
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Constructor_WhenEndTimeAfterAllowedHours_ThrowsDomainException()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var startTime = new DateTime(2026, 9, 1, 21, 0, 0, DateTimeKind.Utc);
        var endTime = new DateTime(2026, 9, 1, 23, 30, 0, DateTimeKind.Utc);

        // Act
        var act = () => new Booking(roomId, startTime, endTime);

        // Assert
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Constructor_RoundsStartTimeDown_ToNearestHalfHour()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var startTime = new DateTime(2026, 9, 1, 10, 10, 0, DateTimeKind.Utc);
        var endTime = new DateTime(2026, 9, 1, 12, 0, 0, DateTimeKind.Utc);
    
        // Act
        var booking = new Booking(roomId, startTime, endTime);
    
        // Assert
        booking.StartTime.Should().Be(new DateTime(2026, 9, 1, 10, 0, 0, DateTimeKind.Utc));
    }
    
    [Fact]
    public void Constructor_RoundsEndTimeUp_ToNearestHalfHour()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var startTime = new DateTime(2026, 9, 1, 10, 0, 0, DateTimeKind.Utc);
        var endTime = new DateTime(2026, 9, 1, 10, 10, 0, DateTimeKind.Utc);
    
        // Act
        var booking = new Booking(roomId, startTime, endTime);
    
        // Assert
        booking.EndTime.Should().Be(new DateTime(2026, 9, 1, 10, 30, 0, DateTimeKind.Utc));
    }
    
    [Fact]
    public void OverlapsWith_WhenIntervalsOverlap_ReturnsTrue()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var booking = new Booking(roomId,
            new DateTime(2026, 9, 1, 9, 0, 0, DateTimeKind.Utc),
            new DateTime(2026, 9, 1, 11, 0, 0, DateTimeKind.Utc));
    
        // Act
        var result = booking.OverlapsWith(
            new DateTime(2026, 9, 1, 10, 0, 0, DateTimeKind.Utc),
            new DateTime(2026, 9, 1, 12, 0, 0, DateTimeKind.Utc));
    
        // Assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public void OverlapsWith_WhenIntervalsAreAdjacent_ReturnsFalse()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var booking = new Booking(roomId,
            new DateTime(2026, 9, 1, 9, 0, 0, DateTimeKind.Utc),
            new DateTime(2026, 9, 1, 11, 0, 0, DateTimeKind.Utc));
    
        // Act
        var result = booking.OverlapsWith(
            new DateTime(2026, 9, 1, 11, 0, 0, DateTimeKind.Utc),
            new DateTime(2026, 9, 1, 13, 0, 0, DateTimeKind.Utc));
    
        // Assert
        result.Should().BeFalse();
    }
    
    [Fact]
    public void OverlapsWith_WhenIntervalsDoNotOverlap_ReturnsFalse()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var booking = new Booking(roomId,
            new DateTime(2026, 9, 1, 9, 0, 0, DateTimeKind.Utc),
            new DateTime(2026, 9, 1, 11, 0, 0, DateTimeKind.Utc));
    
        // Act
        var result = booking.OverlapsWith(
            new DateTime(2026, 9, 1, 15, 0, 0, DateTimeKind.Utc),
            new DateTime(2026, 9, 1, 17, 0, 0, DateTimeKind.Utc));
    
        // Assert
        result.Should().BeFalse();
    }
    
    [Fact]
    public void Cancel_WhenBookingIsConfirmedAndFarInFuture_ChangesStatusToCancelled()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var booking = new Booking(roomId,
            DateTime.UtcNow.AddDays(2).Date.AddHours(10),
            DateTime.UtcNow.AddDays(2).Date.AddHours(12));
    
        // Act
        booking.Cancel();
    
        // Assert
        booking.BookingStatus.Should().Be(BookingStatus.Cancelled);
    }
    
    [Fact]
    public void Cancel_WhenAlreadyCancelled_ThrowsDomainException()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var booking = new Booking(roomId,
            DateTime.UtcNow.AddDays(2).Date.AddHours(10),
            DateTime.UtcNow.AddDays(2).Date.AddHours(12));
        booking.Cancel();
    
        // Act
        var act = () => booking.Cancel();
    
        // Assert
        act.Should().Throw<DomainException>();
    }
    
    [Fact]
    public void Cancel_WhenLessThan8HoursBeforeStart_ThrowsDomainException()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var nearStart = DateTime.UtcNow.AddHours(2);
        var booking = new Booking(roomId, nearStart, nearStart.AddHours(2));
    
        // Act
        var act = () => booking.Cancel();
    
        // Assert
        act.Should().Throw<DomainException>();
    }
    
    [Fact]
    public void AddService_WhenServiceBelongsToBookedRoom_AddsSuccessfully()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var booking = new Booking(roomId,
            new DateTime(2026, 9, 1, 10, 0, 0, DateTimeKind.Utc),
            new DateTime(2026, 9, 1, 12, 0, 0, DateTimeKind.Utc));
        var service = new Service(roomId, "Projector", new Money(500));
    
        // Act
        booking.AddService(service);
    
        // Assert
        booking.SelectedServices.Should().ContainSingle(s => s.ServiceId == service.Id);
    }
    
    [Fact]
    public void AddService_WhenServiceBelongsToDifferentRoom_ThrowsDomainException()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var otherRoomId = Guid.NewGuid();
        var booking = new Booking(roomId,
            new DateTime(2026, 9, 1, 10, 0, 0, DateTimeKind.Utc),
            new DateTime(2026, 9, 1, 12, 0, 0, DateTimeKind.Utc));
        var service = new Service(otherRoomId, "Projector", new Money(500));
    
        // Act
        var act = () => booking.AddService(service);
    
        // Assert
        act.Should().Throw<DomainException>();
    }
    
    [Fact]
    public void AddService_WhenServiceAlreadyAdded_ThrowsDomainException()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var booking = new Booking(roomId,
            new DateTime(2026, 9, 1, 10, 0, 0, DateTimeKind.Utc),
            new DateTime(2026, 9, 1, 12, 0, 0, DateTimeKind.Utc));
        var service = new Service(roomId, "Projector", new Money(500));
        booking.AddService(service);
    
        // Act
        var act = () => booking.AddService(service);
    
        // Assert
        act.Should().Throw<DomainException>();
    }
    
    [Fact]
    public void AddService_WhenBookingIsCancelled_ThrowsDomainException()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var booking = new Booking(roomId,
            DateTime.UtcNow.AddDays(2).Date.AddHours(10),
            DateTime.UtcNow.AddDays(2).Date.AddHours(12));
        booking.Cancel();
        var service = new Service(roomId, "Projector", new Money(500));
    
        // Act
        var act = () => booking.AddService(service);
    
        // Assert
        act.Should().Throw<DomainException>();
    }
    
    [Fact]
    public void RemoveService_WhenServiceExists_RemovesSuccessfully()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var booking = new Booking(roomId,
            new DateTime(2026, 9, 1, 10, 0, 0, DateTimeKind.Utc),
            new DateTime(2026, 9, 1, 12, 0, 0, DateTimeKind.Utc));
        var service = new Service(roomId, "Projector", new Money(500));
        booking.AddService(service);
    
        // Act
        booking.RemoveService(service.Id);
    
        // Assert
        booking.SelectedServices.Should().BeEmpty();
    }
    
    [Fact]
    public void RemoveService_WhenServiceNotFound_ThrowsDomainException()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var booking = new Booking(roomId,
            new DateTime(2026, 9, 1, 10, 0, 0, DateTimeKind.Utc),
            new DateTime(2026, 9, 1, 12, 0, 0, DateTimeKind.Utc));
    
        // Act
        var act = () => booking.RemoveService(Guid.NewGuid());
    
        // Assert
        act.Should().Throw<DomainException>();
    }
    
    [Fact]
    public void SetTotalPrice_WithValidMoney_UpdatesTotalPrice()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var booking = new Booking(roomId,
            new DateTime(2026, 9, 1, 10, 0, 0, DateTimeKind.Utc),
            new DateTime(2026, 9, 1, 12, 0, 0, DateTimeKind.Utc));
        var price = new Money(4000);
    
        // Act
        booking.SetTotalPrice(price);
    
        // Assert
        booking.TotalPrice.Should().Be(price);
    }
    
    [Fact]
    public void SetTotalPrice_WithNull_ThrowsArgumentNullException()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var booking = new Booking(roomId,
            new DateTime(2026, 9, 1, 10, 0, 0, DateTimeKind.Utc),
            new DateTime(2026, 9, 1, 12, 0, 0, DateTimeKind.Utc));
    
        // Act
        var act = () => booking.SetTotalPrice(null!);
    
        // Assert
        act.Should().Throw<ArgumentNullException>();
    }
}