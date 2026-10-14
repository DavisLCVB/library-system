# Library System API

REST API for a university library's loan management: book catalog, physical copies, members (students and professors), and loans with limits, due dates and fines. Built with ASP.NET Core 10, Entity Framework Core 10 and PostgreSQL.

> **Status:** work in progress — Milestone 2 of 5 (catalog). See the [roadmap](#roadmap).

## Quick start

Prerequisites: [.NET 10 SDK](https://dotnet.microsoft.com/download) and Docker with Compose.

```bash
# 1. Start PostgreSQL (set POSTGRES_PASSWORD in .env first)
cp .env.example .env
docker compose up -d

# 2. Configure the connection string (stored outside the repo with User Secrets)
dotnet user-secrets set "ConnectionStrings:LibraryDb" \
  "Host=localhost;Port=5432;Database=library;Username=library;Password=<your-password>" \
  --project src/LibrarySystem.Api

# 3. Apply migrations (sample data is seeded in Development)
dotnet tool restore
dotnet ef database update \
  --project src/LibrarySystem.Infrastructure \
  --startup-project src/LibrarySystem.Api

# 4. Run the API
dotnet run --project src/LibrarySystem.Api
```

Verify it works:

```bash
curl http://localhost:5111/api/categories
curl -i http://localhost:5111/api/categories/999   # 404 as ProblemDetails
```

The OpenAPI document is served at `http://localhost:5111/openapi/v1.json` in Development.

## Architecture

Layered solution with dependencies pointing inward:

```
Api  ──►  Infrastructure  ──►  Application  ──►  Domain
```

| Project | Responsibility |
|---|---|
| `LibrarySystem.Domain` | Entities and enums. No dependencies. |
| `LibrarySystem.Application` | Use-case services, DTOs (`record`), repository interfaces (ports) and application exceptions. |
| `LibrarySystem.Infrastructure` | EF Core `DbContext`, `IEntityTypeConfiguration<T>` mappings, migrations, seeding and repository implementations (adapters). |
| `LibrarySystem.Api` | Controllers, global exception handling and the DI composition root. |

Key decisions:

| Topic | Decision |
|---|---|
| Data access | Repository interfaces in Application, EF Core implementations in Infrastructure, so business rules can be unit-tested without a database. Reads use `AsNoTracking`. |
| Errors | Application exceptions (`NotFoundException`, ...) mapped to [RFC 9457](https://www.rfc-editor.org/rfc/rfc9457) ProblemDetails by a global `IExceptionHandler`. |
| Many-to-many | `Book` ↔ `Author` configured explicitly with Fluent API (`BookAuthors` join table). |
| Secrets | No credentials in the repo: `.env` for Docker Compose, User Secrets in Development, environment variables elsewhere. |
| Time | `TimeProvider` is injected (planned for loans) instead of `DateTime.Now`, so due dates and fines are testable. |

## Domain

| Entity | Notes |
|---|---|
| `Category` | Unique name. |
| `Author` | Many-to-many with `Book`. |
| `Book` | Unique ISBN; belongs to a category. A book is the title… |
| `BookCopy` | …and a copy is the physical item that gets loaned. Unique inventory code; status `Available`, `Loaned`, `Maintenance` or `Lost`. |
| `Member` | Student or professor *(planned)*. |
| `Loan` | Loan date, due date, return date, renewals and fines *(planned)*. |

Business rules to implement in `LoanService` (inside transactions):

- **R1** Students: max 3 active loans, 14 days. Professors: max 5, 30 days.
- **R2** No loans to inactive members, or members with overdue loans or unpaid fines.
- **R3** Only `Available` copies can be loaned; the copy becomes `Loaned`.
- **R4** Returning a loan frees the copy and charges S/ 1.00 per day late.
- **R5** One renewal only, +7 days, and only if the loan is not overdue.
- **R6** A book with loaned copies cannot be deleted.
- **R7** Concurrent loans of the same copy: only one succeeds, the other gets `409 Conflict` (optimistic concurrency with PostgreSQL `xmin`).

## Roadmap

- [x] **1. Setup** — layered solution, PostgreSQL in Docker, `DbContext`, first migration, `GET /api/categories`.
- [ ] **2. Catalog** — authors (N:M) ✅, book copies ✅, seed data ✅, `GET /api/categories/{id}` ✅; categories and authors CRUD, paginated and filtered books, book copies endpoint.
- [ ] **3. Members and loans** — `Member`, `Loan`, `LoanService` with R1–R6 and transactions.
- [ ] **4. Robustness** — full ProblemDetails coverage, FluentValidation, concurrency (R7), logging.
- [ ] **5. Quality and deployment** — xUnit unit tests, integration tests with `WebApplicationFactory` + Testcontainers, full `docker-compose`.

## Tech stack

C# 14 · .NET 10 (LTS) · ASP.NET Core (Controllers) · EF Core 10 · Npgsql · PostgreSQL 18 · Docker Compose
