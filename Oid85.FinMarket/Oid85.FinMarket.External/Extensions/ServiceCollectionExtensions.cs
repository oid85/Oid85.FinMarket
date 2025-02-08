using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Oid85.FinMarket.Common.Helpers;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.External.ResourceStore;
using Oid85.FinMarket.External.Telegram;
using Oid85.FinMarket.External.Tinkoff;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Oid85.FinMarket.External.Extensions;

public static class ServiceCollectionExtensions
{
    public static void ConfigureExternalServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTransient<ITinkoffService, TinkoffService>();
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
    
    public static async Task TelegramBotSubscribe(this IHost host)
    {
        var scopeFactory = host.Services.GetRequiredService<IServiceScopeFactory>();
        await using var scope = scopeFactory.CreateAsyncScope();
        var botClient = scope.ServiceProvider.GetRequiredService<TelegramBotClient>();
        var telegramService = scope.ServiceProvider.GetRequiredService<ITelegramService>();

        botClient.OnMessage += async (message, type) => 
            await telegramService.MessageHandleAsync(message, type);
    }
}