public sealed class StaticPricingRuleProvider : IPricingRuleProvider
{
    public IReadOnlyList<PricingRule> GetRules() => PricingRules.All;
}