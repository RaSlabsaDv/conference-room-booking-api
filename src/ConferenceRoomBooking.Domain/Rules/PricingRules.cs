public static class PricingRules
{
    // The order is important: they are checked from top to bottom, and the first match wins.
    // The range (12–14) is a subset of the standard range (9–18), so it comes first.

    public static readonly IReadOnlyList<PricingRule> All =
    [
        new(new TimeOnly(12, 0), new TimeOnly(14, 0), 1.15m), // peak
        new(new TimeOnly(18, 0), new TimeOnly(23, 0), 0.80m), // evening
        new(new TimeOnly(6, 0),  new TimeOnly(9, 0),  0.90m), // morning
        new(new TimeOnly(9, 0),  new TimeOnly(18, 0), 1.00m), // standart
    ];

    public static readonly TimeOnly EarliestAllowed = new(6, 0);
    public static readonly TimeOnly LatestAllowed = new(23, 0);
}