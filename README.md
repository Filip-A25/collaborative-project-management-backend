# Collaborative Project Management API

REST API kreiranje, upravljanje i suradnju na projektima. API je razvijen pomoću .NET 9.0, Entity Framework Core-a i SQL Server baze podataka.

## Preduvjeti za pokretanje

- [Docker](https://docs.docker.com/get-started/get-docker/)
- [Docker Compose](https://docs.docker.com/compose/install)

## Konfiguracija

| Varijable okruženja | Opis                                    | Primjer                   |
| ------------------- | --------------------------------------- | ------------------------- |
| `MSSQL_PASSWORD`    | Lozinka baze podataka za korisnika `sa` | `CPMApiPassword000!`      |
| `JWT_SECRET`        | Niz znakova za potpisivanje tokena      | `strong-generated-string` |

## Brzo pokretanje u Dockeru

Koraci za pokretanje API-ja:

```bash
# Klonirajte repozitorij
git clone https://github.com/Filip-A25/collaborative-project-management-backend.git
```

Stvorite `.env` datoteku s varijablama okruženja na temelju `.env.example`.

```bash
# Pokrenite API i Docker kontejnere
docker compose up -d --build
```

Migracije baze podataka primjenjuju se automatski prilikom prvog pokretanja API-ja.
