# KinoBot

KinoBot is a Telegram bot designed to help users find information about movies and TV shows using The Movie Database (TMDb) API. It provides a convenient way to search for media and view detailed information directly within Telegram.

## Features

- **Multi-Search**: Search for movies and TV shows simultaneously via inline queries.
- **Media Details**: View comprehensive details including ratings, genres, production countries, and budgets.
- **Caching**: Uses `HybridCache` (Redis + L1 cache) for optimized performance and reduced API calls.
- **Webhook Integration**: Built as an ASP.NET Core API to receive updates via Telegram webhooks.

## Tech Stack

- **.NET 10**
- **ASP.NET Core Minimal APIs / Controllers**
- **Telegram.Bot Library**
- **TMDb API**
- **Redis** (via `HybridCache`)
- **Docker Compose** (for Redis and environment setup)

## To-Do List

- [x] Get movies (search and basic info)
- [x] Get movie details
- [ ] Watch list (save for later)
- [ ] Watched list (history)
- [ ] AI Search (powered by LLM)

## Setup

1. Clone the repository.
2. Configure `appsettings.Local.json` with your `BotToken`, `BearerToken` (TMDb), and other necessary configurations (see `appsettings.json` for structure).
3. Run the project using your preferred IDE or `dotnet run`.
4. Ensure Redis is running (you can use `docker-compose up -d`).
