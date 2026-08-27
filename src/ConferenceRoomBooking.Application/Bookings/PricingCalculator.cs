public sealed class PricingCalculator(IPricingRuleProvider ruleProvider) : IPricingCalculator
{
    private static readonly TimeSpan BlockDuration = TimeSpan.FromMinutes(30);

    public Money Calculate
    (
        Room room, 
        DateTime startTime, 
        DateTime endTime, 
        IReadOnlyCollection<Service> selectedServices)
    {
        var roomCost = CalculateRoomCost(room, startTime, endTime);
        var servicesCost = selectedServices
            .Select(s => s.Price)
            .Aggregate(Money.Zero, (total, price) => total.Add(price));

        return roomCost.Add(servicesCost);
    }

    private Money CalculateRoomCost(Room room, DateTime startTime, DateTime endTime)
    {
        var rules = ruleProvider.GetRules();
        var total = Money.Zero;

        for (var blockStart = startTime; blockStart < endTime; blockStart += BlockDuration)
        {
            var rule = FindApplicableRule(rules, TimeOnly.FromDateTime(blockStart));
            var blockCost = room.BaseHourlyRate.Multiply(0.5m * rule.Multiplier);

            total = total.Add(blockCost);
        }

        return total;
    }

    private static PricingRule FindApplicableRule(IReadOnlyList<PricingRule> rules, TimeOnly blockStart)
    {
        foreach (var rule in rules)
        {
            if (rule.Covers(blockStart))
                return rule;
        }

        throw new DomainException($"No pricing rule covers time {blockStart}");
    }
}