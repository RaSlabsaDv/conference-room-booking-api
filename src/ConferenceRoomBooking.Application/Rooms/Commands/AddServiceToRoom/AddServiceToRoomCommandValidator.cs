using FluentValidation;

public sealed class AddServiceToRoomCommandValidator : AbstractValidator<AddServiceToRoomCommand>
{
    public AddServiceToRoomCommandValidator()
    {
        RuleFor(x => x.RoomId)
            .NotEmpty().WithMessage("Room ID is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Service name is required.")
            .MaximumLength(150).WithMessage("Service name must not exceed 150 characters.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Service price must be greater than zero.");

        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage("Currency is required.")
            .Length(3).WithMessage("Currency must be a valid 3-letter ISO code (e.g. UAH, USD).");
    }
}