public sealed record BookedServiceDto(
    Guid ServiceId,
    string Name,
    decimal Price,
    string Currency
)
{
    public static BookedServiceDto FromDomain(BookedService bookedService) =>
        new(bookedService.ServiceId, bookedService.Name, bookedService.Price.Amount, bookedService.Price.Currency);
}