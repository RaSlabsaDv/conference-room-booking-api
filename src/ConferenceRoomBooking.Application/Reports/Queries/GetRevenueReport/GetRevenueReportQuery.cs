using MediatR;

public sealed record GetRevenueReportQuery
(
    DateTime From, 
    DateTime To) : IRequest<RevenueReportDto>; 