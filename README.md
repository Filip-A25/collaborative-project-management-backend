# Collaborative Project Management API

A REST API for creating, managing and collaborating on projects. API is built with .NET 9.0, Entity Framework Core and SQL Server database.

## Requirements

- [Docker](https://docs.docker.com/get-started/get-docker/)
- [Docker Compose](https://docs.docker.com/compose/install)

## Configuration

| Env Variable     | Description                         | Example                   |
| ---------------- | ----------------------------------- | ------------------------- |
| `MSSQL_PASSWORD` | Database password for the `sa` user | `CPMApiPassword000!`      |
| `JWT_SECRET`     | String used for signing tokens      | `strong-generated-string` |

## Quickstart in Docker

Steps to start the API:

```bash
# Clone the repo
git clone https://github.com/Filip-A25/collaborative-project-management-backend.git
```

Create `.env` file with environment variables based on `.env.example` with a database password (`MSSQL_PASSWORD`) and your JWT secret key (`JWT_SECRET`).

```bash
# Start the API and database containers
docker compose up -d --build
```

Database migrations are applied automatically by the API.
