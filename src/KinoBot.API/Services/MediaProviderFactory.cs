using KinoBot.API.Abstractions;

namespace KinoBot.API.Services;

public class MediaProviderFactory(IEnumerable<IMediaProvider> providers)
{
  public IMediaProvider GetProvider(string mediaType)
  {
    return providers.FirstOrDefault(p => string.Equals(p.MediaType, mediaType, StringComparison.OrdinalIgnoreCase))
           ?? throw new NotSupportedException($"Media type '{mediaType}' is not supported.");
  }
}
