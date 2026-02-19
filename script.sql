IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260218121846_Initial'
)
BEGIN
    CREATE TABLE [WatchlistMedias] (
        [Id] uniqueidentifier NOT NULL,
        [TelegramUserId] bigint NOT NULL,
        [MediaType] nvarchar(max) NOT NULL,
        [MediaId] int NOT NULL,
        [CreatedAtUtc] datetime2 NOT NULL,
        CONSTRAINT [PK_WatchlistMedias] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260218121846_Initial'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260218121846_Initial', N'10.0.3');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260218123627_AddUniqueIndexOnUserIdMediaIdMediaType'
)
BEGIN
    DECLARE @var nvarchar(max);
    SELECT @var = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[WatchlistMedias]') AND [c].[name] = N'MediaType');
    IF @var IS NOT NULL EXEC(N'ALTER TABLE [WatchlistMedias] DROP CONSTRAINT ' + @var + ';');
    ALTER TABLE [WatchlistMedias] ALTER COLUMN [MediaType] nvarchar(450) NOT NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260218123627_AddUniqueIndexOnUserIdMediaIdMediaType'
)
BEGIN
    CREATE UNIQUE INDEX [IX_WatchlistMedias_TelegramUserId_MediaType_MediaId] ON [WatchlistMedias] ([TelegramUserId], [MediaType], [MediaId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260218123627_AddUniqueIndexOnUserIdMediaIdMediaType'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260218123627_AddUniqueIndexOnUserIdMediaIdMediaType', N'10.0.3');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260218152311_AddNewColumn'
)
BEGIN
    ALTER TABLE [WatchlistMedias] ADD [IsWatched] bit NOT NULL DEFAULT CAST(0 AS bit);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260218152311_AddNewColumn'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260218152311_AddNewColumn', N'10.0.3');
END;

COMMIT;
GO

