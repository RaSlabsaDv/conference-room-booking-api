public interface IPricingRuleProvider
{
    IReadOnlyList<PricingRule> GetRules();
}