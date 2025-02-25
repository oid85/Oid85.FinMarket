using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Oid85.FinMarket.Common.Helpers;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.External.ResourceStore;
using Oid85.FinMarket.External.Telegram;
using Oid85.FinMarket.External.Tinkoff;
using Oid85.FinMarket.External.Tinkoff.Helpers;
using Telegram.Bot;

namespace Oid85.FinMarket.External.Extensions;

public static class ServiceCollectionExtensions
{
    public static void ConfigureExternalServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTransient<ITinkoffService, TinkoffService>();
        services.AddTransient<GetPricesHelper>();
        services.AddTransient<ITelegramService, TelegramService>();
        services.AddTransient<IResourceStoreService, ResourceStoreService>();
        
        services.AddInvestApiClient((_, settings) =>
        {
            settings.AccessToken = ConvertHelper.Base64Decode(
                configuration.GetValue<string>(KnownSettingsKeys.TinkoffToken)!);
        });

        var botClient = new TelegramBotClient(
            ConvertHelper.Base64Decode(
                configuration.GetValue<string>(KnownSettingsKeys.TelegramToken)!));
        
        services.AddSingleton(botClient);
        
        
    }
}