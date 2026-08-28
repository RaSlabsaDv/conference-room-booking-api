using FluentValidation;

public sealed class RemoveServiceFromRoomCommandValidator : AbstractValidator<RemoveServiceFromRoomCommand>
{
    public RemoveServiceFromRoomCommandValidator()
    {
        RuleFor(x => x.RoomId)
            .NotEmpty().WithMessage("Room ID is required.");

        RuleFor(x => x.ServiceId)
            .NotEmpty().WithMessage("Service ID is required.");
    }
}