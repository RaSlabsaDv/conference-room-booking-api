using FluentValidation;

public sealed class GetRoomOccupancyReportQueryValidator : AbstractValidator<GetRoomOccupancyReportQuery>
{
    public GetRoomOccupancyReportQueryValidator()
    {
        RuleFor(x => x.From)
            .LessThan(x => x.To)
            .WithMessage("'From' date must be earlier than 'To' date");

        RuleFor(x => x.To)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("'To' date cannot be in the future");
    }
}