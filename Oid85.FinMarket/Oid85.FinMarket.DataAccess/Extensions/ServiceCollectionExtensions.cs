using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.DataAccess.Interceptors;
using Oid85.FinMarket.DataAccess.Repositories;

namespace Oid85.FinMarket.DataAccess.Extensions;

public static class ServiceCollectionExtensions
{
    public static void ConfigureFinMarketDataAccess(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<UpdateAuditableEntitiesInterceptor>();
                
        services.AddDbContextPool<FinMarketContext>((serviceProvider, options) =>
        {
            var updateInterceptor = serviceProvider.GetRequiredService<UpdateAuditableEntitiesInterceptor>();
                
            options
                .UseNpgsql(configuration.GetValue<string>(KnownSettingsKeys.PostgresFinMarketConnectionString)!)
                .AddInterceptors(updateInterceptor);
        });

        services.AddPooledDbContextFactory<FinMarketContext>(options =>
            options
                .UseNpgsql(configuration.GetValue<string>(KnownSettingsKeys.PostgresFinMarketConnectionString)!)
                .EnableServiceProviderCaching(false), poolSize: 32);
        
        services.AddTransient<IShareRepository, ShareRepository>();
        services.AddTransient<IFutureRepository, FutureRepository>();
        services.AddTransient<IBondRepository, BondRepository>();
        services.AddTransient<IIndexRepository, IndexRepository>();
        services.AddTransient<ICurrencyRepository, CurrencyRepository>();
        services.AddTransient<IBondCouponRepository, BondCouponRepository>();
        services.AddTransient<IDividendInfoRepository, DividendInfoRepository>();
        services.AddTransient<IAnalyseResultRepository, AnalyseResultRepository>();
        services.AddTransient<IDailyCandleRepository, DailyCandleRepository>();
        services.AddTransient<IHourlyCandleRepository, HourlyCandleRepository>();
        services.AddTransient<IInstrumentRepository, InstrumentRepository>();
        services.AddTransient<IShareMultiplicatorRepository, ShareMultiplicatorRepository>();
        services.AddTransient<IBankMultiplicatorRepository, BankMultiplicatorRepository>();
        services.AddTransient<IForecastTargetRepository, ForecastTargetRepository>();
        services.AddTransient<IForecastConsensusRepository, ForecastConsensusRepository>();
        services.AddTransient<IMarketEventRepository, MarketEventRepository>();
        services.AddTransient<IAssetReportEventRepository, AssetReportEventRepository>();
        services.AddTransient<IFeerGreedRepository, FeerGreedRepository>();
        services.AddTransient<IBacktestResultRepository, BacktestResultRepository>();
        services.AddTransient<IOptimizationResultRepository, OptimizationResultRepository>();
        services.AddTransient<IStrategySignalRepository, StrategySignalRepository>();
        services.AddTransient<IPairArbitrageBacktestResultRepository, PairArbitrageBacktestResultRepository>();
        services.AddTransient<IPairArbitrageOptimizationResultRepository, PairArbitrageOptimizationResultRepository>();
        services.AddTransient<IPairArbitrageStrategySignalRepository, PairArbitrageStrategySignalRepository>();
        services.AddTransient<ICorrelationRepository, CorrelationRepository>();
        services.AddTransient<IRegressionTailRepository, RegressionTailRepository>();
    }
    
    public static async Task ApplyMigrations(this IHost host)
    {
        var scopeFactory = host.Services.GetRequiredService<IServiceScopeFactory>();
        await using var scope = scopeFactory.CreateAsyncScope();
        await using var context = scope.ServiceProvider.GetRequiredService<FinMarketContext>();
        await context.Database.MigrateAsync();
    }
}