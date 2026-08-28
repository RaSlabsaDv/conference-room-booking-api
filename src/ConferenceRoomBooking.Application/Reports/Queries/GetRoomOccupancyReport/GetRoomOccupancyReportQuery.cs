using MediatR;

public sealed record GetRoomOccupancyReportQuery
(
    DateTime From, 
    DateTime To) : IRequest<RoomOccupancyReportDto>; 