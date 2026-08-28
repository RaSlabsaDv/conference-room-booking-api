using FluentValidation;

public sealed class SearchAvailableRoomsQueryValidator : AbstractValidator<SearchAvailableRoomsQuery>
{
    public SearchAvailableRoomsQueryValidator()
    {
        RuleFor(x => x.Start)
            .GreaterThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Start time cannot be in the past");

        RuleFor(x => x.End)
            .GreaterThan(x => x.Start)
            .WithMessage("End time must be after start time");

        RuleFor(x => x.Capacity)
            .GreaterThan(0);
    }
}