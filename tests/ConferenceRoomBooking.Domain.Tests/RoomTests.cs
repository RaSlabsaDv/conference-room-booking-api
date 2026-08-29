using FluentAssertions;

public class RoomTests
{
    [Fact]
    public void Constructor_WithValidData_CreatesRoomSuccessfully()
    {
        // Arrange & Act
        var room = new Room("Room A", 50, new Money(2000));

        // Assert
        room.Name.Should().Be("Room A");
        room.Capacity.Should().Be(50);
        room.BaseHourlyRate.Should().Be(new Money(2000));
        room.RoomStatus.Should().Be(RoomStatus.Active);
        room.Services.Should().BeEmpty();
    }

    [Fact]
    public void Constructor_WithEmptyName_ThrowsDomainException()
    {
        // Arrange
        var act = () => new Room("", 50, new Money(2000));

        // Act & Assert
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Constructor_WithWhitespaceName_ThrowsDomainException()
    {
        // Arrange
        var act = () => new Room("   ", 50, new Money(2000));

        // Act & Assert
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Constructor_WithZeroCapacity_ThrowsDomainException()
    {
        // Arrange
        var act = () => new Room("Room A", 0, new Money(2000));

        // Act & Assert
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Constructor_WithNegativeCapacity_ThrowsDomainException()
    {
        // Arrange
        var act = () => new Room("Room A", -5, new Money(2000));

        // Act & Assert
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void UpdateCapacity_WithValidValue_UpdatesCapacity()
    {
        // Arrange
        var room = new Room("Room A", 50, new Money(2000));

        // Act
        room.UpdateCapacity(80);

        // Assert
        room.Capacity.Should().Be(80);
    }

    [Fact]
    public void UpdateCapacity_WithZeroOrNegative_ThrowsDomainException()
    {
        // Arrange
        var room = new Room("Room A", 50, new Money(2000));
        var act = () => room.UpdateCapacity(0);

        // Act & Assert
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void UpdateBaseRate_WithValidRate_UpdatesRate()
    {
        // Arrange
        var room = new Room("Room A", 50, new Money(2000));
        var newRate = new Money(2500);

        // Act
        room.UpdateBaseRate(newRate);

        // Assert
        room.BaseHourlyRate.Should().Be(newRate);
    }

    [Fact]
    public void Rename_WithValidName_UpdatesName()
    {
        // Arrange
        var room = new Room("Room A", 50, new Money(2000));

        // Act
        room.Rename("Room A1");

        // Assert
        room.Name.Should().Be("Room A1");
    }

    [Fact]
    public void Rename_WithEmptyName_ThrowsDomainException()
    {
        // Arrange
        var room = new Room("Room A", 50, new Money(2000));
        var act = () => room.Rename("");

        // Act & Assert
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void AddService_WithValidData_AddsServiceSuccessfully()
    {
        // Arrange
        var room = new Room("Room A", 50, new Money(2000));

        // Act
        room.AddService("Projector", new Money(500));

        // Assert
        room.Services.Should().ContainSingle(s => s.Name == "Projector");
    }

    [Fact]
    public void AddService_WithDuplicateName_ThrowsDomainException()
    {
        // Arrange
        var room = new Room("Room A", 50, new Money(2000));
        room.AddService("Projector", new Money(500));

        // Act
        var act = () => room.AddService("Projector", new Money(600));

        // Assert
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void AddService_WithDuplicateNameDifferentCase_ThrowsDomainException()
    {
        // Arrange
        var room = new Room("Room A", 50, new Money(2000));
        room.AddService("Projector", new Money(500));

        // Act
        var act = () => room.AddService("PROJECTOR", new Money(600));

        // Assert
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void AddService_WhenRoomIsDeleted_ThrowsDomainException()
    {
        // Arrange
        var room = new Room("Room A", 50, new Money(2000));
        room.MarkAsDeleted();

        // Act
        var act = () => room.AddService("Projector", new Money(500));

        // Assert
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void RemoveService_WhenServiceExists_RemovesSuccessfully()
    {
        // Arrange
        var room = new Room("Room A", 50, new Money(2000));
        room.AddService("Projector", new Money(500));
        var serviceId = room.Services.Single().Id;

        // Act
        room.RemoveService(serviceId);

        // Assert
        room.Services.Should().BeEmpty();
    }

    [Fact]
    public void RemoveService_WhenServiceNotFound_ThrowsDomainException()
    {
        // Arrange
        var room = new Room("Room A", 50, new Money(2000));
        var act = () => room.RemoveService(Guid.NewGuid());

        // Act & Assert
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void HasCapacityFor_WhenActiveAndEnoughCapacity_ReturnsTrue()
    {
        // Arrange
        var room = new Room("Room A", 50, new Money(2000));

        // Act
        var result = room.HasCapacityFor(40);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void HasCapacityFor_WhenCapacityNotEnough_ReturnsFalse()
    {
        // Arrange
        var room = new Room("Room A", 50, new Money(2000));

        // Act
        var result = room.HasCapacityFor(60);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void HasCapacityFor_WhenRoomIsUnderMaintenance_ReturnsFalse()
    {
        // Arrange
        var room = new Room("Room A", 50, new Money(2000));
        room.SetUnderMaintenance();

        // Act
        var result = room.HasCapacityFor(40);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void HasCapacityFor_WhenRoomIsDeleted_ReturnsFalse()
    {
        // Arrange
        var room = new Room("Room A", 50, new Money(2000));
        room.MarkAsDeleted();

        // Act
        var result = room.HasCapacityFor(40);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void SetUnderMaintenance_WhenActive_ChangesStatus()
    {
        // Arrange
        var room = new Room("Room A", 50, new Money(2000));

        // Act
        room.SetUnderMaintenance();

        // Assert
        room.RoomStatus.Should().Be(RoomStatus.UnderMaintenance);
    }

    [Fact]
    public void SetUnderMaintenance_WhenDeleted_ThrowsDomainException()
    {
        // Arrange
        var room = new Room("Room A", 50, new Money(2000));
        room.MarkAsDeleted();

        // Act
        var act = () => room.SetUnderMaintenance();

        // Assert
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Reactivate_WhenUnderMaintenance_ChangesStatusToActive()
    {
        // Arrange
        var room = new Room("Room A", 50, new Money(2000));
        room.SetUnderMaintenance();

        // Act
        room.Reactivate();

        // Assert
        room.RoomStatus.Should().Be(RoomStatus.Active);
    }

    [Fact]
    public void Reactivate_WhenDeleted_ThrowsDomainException()
    {
        // Arrange
        var room = new Room("Room A", 50, new Money(2000));
        room.MarkAsDeleted();

        // Act
        var act = () => room.Reactivate();

        // Assert
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void MarkAsDeleted_ChangesStatusToDeleted()
    {
        // Arrange
        var room = new Room("Room A", 50, new Money(2000));

        // Act
        room.MarkAsDeleted();

        // Assert
        room.RoomStatus.Should().Be(RoomStatus.Deleted);
    }
}