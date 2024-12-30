using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.DataAccess.Interceptors;
using Oid85.FinMarket.DataAccess.Mapping;
using Oid85.FinMarket.DataAccess.Repositories;

namespace Oid85.FinMarket.DataAccess.Extensions;

public static class ServiceCollectionExtensions
{
    public static void ConfigureFinMarketDataAccess(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<UpdateAuditableEntitiesInterceptor>();
                
        services.AddDbContext<FinMarketContext>((serviceProvider, options) =>
        {
            var updateInterceptor = serviceProvider.GetRequiredService<UpdateAuditableEntitiesInterceptor>();
                
            options
                .UseNpgsql(configuration
                    .GetValue<string>(KnownSettingsKeys.PostgresFinMarketConnectionString))
                .AddInterceptors(updateInterceptor);
        });

        var mapsterConfig = new MapsterConfig();
        services.AddSingleton<MapsterConfig>();
            
        services.AddTransient<IShareRepository, ShareRepository>();
        services.AddTransient<IFutureRepository, FutureRepository>();
        services.AddTransient<IBondRepository, BondRepository>();
        services.AddTransient<IIndexRepository, IndexRepository>();
        services.AddTransient<ICurrencyRepository, CurrencyRepository>();
        services.AddTransient<IBondCouponRepository, BondCouponRepository>();
        services.AddTransient<IDividendInfoRepository, DividendInfoRepository>();
        services.AddTransient<IAnalyseResultRepository, AnalyseResultRepository>();
        services.AddTransient<ICandleRepository, CandleRepository>();
        services.AddTransient<IAssetFundamentalRepository, AssetFundamentalRepository>();
        services.AddTransient<IInstrumentRepository, InstrumentRepository>();
        services.AddTransient<ISpreadRepository, SpreadRepository>();
    }

    public static async Task ApplyMigrations(this IHost host)
    {
        var scopeFactory = host.Services.GetRequiredService<IServiceScopeFactory>();
        await using var scope = scopeFactory.CreateAsyncScope();
        await using var context = scope.ServiceProvider.GetRequiredService<FinMarketContext>();
        await context.Database.MigrateAsync();
    }
}