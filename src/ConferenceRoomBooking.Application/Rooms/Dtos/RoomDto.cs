public sealed record RoomDto(
    Guid Id,
    string Name,
    int Capacity,
    decimal BaseHourlyRateAmount,
    string BaseHourlyRateCurrency,
    string Status,
    IReadOnlyCollection<ServiceDto> Services)
{
    public static RoomDto FromDomain(Room room) =>
        new(
            room.Id,
            room.Name,
            room.Capacity,
            room.BaseHourlyRate.Amount,
            room.BaseHourlyRate.Currency,
            room.RoomStatus.ToString(),
            room.Services.Select(ServiceDto.FromDomain).ToList());
}