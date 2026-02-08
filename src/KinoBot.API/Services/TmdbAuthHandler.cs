using KinoBot.API.Configs;
using Microsoft.Extensions.Options;

namespace KinoBot.API.Services;

public class TmdbAuthHandler(IOptions<TmdbConfiguration> config) : DelegatingHandler
{
    private readonly TmdbConfiguration _config = config.Value;

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        request.Headers.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _config.BearerToken);

        return await base.SendAsync(request, cancellationToken);
    }
}
