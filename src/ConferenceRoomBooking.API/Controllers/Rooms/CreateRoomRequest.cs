public sealed record CreateRoomRequest
(
    string Name, 
    int Capacity,
    decimal Amount,
    string Currency = "UAH");