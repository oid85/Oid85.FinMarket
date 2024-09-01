using Google.Api;
using Microsoft.Extensions.DependencyInjection;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.External.Catalogs;
using Oid85.FinMarket.External.Helpers;
using Oid85.FinMarket.External.Postgres;
using Oid85.FinMarket.External.Settings;
using Oid85.FinMarket.External.Tinkoff;

namespace Oid85.FinMarket.External.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public async static void AddExternalServicesAsync(this IServiceCollection services)
        {
            services.AddTransient<PostgresSqlHelper>();
            services.AddTransient<IPostgresService, PostgresService>();
            services.AddTransient<ICatalogService, CatalogService>();
            services.AddTransient<ISettingsService, SettingsService>();
            services.AddTransient<ITinkoffService, TinkoffService>();

            var serviceProvider = services.BuildServiceProvider();
            var settingsService = serviceProvider.GetRequiredService<ISettingsService>();
            string token = await settingsService.GetValueAsync<string>(KnownSettingsKeys.Tinkoff_Token);

            services.AddInvestApiClient((_, settings) =>
            {
                settings.AccessToken = token;
            });
        }
    }
}
