using FluentValidation;

public sealed class SetRoomUnderMaintenanceCommandValidator : AbstractValidator<SetRoomUnderMaintenanceCommand>
{
    public SetRoomUnderMaintenanceCommandValidator()
    {
        RuleFor(x => x.RoomId)
            .NotEmpty();
    }
}