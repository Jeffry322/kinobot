# Agent Guidelines

This document outlines the coding standards and practices for AI agents working on the KinoBot project.

## Project Structure
- `src/KinoBot.API`: Main API project
- `src/KinoBot.API/Configs`: Configuration classes
- `src/KinoBot.API/Services`: Business logic and external API clients
- `src/KinoBot.API/Models`: Data transfer objects and internal models

## Coding Standards
1. **Namespaces**: Ensure classes are placed in appropriate namespaces reflecting their folder structure.
2. **Dependency Injection**: Use extension methods in `DependencyInjection.cs` to manage service registrations. Prefer injecting dependencies via primary constructors.
3. **Configuration**: Bind configuration sections from `appsettings.json` to strongly-typed classes in the `Configs` namespace.
4. **Error Handling**: Use `try-catch` blocks in external service clients and prefer returning descriptive error responses or `null` for not-found scenarios.
5. **C# Features**: Use modern C# features like pattern matching and records where appropriate.
6. **Emojis**: Never use emojis in the codebase (including strings, comments, and logs).
7. **Comments**: Minimize the use of comments. Use them only for unintuitive logic or special cases that require explanation. Code should be self-documenting.
8. **Naming**: Use PascalCase for class names, methods, and properties. Use camelCase for local variables and parameters. Use camelCase case for parameters injected via primary constructors.

## TMDB API Integration
- Use `ITmdbClient` for searching and fetching movie/TV details.
- Authentication is handled via `TmdbAuthHandler` using a Bearer token.
- Relative paths in the client should not start with a leading slash if a base path (like `/3/`) is defined in `BaseAddress`.

## Telegram Bot Integration
- Updates are handled by `UpdateHandler`.
- Use `InlineQueryResult` for inline queries.
- Avoid complex logic directly in `HandleUpdateAsync`; delegate to private methods or services.
