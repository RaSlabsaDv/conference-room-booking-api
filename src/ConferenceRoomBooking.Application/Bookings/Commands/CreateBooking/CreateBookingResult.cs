public sealed record CreateBookingResult(
    Guid BookingId,
    Guid RoomId,
    DateTime StartTime,
    DateTime EndTime,
    decimal TotalPriceAmount,
    string TotalPriceCurrency,
    IReadOnlyCollection<BookedServiceDto> SelectedServices
){
    public static CreateBookingResult FromDomain(Booking booking) =>
        new(
            booking.Id,
            booking.RoomId,
            booking.StartTime,
            booking.EndTime,
            booking.TotalPrice.Amount,
            booking.TotalPrice.Currency,
            booking.SelectedServices.Select(BookedServiceDto.FromDomain).ToList());
};