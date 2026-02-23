using System.Text.Json.Serialization;
using KinoBot.API.Abstractions;
using KinoBot.API.Configs;
using KinoBot.API.Messaging;
using KinoBot.API.Services;
using KinoBot.API.UpdateHandlers;
using KinoBot.API.Updates;
using Microsoft.Extensions.Caching.Hybrid;
using Telegram.Bot;

namespace KinoBot.API;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddApi(IConfigurationManager config)
        {
            services.AddControllers()
                .AddJsonOptions(options =>
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            services.AddHostedService<TelegramBotSetupService>();
            services.AddScoped<IWatchlistService, WatchlistService>();

            services
                .AddTelegram(config)
                .AddTmdb(config)
                .AddCaching(config)
                .AddUpdateHandlers();

            return services;
        }

        private IServiceCollection AddCaching(IConfigurationManager config)
        {
            services.AddStackExchangeRedisCache(o =>
                o.Configuration = config.GetConnectionString("Redis"));

            services.AddHybridCache(options =>
            {
                options.DefaultEntryOptions = new HybridCacheEntryOptions
                {
                    Expiration = TimeSpan.FromHours(12),
                    LocalCacheExpiration = TimeSpan.FromHours(1)
                };
            });

            return services;
        }

        private IServiceCollection AddTelegram(IConfigurationManager config)
        {
            var botConfigSection = config.GetSection("BotConfiguration");

            services.AddSingleton<IUpdateService, UpdateService>();

            services.Configure<BotConfiguration>(botConfigSection);
            services.AddHttpClient("tgwebhook")
                .RemoveAllLoggers()
                .AddTypedClient<ITelegramBotClient>(httpClient =>
                    new TelegramBotClient(botConfigSection.Get<BotConfiguration>()!.BotToken, httpClient));

            return services;
        }

        private IServiceCollection AddTmdb(IConfigurationManager config)
        {
            var tmdbConfigSection = config.GetSection("TmdbConfiguration");

            services.Configure<TmdbConfiguration>(tmdbConfigSection);

            services.AddTransient<TmdbAuthHandler>();
            services.AddScoped<ITmdbService, TmdbService>();

            var tmdbConfig = tmdbConfigSection.Get<TmdbConfiguration>();
            services.AddHttpClient<ITmdbClient, TmdbClient>(client =>
                {
                    client.BaseAddress = new Uri(tmdbConfig!.BaseUrl);
                })
                .AddHttpMessageHandler<TmdbAuthHandler>();

            return services;
        }

        private IServiceCollection AddUpdateHandlers()
        {
            services.AddScoped<IUpdateHandler<ChosenInlineResultUpdate>, GetRandomMediaChosenInlineResultHandler>();
            services.AddScoped<IUpdateHandler<ChosenInlineResultUpdate>, GetMediaDetailsChosenInlineResultHandler>();
            services.AddScoped<IUpdateHandler<InlineQueryUpdate>, InlineQueryHandler>();
            services.AddScoped<IUpdateHandler<CallbackQueryUpdate>, AddMediaToWatchlistCallbackQueryHandler>();
            services.AddSingleton<IUpdateDispatcher, UpdateDispatcher>();

            return services;
        }
    }
}