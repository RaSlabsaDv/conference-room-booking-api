public sealed record UpdateRoomResult
(
    Guid RoomId, 
    string Name, 
    int Capacity, 
    decimal Amount, 
    string Currency);