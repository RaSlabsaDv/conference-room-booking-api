using MediatR;

public sealed record CreateRoomCommand
(
    string Name, 
    int Capacity,
    decimal Amount,
    string Currency = "UAH") : IRequest<Guid>;