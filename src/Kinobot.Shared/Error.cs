namespace Kinobot.Shared;

public sealed record Error(string Code, string Description)
{
    public override string ToString() => $"{Code}: {Description}";
}

public static class DatabaseErrors
{
    public const string SaveChangesErrorCode = "Database.SaveChanges";
    
    public static Error SaveChangesError = new(SaveChangesErrorCode,
        "Couldn't save changes to database");
}