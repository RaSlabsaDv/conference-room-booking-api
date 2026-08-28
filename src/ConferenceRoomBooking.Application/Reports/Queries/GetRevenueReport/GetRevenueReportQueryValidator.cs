using FluentValidation;

public sealed class GetRevenueReportQueryValidator : AbstractValidator<GetRevenueReportQuery>
{
    public GetRevenueReportQueryValidator()
    {
        RuleFor(x => x.From)
            .LessThan(x => x.To)
            .WithMessage("'From' date must be earlier than 'To' date");

        RuleFor(x => x.To)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("'To' date cannot be in the future");
    }
}