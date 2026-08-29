using FluentAssertions;

public class MoneyTest
{
    [Fact]
    public void Constructor_WithNegativeAmount_ThrowsDomainException()
    {
        // Arrange 
        var negativeAmount = -100m;

        // Act
        var act = () => new Money(negativeAmount);

        // Assert
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Constructor_WithZeroAmount_DoesNotThrow()
    {
        // Arrange & act
        var act = () => new Money(0);

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void Add_WithSameCurrency_ReturnsSum()
    {
        // Arrange
        var first = new Money(100);
        var second = new Money(150);

        // Act
        var result = first.Add(second);

        // Assert
        result.Amount.Should().Be(250);
        result.Currency.Should().Be("UAH");
    }

     [Fact]
    public void Add_WithDifferentCurrency_ThrowsDomainException()
    {
        // Arrange
        var uah = new Money(100, "UAH");
        var usd = new Money(50, "USD");

        // Act
        var act = () => uah.Add(usd);

        // Assert
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Multiply_ByFactor_ReturnsScaledAmount()
    {
        // Arrange
        var money = new Money(1000);

        // Act
        var result = money.Multiply(0.8m);

        // Assert
        result.Amount.Should().Be(800);
    }

    [Fact]
    public void Equality_TwoInstancesWithSameValues_AreEqual()
    {
        // Arrange
        var first = new Money(100, "UAH");
        var second = new Money(100, "UAH");

        // Act & Assert
        first.Should().Be(second);
    }
}
