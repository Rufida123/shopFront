# Shopfront

Shopfront is a small e-commerce API written with .NET 9. You can create products, search them, change a price and place an order. It is built to show how the code is organised more than how many features it has.

## Layers

The solution has four projects under `src` and the dependencies only point inward.

- `Shopfront.Domain` holds the entities, the `Money` value object and the domain exceptions. It references nothing.
- `Shopfront.Application` holds the commands, queries, handlers, validators, DTOs and the repository interfaces. It references Domain only.
- `Shopfront.Infrastructure` implements those interfaces with EF Core and PostgreSQL. It references Application.
- `Shopfront.WebApi` has the controllers, the exception handler and the dependency wiring. It references Application and Infrastructure.

Business rules live in the domain. A product price must be above zero, stock cannot go below zero, an order must contain at least one product and cannot ask for more units than are in stock. Order totals are calculated by the `Order` itself, and each order item keeps the product name and unit price from the moment of purchase, so a later price change never alters an old order.

## Running it

You need the .NET 9 SDK and a PostgreSQL server. The connection string is in `src/Shopfront.WebApi/appsettings.json`.

Create the initial migration once and keep the generated folder in version control:

```
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate --project src/Shopfront.Infrastructure --startup-project src/Shopfront.WebApi --output-dir Persistence/Migrations
```

Then start the API:

```
dotnet run --project src/Shopfront.WebApi
```

In development the app applies pending migrations on startup and seeds a few products. Swagger is at `http://localhost:5080/swagger`.

## Running with Docker

```
docker compose up --build
```

This starts PostgreSQL and the API. Swagger is at `http://localhost:8081/swagger`. Use `docker compose down -v` to stop everything and delete the database volume.

## Tests

```
dotnet test
```

The unit tests cover the domain rules, the place order handler and the validators. The integration tests start the API with `WebApplicationFactory` and swap PostgreSQL for the EF Core in-memory provider.

## Decisions

Requests go through MediatR, one handler per command or query, so controllers only send a request and map the result to HTTP. Handlers that change state call `IUnitOfWork.SaveChangesAsync` once, which is what makes placing an order atomic: the order and the stock changes are saved together.

Validation runs in a MediatR pipeline behavior that executes every FluentValidation validator before the handler. Validators check shape and limits, the domain checks the business rules.

There are no try/catch blocks in controllers or handlers. A single `GlobalExceptionHandler` turns validation errors into 400, missing resources into 404, domain rule violations into 422 and anything else into a generic 500, all as problem details.
