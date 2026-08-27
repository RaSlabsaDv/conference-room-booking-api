public sealed record PricingRule(TimeOnly Start, TimeOnly End, decimal Multiplier)
{
    public bool Covers(TimeOnly time) => time >= Start && time < End;
}