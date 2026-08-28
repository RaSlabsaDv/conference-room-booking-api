using FluentValidation;

public sealed class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
{
    public CreateBookingCommandValidator()
    {
        RuleFor(x => x.RoomId)
            .NotEmpty();

        RuleFor(x => x.StartTime)
            .GreaterThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Start time cannot be in the past");

        RuleFor(x => x.EndTime)
            .GreaterThan(x => x.StartTime)
            .WithMessage("End time must be after start time");

        RuleFor(x => x.SelectedServiceIds)
            .NotNull()
            .WithMessage("Selected services list cannot be null");

        RuleForEach(x => x.SelectedServiceIds)
            .NotEmpty()
            .WithMessage("Service id cannot be empty");
    }
}