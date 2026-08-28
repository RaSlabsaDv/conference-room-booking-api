public sealed record UpdateRoomRequest
(
    string Name,
    int Capacity,
    decimal Amount,
    string Currency);