public sealed record ServiceDto(Guid Id, string Name, decimal Price, string Currency)
{
    public static ServiceDto FromDomain(Service service) =>
        new(service.Id, service.Name, service.Price.Amount, service.Price.Currency);
}