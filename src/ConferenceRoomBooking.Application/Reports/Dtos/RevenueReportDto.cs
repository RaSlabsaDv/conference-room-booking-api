public sealed record RevenueReportDto
(
    DateTime From, 
    DateTime To, 
    decimal TotalRevenue, 
    string Currency, 
    int BookingsCount);