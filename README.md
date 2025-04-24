# TodoApi

A RESTful API for managing todo tasks, built with .NET 9.

## 🚀 Features

- Full CRUD operations for todo tasks
- View todos scheduled for today, tomorrow, or the current week
- Update completion percentage and mark tasks as done
- Data persistence using SQL Server
- Unit and integration tests with xUnit
- Docker support for easy deployment

## 🧰 Technologies

- .NET 9
- Entity Framework Core
- SQL Server
- xUnit
- FluentValidation
- Docker

## 📁 Project Structure

- **TodoApi**: Main API project
  - `Controllers` – API endpoints
  - `Models` – Domain models and DTOs
  - `Services` – Business logic
  - `Repositories` – Data access layer
  - `Data` – EF Core DbContext and configurations
  - `Validators` – Input validation
- **TodoApi.Tests**: Unit tests
  - `Unit` – Tests for services and business logic


## 🛠 Requirements

- .NET SDK 9.0 or newer
- SQL Server instance (local or containerized)
- Docker (optional, for containerized setup)

## 🏁 Getting Started

### 🔧 Run Locally

1. Clone the repository
2. Set your SQL Server connection string in `appsettings.json`
3. Navigate to the `TodoApi` directory
4. Start the application:

```bash
dotnet run
```

Visit the API at: `https://localhost:5025` or `http://localhost:7030`

### 🐳 Run with Docker

1. Build and run using Docker Compose:

```bash
docker-compose up -d
```

The API will be available at: `http://localhost:5025`

## 🌐 API Endpoints

- `GET /api/todos` – Get all todos
- `GET /api/todos/{id}` – Get a specific todo
- `GET /api/todos/today` – Get today's todos
- `GET /api/todos/tomorrow` – Get tomorrow's todos
- `GET /api/todos/week` – Get this week's todos
- `POST /api/todos` – Create a new todo
- `PUT /api/todos/{id}` – Update a todo
- `PATCH /api/todos/{id}/percent` – Update completion percentage
- `PATCH /api/todos/{id}/done` – Mark as done
- `DELETE /api/todos/{id}` – Delete a todo

## 🧪 Running Tests

```bash
dotnet test TodoApi.Tests
```

---

#WERSJA POLSKA

# TodoApi

RESTful API do zarządzania zadaniami typu todo, zbudowane z użyciem .NET 9.

## 🚀 Funkcje

- Pełne operacje CRUD dla zadań todo
- Przeglądanie zadań zaplanowanych na dziś, jutro lub bieżący tydzień
- Aktualizacja procentu ukończenia i oznaczanie zadań jako wykonane
- Przechowywanie danych przy użyciu SQL Server
- Testy jednostkowe i integracyjne z xUnit
- Wsparcie dla Docker ułatwiające wdrożenie

## 🧰 Technologie

- .NET 9
- Entity Framework Core
- SQL Server
- xUnit
- FluentValidation
- Docker

## 📁 Struktura projektu

- **TodoApi**: Główny projekt API
  - `Controllers` – Endpointy API
  - `Models` – Modele domeny i DTO
  - `Services` – Logika biznesowa
  - `Repositories` – Warstwa dostępu do danych
  - `Data` – EF Core DbContext i konfiguracje
  - `Validators` – Walidacja danych wejściowych
- **TodoApi.Tests**: Testy jednostkowe
  - `Unit` – Testy dla serwisów i logiki biznesowej


## 🛠 Wymagania

- .NET SDK 9.0 lub nowszy
- Instancja SQL Server (lokalna lub w kontenerze)
- Docker (opcjonalnie, dla konfiguracji w kontenerach)

## 🏁 Jak zacząć

### 🔧 Uruchomienie lokalnie

1. Sklonuj repozytorium
2. Ustaw swój connection string do SQL Server w `appsettings.json`
3. Przejdź do katalogu `TodoApi`
4. Uruchom aplikację:

```bash
dotnet run
```

API będzie dostępne pod adresem: `https://localhost:5025` lub `http://localhost:7030`

### 🐳 Uruchomienie z Dockerem

1. Zbuduj i uruchom przy użyciu Docker Compose:

```bash
docker-compose up -d
```

API będzie dostępne pod adresem: `http://localhost:5025`

## 🌐 Endpointy API

- `GET /api/todos` – Pobierz wszystkie zadania
- `GET /api/todos/{id}` – Pobierz konkretne zadanie
- `GET /api/todos/today` – Pobierz dzisiejsze zadania
- `GET /api/todos/tomorrow` – Pobierz jutrzejsze zadania
- `GET /api/todos/week` – Pobierz zadania z tego tygodnia
- `POST /api/todos` – Utwórz nowe zadanie
- `PUT /api/todos/{id}` – Zaktualizuj zadanie
- `PATCH /api/todos/{id}/percent` – Zaktualizuj procent ukończenia
- `PATCH /api/todos/{id}/done` – Oznacz jako wykonane
- `DELETE /api/todos/{id}` – Usuń zadanie

## 🧪 Uruchamianie testów

```bash
dotnet test TodoApi.Tests
```
