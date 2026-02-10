# Library Management System - Microservices Architecture

A complete library management system built with .NET 9, featuring a microservices architecture with event-driven communication using RabbitMQ and MassTransit.

## Architecture Overview

The system consists of 5 microservices communicating through RabbitMQ with an API Gateway (YARP) as the entry point:

```
┌─────────────────────────────────────────────────────────────────┐
│                        API Gateway (YARP)                        │
│                         Port: 5000                               │
└────────┬────────┬────────┬────────┬────────────────────────────┘
         │        │        │        │
    ┌────▼───┐ ┌─▼────┐ ┌─▼──────┐ ┌▼────────┐
    │ Party  │ │Catalog│ │Reserv. │ │ Audit   │
    │ :5001  │ │ :5002 │ │ :5003  │ │ :5004   │
    └───┬────┘ └──┬────┘ └───┬────┘ └───┬─────┘
        │         │           │          │
    ┌───▼─────────▼───────────▼──────────▼────┐
    │           RabbitMQ Event Bus             │
    └────────────────────────────────────────┬─┘
                                             │
    ┌────────────┬───────────────────────────▼─┐
    │ PostgreSQL │ MongoDB                     │
    │ (3 DBs)    │ (AuditEventsDb)             │
    └────────────┴─────────────────────────────┘
```

## Services

### 1. Party Service (Port 5001)
**Database:** PostgreSQL (PartyDb)

Manages parties (users), authors, customers, and their roles.

**Entities:**
- Party: Id, FirstName, LastName, Email, CreatedAt, UpdatedAt
- Role: Id, Name (Author/Customer)
- PartyRole: Junction table with AssignedAt

**Endpoints:**
- `GET /api/parties` - List all parties
- `GET /api/parties/{id}` - Get party by ID
- `POST /api/parties` - Create party
- `PUT /api/parties/{id}` - Update party
- `DELETE /api/parties/{id}` - Delete party
- `POST /api/parties/{id}/roles` - Assign role to party
- `DELETE /api/parties/{id}/roles/{roleId}` - Remove role from party
- `GET /api/roles` - List all roles

**Events Published:**
- PartyCreated, PartyUpdated, PartyDeleted
- RoleAssigned, RoleRemoved

### 2. Catalog Service (Port 5002)
**Database:** PostgreSQL (CatalogDb)

Manages books and categories.

**Entities:**
- Book: Id, Title, ISBN, AuthorPartyId, CategoryId, TotalCopies, AvailableCopies, CreatedAt, UpdatedAt
- Category: Id, Name

**Endpoints:**
- `GET /api/books` - List all books
- `GET /api/books/{id}` - Get book by ID
- `GET /api/books/search?title={title}` - Search books by title
- `POST /api/books` - Create book
- `PUT /api/books/{id}` - Update book
- `DELETE /api/books/{id}` - Delete book
- `GET /api/categories` - List all categories
- `GET /api/categories/{id}` - Get category by ID
- `POST /api/categories` - Create category
- `PUT /api/categories/{id}` - Update category
- `DELETE /api/categories/{id}` - Delete category

**Events Published:**
- BookCreated, BookUpdated, BookDeleted
- CategoryCreated, CategoryUpdated, CategoryDeleted

**Events Consumed:**
- BookBorrowed - Decreases AvailableCopies
- BookReturned - Increases AvailableCopies

### 3. Reservation Service (Port 5003)
**Database:** PostgreSQL (ReservationDb)

Manages book borrowing and returns.

**Entities:**
- Reservation: Id, BookId, CustomerPartyId, BorrowedAt, ReturnedAt, IsActive

**Endpoints:**
- `GET /api/reservations` - List all reservations
- `GET /api/reservations/active` - List active reservations
- `GET /api/reservations/{id}` - Get reservation by ID
- `GET /api/reservations/customer/{customerPartyId}` - Get customer reservations
- `POST /api/reservations/borrow` - Borrow a book
- `POST /api/reservations/return/{reservationId}` - Return a book

**Business Rules:**
- One customer can borrow one copy of a book at a time
- Prevents duplicate active reservations

**Events Published:**
- BookBorrowed, BookReturned

### 4. Audit Service (Port 5004)
**Database:** MongoDB (AuditEventsDb)

Stores and manages audit events from all services.

**Entities:**
- AuditEvent: EventId, EntityId, EntityType, ActionType, Timestamp, Payload

**Endpoints:**
- `GET /api/events?page=1&pageSize=50` - Get paginated events
- `GET /api/events/party/{partyId}` - Get events for a party
- `GET /api/events/book/{bookId}` - Get events for a book

**Events Consumed:**
- ALL events from Party, Catalog, and Reservation services

**Background Services:**
- Data Retention: Automatically deletes events older than 1 year (runs daily)

### 5. API Gateway (Port 5000)
**Technology:** YARP Reverse Proxy

Routes requests to downstream services based on path patterns.

**Route Mappings:**
- `/api/parties/*` → Party Service
- `/api/roles/*` → Party Service
- `/api/books/*` → Catalog Service
- `/api/categories/*` → Catalog Service
- `/api/reservations/*` → Reservation Service
- `/api/events/*` → Audit Service

## Technology Stack

- **.NET 9** 
- **PostgreSQL 16** - Transactional data storage
- **MongoDB 7** - Event storage
- **RabbitMQ 3** - Message broker
- **MassTransit 8** - Distributed application framework
- **Entity Framework Core 9** - ORM with code-first migrations
- **YARP** - Reverse proxy for API Gateway
- **Docker** - Containerization
- **xUnit + Moq** - Unit testing

## Clean Architecture

Each service follows Clean Architecture principles:

```
Service/
├── Domain/          # Entities, Value Objects, Interfaces
├── Application/     # Services, DTOs, Business Logic
├── Infrastructure/  # EF Core, Repositories, MassTransit
└── API/             # Controllers, Program.cs, Dockerfile
```

## Getting Started

### Prerequisites

- Docker and Docker Compose
- .NET 9 SDK (for local development)

### Running with Docker Compose

1. Clone the repository:
```bash
git clone <repository-url>
cd LibraryManagementSystem
```

2. Build and start all services:
```bash
docker-compose up --build
```

3. Wait for all services to start (check health status):
```bash
docker-compose ps
```

4. Access the API Gateway:
```bash
curl http://localhost:5000/api/parties
curl http://localhost:5000/api/books
curl http://localhost:5000/api/reservations
curl http://localhost:5000/api/events?page=1&pageSize=10
```

5. Access RabbitMQ Management UI:
```
URL: http://localhost:15672
Username: guest
Password: guest
```

### Service Ports

- API Gateway: http://localhost:5000
- Party Service: http://localhost:5001 (via Gateway)
- Catalog Service: http://localhost:5002 (via Gateway)
- Reservation Service: http://localhost:5003 (via Gateway)
- Audit Service: http://localhost:5004 (via Gateway)
- PostgreSQL: localhost:5432
- MongoDB: localhost:27017
- RabbitMQ: localhost:5672
- RabbitMQ Management: http://localhost:15672

### Stopping Services

```bash
docker-compose down
```

To also remove volumes:
```bash
docker-compose down -v
```

## Running Tests

```bash
# Run all tests
dotnet test

# Run specific service tests
dotnet test tests/Party.UnitTests/
dotnet test tests/Catalog.UnitTests/
dotnet test tests/Reservation.UnitTests/
dotnet test tests/Audit.UnitTests/
```

## Seed Data

The system comes pre-seeded with sample data:

**Party Service:**
- 2 Roles: Author, Customer
- 3 Parties:
  - John Author (Author role)
  - Jane Customer (Customer role)
  - Bob Both (Both roles)

**Catalog Service:**
- 2 Categories: Fiction, Mystery
- 3 Books with various inventory levels

**Reservation Service:**
- 1 Active reservation (5 days old)

## Event-Driven Communication

### MassTransit Configuration

All services use MassTransit with RabbitMQ for event-driven communication:

- **Retry Strategy:** 3 retries with incremental/exponential backoff
- **Error Handling:** Failed messages routed to error queue
- **Event Publishing:** Automatic on all mutations (Create, Update, Delete)
- **Event Consumption:** Audit service consumes ALL events

### Event Flow Example

1. User borrows a book via Reservation Service
2. Reservation Service publishes `BookBorrowed` event
3. Catalog Service consumes event and decreases `AvailableCopies`
4. Audit Service consumes event and stores audit trail
5. All operations logged with timestamps and payload

## Design Decisions & Trade-offs

### 1. Microservices Architecture
**Decision:** Separate services for different bounded contexts
- **Pros:** Independent deployment, scalability, technology flexibility
- **Cons:** Distributed system complexity, eventual consistency

### 2. Event-Driven Communication
**Decision:** RabbitMQ + MassTransit for inter-service communication
- **Pros:** Loose coupling, async processing, resilience
- **Cons:** Eventual consistency, debugging complexity

### 3. Database per Service
**Decision:** Each service has its own database
- **Pros:** Service independence, optimized per service needs
- **Cons:** Cross-service queries difficult, data duplication

### 4. PostgreSQL for Transactional Data
**Decision:** Use PostgreSQL for Party, Catalog, Reservation
- **Pros:** ACID compliance, mature ecosystem, relational integrity
- **Cons:** Scaling writes can be challenging

### 5. MongoDB for Audit Events
**Decision:** Use MongoDB for event storage
- **Pros:** Flexible schema, high write throughput, document-oriented
- **Cons:** No ACID transactions across documents

### 6. API Gateway Pattern
**Decision:** YARP reverse proxy as single entry point
- **Pros:** Single endpoint, routing flexibility, security layer
- **Cons:** Single point of failure (mitigated with HA setup)

### 7. Clean Architecture
**Decision:** Separate Domain, Application, Infrastructure layers
- **Pros:** Testability, maintainability, clear dependencies
- **Cons:** More files/projects, learning curve

### 8. EF Core Migrations Auto-Apply
**Decision:** Migrations applied automatically on startup
- **Pros:** Simplified deployment, always up-to-date schema
- **Cons:** Risk in production (should use proper migration strategy)

### 9. Seed Data in Migrations
**Decision:** Sample data seeded via EF Core
- **Pros:** Consistent test environment, demo-ready
- **Cons:** Not suitable for production

### 10. Background Service for Data Retention
**Decision:** Daily cleanup of events older than 1 year
- **Pros:** Automated maintenance, controlled storage growth
- **Cons:** No soft delete, permanent data loss

## Production Considerations

For production deployment, consider:

1. **Security:**
   - Add authentication/authorization (JWT, OAuth2)
   - Use HTTPS everywhere
   - Secure database connections
   - API Gateway rate limiting

2. **Observability:**
   - Centralized logging (ELK, Seq)
   - Distributed tracing (OpenTelemetry)
   - Metrics and monitoring (Prometheus, Grafana)

3. **Resilience:**
   - Circuit breakers
   - Health checks
   - Graceful shutdown
   - Database connection pooling

4. **Scalability:**
   - Horizontal scaling with Kubernetes
   - Database read replicas
   - Caching layer (Redis)
   - CDN for static content

5. **Data Management:**
   - Proper migration strategy
   - Database backups
   - Disaster recovery plan
   - GDPR compliance

## Development

### Local Development

```bash
# Restore packages
dotnet restore

# Build solution
dotnet build

# Run Party Service locally
cd src/Services/Party/Party.API
dotnet run

# Run tests
dotnet test
```

### Adding a Migration

```bash
# Party Service
cd src/Services/Party/Party.Infrastructure
dotnet ef migrations add MigrationName --startup-project ../Party.API

# Catalog Service
cd src/Services/Catalog/Catalog.Infrastructure
dotnet ef migrations add MigrationName --startup-project ../Catalog.API

# Reservation Service
cd src/Services/Reservation/Reservation.Infrastructure
dotnet ef migrations add MigrationName --startup-project ../Reservation.API
```

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License

This project is licensed under the MIT License.

## Contact

For questions or support, please open an issue on GitHub.
