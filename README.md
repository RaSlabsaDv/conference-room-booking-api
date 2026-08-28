# Conference Room Booking API

![.NET](https://img.shields.io/badge/.NET-10-512BD4?logo=dotnet)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16-4169E1?logo=postgresql&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-ready-2496ED?logo=docker&logoColor=white)

REST API for managing conference room rentals: searching available rooms,
booking with automatic rental cost calculation, managing additional
services, and business analytics reports.

## Tech Stack

- **.NET 10** / ASP.NET Core Web API
- **PostgreSQL 16** — primary database
- **Entity Framework Core** — ORM, migrations
- **MediatR** — CQRS (commands/queries)
- **FluentValidation** — input validation
- **Swagger / Swashbuckle** — API documentation
- **Docker Compose** — database provisioning

## Architecture

The project follows **Clean Architecture** principles, split into four layers:

- **Domain** — entities, value objects, business rules. No external dependencies.
- **Application** — use cases (CQRS commands/queries via MediatR), repository interfaces, validation.
- **Infrastructure** — EF Core persistence, repository implementations, external concerns.
- **API** — controllers, middleware, Swagger configuration.

Dependencies point inward: `API → Infrastructure → Application → Domain`.

## How to Run

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/)

### Steps

1. Start the PostgreSQL database:

```bash
   docker-compose up -d
```

2. Run the API:

```bash
   cd src/ConferenceRoomBooking.API
   dotnet run
```

   Database migrations and seed data are applied automatically on startup.

3. Open Swagger UI: https://localhost:5110/swagger

   The port is printed in the console on startup.

## API Documentation

Full API reference with request/response schemas is available via Swagger UI
after starting the application (see [How to Run](#how-to-run)).