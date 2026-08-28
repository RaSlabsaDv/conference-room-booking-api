using FluentValidation;

public sealed class ReactivateRoomCommandValidator : AbstractValidator<ReactivateRoomCommand>
{
    public ReactivateRoomCommandValidator()
    {
        RuleFor(x => x.RoomId)
            .NotEmpty();
    }
}