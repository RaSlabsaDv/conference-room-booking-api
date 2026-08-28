using FluentValidation;

public sealed class GetBookingByIdQueryValidator : AbstractValidator<GetBookingByIdQuery>
{
    public GetBookingByIdQueryValidator()
    {
        RuleFor(x => x.BookingId).NotEmpty();
    }
}