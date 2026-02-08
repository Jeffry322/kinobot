using System.Text.Json.Serialization;
using KinoBot.API.Abstractions;
using KinoBot.API.CallbackData;
using KinoBot.API.CallbackQueriesHandlers;
using KinoBot.API.Configs;
using KinoBot.API.Services;
using Telegram.Bot;

namespace KinoBot.API;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddApi(IConfigurationManager config)
        {
            services.AddScoped<IUpdateHandler, UpdateHandler>();
            services.AddScoped<ITmdbService, TmdbService>();
            services.AddControllers().AddJsonOptions(options =>
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
            services.AddTelegram(config);
            services.AddTmdb(config);
            services.AddMediaProviders();
            services.AddCallbackQueryHandlers();

            return services;
        }

        private IServiceCollection AddMediaProviders()
        {
            services.AddScoped<IMediaProvider, MovieProvider>();
            services.AddScoped<IMediaProvider, TvShowProvider>();
            services.AddScoped<MediaProviderFactory>();
            return services;
        }

        private IServiceCollection AddTelegram(IConfigurationManager config)
        {
            var botConfigSection = config.GetSection("BotConfiguration");

            services.Configure<BotConfiguration>(botConfigSection);
            services.AddHttpClient("tgwebhook").RemoveAllLoggers().AddTypedClient<ITelegramBotClient>(httpClient =>
                new TelegramBotClient(botConfigSection.Get<BotConfiguration>()!.BotToken, httpClient));

            return services;
        }

        private IServiceCollection AddTmdb(IConfigurationManager config)
        {
            var tmdbConfigSection = config.GetSection("TmdbConfiguration");

            services.Configure<TmdbConfiguration>(tmdbConfigSection);

            services.AddTransient<TmdbAuthHandler>();

            var tmdbConfig = tmdbConfigSection.Get<TmdbConfiguration>();
            services.AddHttpClient<ITmdbClient, TmdbClient>(client =>
                {
                    client.BaseAddress = new Uri(tmdbConfig!.BaseUrl);
                })
                .AddHttpMessageHandler<TmdbAuthHandler>();

            return services;
        }

        private IServiceCollection AddCallbackQueryHandlers()
        {
            services.AddScoped<ICallbackQueryHandler<GetMediaDetailsCallbackData>, GetMediaDetailsCallbackQuery>();
            services.AddScoped<ICallbackQueryHandler<GetBackCallbackData>, GetBackCallbackQueryHandler>();
            return services;
        }
    }
}