﻿using Microsoft.Extensions.DependencyInjection;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.External.Catalogs;
using Oid85.FinMarket.External.Helpers;
using Oid85.FinMarket.External.Portfolio;
using Oid85.FinMarket.External.Postgres;
using Oid85.FinMarket.External.Settings;
using Oid85.FinMarket.External.Tinkoff;

namespace Oid85.FinMarket.External.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddExternalServices(this IServiceCollection services)
        {
            services.AddTransient<PostgresSqlHelper>();
            services.AddTransient<IPostgresService, PostgresService>();
            services.AddTransient<ICatalogService, CatalogService>();
            services.AddTransient<ISettingsService, SettingsService>();
            services.AddTransient<IPortfolioService, PortfolioService>();
            services.AddTransient<ITinkoffService, TinkoffService>();

            var serviceProvider = services.BuildServiceProvider();
            var settingsService = serviceProvider.GetRequiredService<ISettingsService>();

            if (settingsService == null)
                throw new NullReferenceException(nameof(settingsService));

            string token = settingsService.GetValueAsync<string>(KnownSettingsKeys.Tinkoff_Token).GetAwaiter().GetResult();

            services.AddInvestApiClient((_, settings) =>
            {
                settings.AccessToken = token;
            });
        }
    }
}
