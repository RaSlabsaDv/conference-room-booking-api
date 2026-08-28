using MediatR;

public sealed class GetRevenueReportQueryHandler
(
    IBookingRepository bookingRepository
) : IRequestHandler<GetRevenueReportQuery, RevenueReportDto>
{
    public async Task<RevenueReportDto> Handle(GetRevenueReportQuery request, CancellationToken ct)
    {
        var revenue = await bookingRepository.GetRevenueAsync(request.From, request.To, ct);
        var bookingsCount = await bookingRepository.GetBookingsCountAsync(request.From, request.To, ct);

        return new RevenueReportDto
        (
            request.From,
            request.To,
            revenue,
            "UAH",
            bookingsCount
        );
    }
}