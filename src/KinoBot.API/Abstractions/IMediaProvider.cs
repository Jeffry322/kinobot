namespace KinoBot.API.Abstractions;

public interface IMediaProvider
{
    string MediaType { get; }
    string FormatCaption(Models.SearchResult result);
    string FormatCaption(Models.Movie movie);
    string FormatCaption(Models.TvShow tvShow);
}
