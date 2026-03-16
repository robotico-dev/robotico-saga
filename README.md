# Robotico.Saga

[![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![.NET 10](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/download/dotnet/10.0)
[![GitHub Packages](https://img.shields.io/badge/GitHub%20Packages-Robotico.Saga-blue?logo=github)](https://github.com/robotico-dev/robotico-saga-csharp/packages)

Saga pattern for compensating transactions and cross-service consistency. Orchestration or choreography; Result-based. Depends on Robotico.Result.

## Robotico dependencies

```mermaid
flowchart LR
  A[Robotico.Saga] --> B[Robotico.Result]
```

## Installation

```bash
dotnet add package Robotico.Saga
```

## License

See repository license file.
