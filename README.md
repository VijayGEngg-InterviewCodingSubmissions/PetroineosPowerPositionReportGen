# Petroineos Power Position Report Generator

## Overview

Petroineos Power Position Report Generator is a .NET 10 backend service designed to generate intraday power position reports from trade data supplied by an external power trading system.

The service retrieves power trades for a given trading date, aggregates hourly positions, and exports a CSV report suitable for operational and business consumption.

This solution was developed as part of a backend engineering coding challenge and demonstrates:

- Clean separation of concerns
- Modern C# and .NET 10 development practices
- Test-driven development
- Dependency Injection
- Structured logging
- Resilience and fault handling
- Maintainable and extensible architecture

---

## Functional Requirements

The application:

1. Retrieves trades from an external trading service.
2. Aggregates hourly traded volumes.
3. Calculates net positions.
4. Generates a CSV report.
5. Produces timestamped report files.
6. Supports configurable execution intervals.
7. Logs application activity and failures.
8. Handles transient failures with retry policies.

---

## Architecture

The solution follows a layered architecture with clear separation between business logic and infrastructure concerns.

```text
┌──────────────────────┐
│      Service         │
│ (Hosted Worker)      │
└──────────┬───────────┘
           │
           ▼
┌──────────────────────┐
│     Application      │
│ Use Cases / Services │
└──────────┬───────────┘
           │
           ▼
┌──────────────────────┐
│       Domain         │
│ Business Rules       │
└──────────┬───────────┘
           │
           ▼
┌──────────────────────┐
│   Infrastructure     │
│ External Adapters    │
└──────────────────────┘
```

---

## Technologies

- .NET 10
- C# 14
- Microsoft Extensions Hosting
- Dependency Injection
- Serilog
- xUnit
- FluentAssertions
- CsvHelper

---

## Project Structure

```text
src/
├── Domain
├── Application
├── Infrastructure
└── Worker

tests/
└── UnitTests
```

---

## Build

### Prerequisites

- .NET 10 SDK

### Restore

```bash
dotnet restore
```

### Build

```bash
dotnet build
```

### Run Tests

```bash
dotnet test
```

### Run Application

```bash
dotnet run
```

---

## Configuration

Application settings are provided through:

```json
{
  "PowerPosition": {
    "OutputDirectory": "Reports",
    "ExecutionIntervalMinutes": 15
  }
}
```

Environment specific configuration can be supplied through:

- appsettings.json
- appsettings.Development.json
- Environment Variables

---

## Logging

The service uses structured logging to provide:

- Startup diagnostics
- Job execution information
- Trade processing summaries
- Error reporting

---

## Testing

The solution includes automated tests covering:

- Trade aggregation
- Report generation
- Application services
- Failure scenarios

Run all tests:

```bash
dotnet test
```

---

## Future Improvements

Potential enhancements include:

- Containerisation with Docker
- OpenTelemetry tracing
- Metrics and monitoring
- Parallel trade processing
- Cloud-native deployment
- Scheduled execution through Azure Functions or Kubernetes CronJobs

---

## Author

Vijayendra Ghatge
