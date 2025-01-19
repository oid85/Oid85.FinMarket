using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.External.Telegram;
using Oid85.FinMarket.External.Tinkoff;

namespace Oid85.FinMarket.External.Extensions;

public static class ServiceCollectionExtensions
{
    public static void ConfigureExternalServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTransient<ITinkoffService, TinkoffService>();
        services.AddTransient<ITelegramService, TelegramService>();
        
        services.AddInvestApiClient((_, settings) =>
        {
            settings.AccessToken = configuration.GetValue<string>(KnownSettingsKeys.TinkoffToken);
        });
    }
}