public sealed record BookingDto(
    Guid Id,
    Guid RoomId,
    DateTime StartTime,
    DateTime EndTime,
    decimal TotalPriceAmount,
    string TotalPriceCurrency,
    string Status,
    IReadOnlyCollection<BookedServiceDto> SelectedServices
)
{
    public static BookingDto FromDomain(Booking booking) =>
        new(
            booking.Id,
            booking.RoomId,
            booking.StartTime,
            booking.EndTime,
            booking.TotalPrice.Amount,
            booking.TotalPrice.Currency,
            booking.BookingStatus.ToString(),
            booking.SelectedServices.Select(BookedServiceDto.FromDomain).ToList());
}