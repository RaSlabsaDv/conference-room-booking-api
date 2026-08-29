using FluentAssertions;

public class PricingCalculatorTests
{
    private readonly PricingCalculator _calculator = new(new StaticPricingRuleProvider());

    [Fact]
    public void Calculate_EntirelyWithinStandardPeriod_ReturnsBaseRateTimesHours()
    {
        // Arrange
        var room = new Room("Room A", 50, new Money(2000));
        var start = new DateTime(2026, 9, 1, 9, 0, 0, DateTimeKind.Utc);
        var end = new DateTime(2026, 9, 1, 11, 0, 0, DateTimeKind.Utc);

        // Act
        var result = _calculator.Calculate(room, start, end, []);

        // Assert
        result.Amount.Should().Be(4000); // 2 hours * 2000
    }

    [Fact]
    public void Calculate_EntirelyWithinMorningPeriod_AppliesDiscount()
    {
        // Arrange
        var room = new Room("Room A", 50, new Money(2000));
        var start = new DateTime(2026, 9, 1, 6, 0, 0, DateTimeKind.Utc);
        var end = new DateTime(2026, 9, 1, 8, 0, 0, DateTimeKind.Utc);

        // Act
        var result = _calculator.Calculate(room, start, end, []);

        // Assert
        result.Amount.Should().Be(3600); // 2 hours * 2000 * 0.9
    }

    [Fact]
    public void Calculate_EntirelyWithinEveningPeriod_AppliesDiscount()
    {
        // Arrange
        var room = new Room("Room A", 50, new Money(2000));
        var start = new DateTime(2026, 9, 1, 18, 0, 0, DateTimeKind.Utc);
        var end = new DateTime(2026, 9, 1, 20, 0, 0, DateTimeKind.Utc);

        // Act
        var result = _calculator.Calculate(room, start, end, []);

        // Assert
        result.Amount.Should().Be(3200); // 2 hours * 2000 * 0.8
    }

    [Fact]
    public void Calculate_EntirelyWithinPeakPeriod_AppliesSurcharge()
    {
        // Arrange
        var room = new Room("Room A", 50, new Money(2000));
        var start = new DateTime(2026, 9, 1, 12, 0, 0, DateTimeKind.Utc);
        var end = new DateTime(2026, 9, 1, 14, 0, 0, DateTimeKind.Utc);

        // Act
        var result = _calculator.Calculate(room, start, end, []);

        // Assert
        result.Amount.Should().Be(4600); // 2 hours * 2000 * 1.15
    }

    [Fact]
    public void Calculate_SpanningStandardAndPeakPeriods_SumsEachBlockSeparately()
    {
        // Arrange
        var room = new Room("Room A", 50, new Money(2000));
        var start = new DateTime(2026, 9, 1, 11, 0, 0, DateTimeKind.Utc);
        var end = new DateTime(2026, 9, 1, 13, 0, 0, DateTimeKind.Utc);

        // Act
        var result = _calculator.Calculate(room, start, end, []);

        // Assert
        // 11:00-12:00 standard (2000) + 12:00-13:00 peak (2000 * 1.15 = 2300)
        result.Amount.Should().Be(4300);
    }

    [Fact]
    public void Calculate_SpanningMorningAndStandardPeriods_SumsEachBlockSeparately()
    {
        // Arrange
        var room = new Room("Room A", 50, new Money(2000));
        var start = new DateTime(2026, 9, 1, 8, 0, 0, DateTimeKind.Utc);
        var end = new DateTime(2026, 9, 1, 10, 0, 0, DateTimeKind.Utc);

        // Act
        var result = _calculator.Calculate(room, start, end, []);

        // Assert
        // 08:00-09:00 morning (2000 * 0.9 = 1800) + 09:00-10:00 standard (2000)
        result.Amount.Should().Be(3800);
    }

    [Fact]
    public void Calculate_HalfHourBlock_ChargesHalfTheHourlyRate()
    {
        // Arrange
        var room = new Room("Room A", 50, new Money(2000));
        var start = new DateTime(2026, 9, 1, 10, 0, 0, DateTimeKind.Utc);
        var end = new DateTime(2026, 9, 1, 10, 30, 0, DateTimeKind.Utc);

        // Act
        var result = _calculator.Calculate(room, start, end, []);

        // Assert
        result.Amount.Should().Be(1000); // half of 2000
    }

    [Fact]
    public void Calculate_WithSelectedServices_AddsServicePricesOnTop()
    {
        // Arrange
        var room = new Room("Room A", 50, new Money(2000));
        var start = new DateTime(2026, 9, 1, 9, 0, 0, DateTimeKind.Utc);
        var end = new DateTime(2026, 9, 1, 10, 0, 0, DateTimeKind.Utc);
        var services = new List<Service>
        {
            new(room.Id, "Projector", new Money(500)),
            new(room.Id, "Wi-Fi", new Money(300))
        };

        // Act
        var result = _calculator.Calculate(room, start, end, services);

        // Assert
        // 1 hour standard (2000) + Projector (500) + Wi-Fi (300)
        result.Amount.Should().Be(2800);
    }

    [Fact]
    public void Calculate_WithNoServices_ReturnsRoomCostOnly()
    {
        // Arrange
        var room = new Room("Room A", 50, new Money(2000));
        var start = new DateTime(2026, 9, 1, 9, 0, 0, DateTimeKind.Utc);
        var end = new DateTime(2026, 9, 1, 10, 0, 0, DateTimeKind.Utc);

        // Act
        var result = _calculator.Calculate(room, start, end, []);

        // Assert
        result.Amount.Should().Be(2000);
    }

    [Fact]
    public void Calculate_WithZeroBaseRate_ReturnsZeroRoomCost()
    {
        // Arrange
        var room = new Room("Room A", 50, new Money(0));
        var start = new DateTime(2026, 9, 1, 9, 0, 0, DateTimeKind.Utc);
        var end = new DateTime(2026, 9, 1, 11, 0, 0, DateTimeKind.Utc);

        // Act
        var result = _calculator.Calculate(room, start, end, []);

        // Assert
        result.Amount.Should().Be(0);
    }
}