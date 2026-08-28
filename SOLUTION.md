# Solution Overview

## Business Task

The company rents out conference rooms to businesses. Clients need a way to
search for available rooms, book them, and get the rental cost calculated
automatically based on time of day and selected add-on services.

## Core API Capabilities

1. **Add a conference room** — name, capacity, base hourly rate, available services.
2. **Edit a room** — update capacity/rate, add or remove services.
3. **Delete a room** — soft delete; blocked if the room has active future bookings.
4. **Search available rooms** — by date/time range and required capacity.
5. **Book a room** — by date/time and selected services, with automatic cost calculation.

Beyond the core requirements, the solution also supports:

- Marking a room as **under maintenance** (temporarily unavailable, without deleting it)
- **Cancelling** a booking
- **Reports**: revenue and room occupancy over a given period

## Pricing Rules

The room rental cost depends on the time of day. Tariff periods:

| Period            | Time          | Adjustment      |
|--------------------|---------------|------------------|
| Morning            | 06:00–09:00   | -10% discount    |
| Standard           | 09:00–18:00   | base rate        |
| Peak               | 12:00–14:00   | +15% surcharge   |
| Evening            | 18:00–23:00   | -20% discount    |

The **Peak** period is a subset of **Standard** and takes priority when they overlap.
Bookings outside 06:00–23:00 are not allowed.

### Calculation Method

A booking is split into 30-minute blocks. Each block is priced according to
the tariff period its **start time** falls into, using half of the room's
base hourly rate. The total room cost is the sum of all blocks. Selected
services are added on top at a fixed price, regardless of booking duration.

**Example:** a booking from 11:30 to 14:30 spans Standard → Peak → Standard,
and is priced per half-hour block accordingly, rather than a single flat rate
for the whole duration.

## Business Rules & Constraints

Beyond the explicit requirements, the following rules were introduced to
resolve ambiguities in the original task and keep the system consistent:

- **Minimum booking block: 30 minutes.** Bookings are rounded to the nearest
  half-hour (start time rounds down, end time rounds up) — e.g. a 10:00–10:10
  request becomes 10:00–10:30. This keeps the pricing calculation
  deterministic and avoids sub-block edge cases.

- **Room states instead of a single "deleted" flag.** A room can be `Active`,
  `UnderMaintenance`, or `Deleted`. This distinguishes a temporary
  unavailability (e.g. renovation) — where existing bookings remain valid —
  from a permanent removal.

- **Room deletion is blocked if active future bookings exist.** Since
  temporary unavailability is handled via `UnderMaintenance`, actual deletion
  is reserved for rooms with no legitimate bookings to protect, and can be
  safely restricted.

- **Services always belong to a specific room** (not shared across rooms),
  and cannot be added with a price of zero — unlike the room's base rate,
  which may be discounted to zero as a promotion.

- **Selected services are price-snapshotted at booking time.** Changing a
  service's price later does not affect already-confirmed bookings.

- **Cancellation is blocked less than 8 hours before the booking start time.**
  A business rule preventing last-minute cancellations that would leave a
  room unbookable on short notice.

- **Reports only include `Confirmed` bookings.** Cancelled bookings do not
  affect revenue or occupancy figures.

- **Only UAH is currently supported** as a currency; the `Money` value object
  is currency-aware to allow multi-currency support in the future without
  redesigning the domain model.

## Technical Decisions

### Architecture: Clean Architecture + CQRS

The solution is split into four layers (`Domain → Application →
Infrastructure → API`), with dependencies pointing inward toward `Domain`.
Business logic lives in rich domain entities rather than anemic models —
e.g. `Room`, `Booking`, and `Service` enforce their own invariants through
methods, not external services.

Use cases are implemented as CQRS commands/queries via **MediatR**, each in
its own folder with a matching **FluentValidation** validator, wired through
a shared `ValidationBehavior` pipeline. This keeps each use case isolated
and makes adding new ones (e.g. a future discount system) a matter of adding
a new command/handler pair, without touching existing code.

### Rule-Based Pricing Engine

Tariff periods are defined as data (`PricingRule` records) behind an
`IPricingRuleProvider` interface, rather than hardcoded `if/else` branches.
Adding a new tariff period (e.g. a weekend rate) means adding a rule, not
modifying the calculation algorithm — an application of the Open/Closed
Principle.

### Avoiding N+1 Queries in Room Search

Searching for available rooms could naively check each candidate room's
bookings individually. Instead, `GetBusyRoomIdsAsync` retrieves all busy
room IDs for the requested time range in a single query, which the handler
then uses to filter candidates in memory — a constant number of database
round trips regardless of how many rooms exist.

### Reports: Mixed Aggregation Strategy

The revenue report aggregates in SQL (`SUM`/`COUNT`), since it involves only
simple numeric fields. The occupancy report — which requires summing
`(EndTime - StartTime)` durations — aggregates in memory instead, after
fetching a lightweight projection of just the fields needed. This avoids
relying on EF Core's translation of `TimeSpan` arithmetic to SQL, which is
not reliably supported across providers.

### Error Handling

Domain and application exceptions are distinguished by intent:
`DomainException` for business rule violations (mapped to `400 Bad Request`),
`NotFoundException` for missing entities (`404 Not Found`), and
FluentValidation failures for malformed input (`400 Bad Request` with
field-level details). All are caught by a single exception-handling
middleware, keeping controllers free of try/catch blocks.

### PostgreSQL `timestamptz` Compatibility

Npgsql 6+ requires `DateTime` values written to `timestamp with time zone`
columns to have `DateTimeKind.Utc`. Rather than scattering
`DateTime.SpecifyKind` calls across the codebase, a single `ValueConverter`
is registered for all `DateTime` properties in `AppDbContext`, applied
uniformly regardless of where the value originated.