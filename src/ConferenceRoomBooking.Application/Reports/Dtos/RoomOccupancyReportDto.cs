public sealed record RoomOccupancyItemDto
(   Guid RoomId, 
    string RoomName, 
    double TotalBookedHours, 
    int BookingsCount);

public sealed record RoomOccupancyReportDto
(
    DateTime From, 
    DateTime To, 
    IReadOnlyCollection<RoomOccupancyItemDto> Rooms);