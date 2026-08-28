using FluentValidation;

public sealed class DeleteRoomCommandValidator : AbstractValidator<DeleteRoomCommand>
{
    public DeleteRoomCommandValidator()
    {
        RuleFor(x => x.RoomId)
            .NotEmpty();
    }
}