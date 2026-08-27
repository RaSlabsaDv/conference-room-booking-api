public record Money
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency = "UAH")
    {
        if (amount < 0)
            throw new DomainException("Amount cannot be negative");
        
        Amount = amount;
        Currency = currency;
    }

    public static Money Zero => new(0);

    public Money Add(Money other)
    {
        EnsureSameCurrency(other);
        return new Money(Amount + other.Amount, Currency);
    }

    public Money Multiply(decimal factor) => new(Amount * factor, Currency);

    private void EnsureSameCurrency(Money other)
    {
        if (Currency != other.Currency)
            throw new DomainException("Cannot operate on money with different currency");
    }
}