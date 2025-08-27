using System.Linq.Expressions;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Oid85.FinMarket.Application.Factories;
using Oid85.FinMarket.Application.Helpers;
using Oid85.FinMarket.Application.Interfaces.Factories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Application.Interfaces.Services.DiagramServices;
using Oid85.FinMarket.Application.Interfaces.Services.ReportServices;
using Oid85.FinMarket.Application.Services;
using Oid85.FinMarket.Application.Services.AnalyseServices;
using Oid85.FinMarket.Application.Services.DiagramServices;
using Oid85.FinMarket.Application.Services.ReportServices;
using Oid85.FinMarket.Application.StatisticalArbitrageStrategies;
using Oid85.FinMarket.Application.Strategies;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models.Algo;

namespace Oid85.FinMarket.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void ConfigureApplicationServices(
        this IServiceCollection services)
    {
        services.AddTransient<CandleSequenceAnalyseService>();
        services.AddTransient<CandleVolumeAnalyseService>();
        services.AddTransient<DrawdownFromMaximumAnalyseService>();
        services.AddTransient<RsiAnalyseService>();
        services.AddTransient<SupertrendAnalyseService>();
        services.AddTransient<YieldLtmAnalyseService>();
        services.AddTransient<AtrAnalyseService>();
        services.AddTransient<DonchianAnalyseService>();
        services.AddTransient<HurstAnalyseService>();
        
        services.AddTransient<ColorHelper>();
        services.AddTransient<AlgoHelper>();
        
        services.AddTransient<ISharesReportService, SharesReportService>();
        services.AddTransient<IIndexesReportService, IndexesReportService>();
        services.AddTransient<IFuturesReportService, FuturesReportService>();
        services.AddTransient<ICurrenciesReportService, CurrenciesReportService>();
        services.AddTransient<IBondsReportService, BondsReportService>();
        services.AddTransient<IAlgoReportService, AlgoReportService>();
        
        services.AddTransient<ISharesDiagramService, SharesDiagramService>();
        services.AddTransient<IBondsDiagramService, BondsDiagramService>();
        services.AddTransient<IFuturesDiagramService, FuturesDiagramService>();
        services.AddTransient<ICurrenciesDiagramService, CurrenciesDiagramService>();
        services.AddTransient<IIndexesDiagramService, IndexesDiagramService>();
        services.AddTransient<IAlgoDiagramService, AlgoDiagramService>();
        
        services.AddTransient<ILoadService, LoadService>();
        services.AddTransient<IImportService, ImportService>();
        services.AddTransient<IAnalyseService, AnalyseService>();
        services.AddTransient<ISendService, SendService>();
        services.AddTransient<IJobService, JobService>();
        services.AddTransient<IMarketEventService, MarketEventService>();
        services.AddTransient<ITickerListUtilService, TickerListUtilService>();
        services.AddTransient<INormalizeService, NormalizeService>();
        services.AddTransient<IFeerGreedIndexService, FeerGreedIndexService>();
        services.AddTransient<ISectorIndexService, SectorIndexService>();
        
        services.AddTransient<IAlgoService, AlgoService>();
        services.AddTransient<IAlgoStatisticalArbitrageService, AlgoStatisticalArbitrageService>();
        services.AddTransient<IMoneyManagementService, MoneyManagementService>();
        
        services.AddTransient<ITelegramMessageFactory, TelegramMessageFactory>();
        services.AddTransient<IReportDataFactory, ReportDataFactory>();
        services.AddTransient<IDiagramDataFactory, DiagramDataFactory>();
        services.AddTransient<IIndicatorFactory, IndicatorFactory>();
        
        services.AddKeyedTransient<Strategy, DonchianBreakoutClassicLong>("DonchianBreakoutClassicLong");
        services.AddKeyedTransient<Strategy, DonchianBreakoutClassicShort>("DonchianBreakoutClassicShort");
        services.AddKeyedTransient<Strategy, DonchianBreakoutMiddleLong>("DonchianBreakoutMiddleLong");
        services.AddKeyedTransient<Strategy, DonchianBreakoutMiddleShort>("DonchianBreakoutMiddleShort");
        services.AddKeyedTransient<Strategy, SupertrendLong>("SupertrendLong");
        services.AddKeyedTransient<Strategy, SupertrendShort>("SupertrendShort");
        services.AddKeyedTransient<Strategy, VolatilityBreakoutClassicLong>("VolatilityBreakoutClassicLong");
        services.AddKeyedTransient<Strategy, VolatilityBreakoutClassicShort>("VolatilityBreakoutClassicShort");
        services.AddKeyedTransient<Strategy, VolatilityBreakoutMiddleLong>("VolatilityBreakoutMiddleLong");
        services.AddKeyedTransient<Strategy, VolatilityBreakoutMiddleShort>("VolatilityBreakoutMiddleShort");
        services.AddKeyedTransient<Strategy, UltimateSmootherInclinationLong>("UltimateSmootherInclinationLong");
        services.AddKeyedTransient<Strategy, UltimateSmootherInclinationShort>("UltimateSmootherInclinationShort");
        services.AddKeyedTransient<Strategy, HmaInclinationLong>("HmaInclinationLong");
        services.AddKeyedTransient<Strategy, HmaInclinationShort>("HmaInclinationShort");
        services.AddKeyedTransient<Strategy, BollingerBandsClassicLong>("BollingerBandsClassicLong");
        services.AddKeyedTransient<Strategy, BollingerBandsClassicShort>("BollingerBandsClassicShort");
        services.AddKeyedTransient<Strategy, BollingerBandsMiddleLong>("BollingerBandsMiddleLong");
        services.AddKeyedTransient<Strategy, BollingerBandsMiddleShort>("BollingerBandsMiddleShort");
        services.AddKeyedTransient<Strategy, AdaptivePriceChannelAdxClassicLong>("AdaptivePriceChannelAdxClassicLong");
        services.AddKeyedTransient<Strategy, AdaptivePriceChannelAdxClassicShort>("AdaptivePriceChannelAdxClassicShort");
        services.AddKeyedTransient<Strategy, AdaptivePriceChannelAdxMiddleLong>("AdaptivePriceChannelAdxMiddleLong");
        services.AddKeyedTransient<Strategy, AdaptivePriceChannelAdxMiddleShort>("AdaptivePriceChannelAdxMiddleShort");
        
        services.AddKeyedTransient<StatisticalArbitrageStrategy, CrossStdDevLongShort>("CrossStdDevLongShort");
        services.AddKeyedTransient<StatisticalArbitrageStrategy, CrossStdDevShortLong>("CrossStdDevShortLong");
    }
    
    public static async Task RegisterHangfireJobs(
        this IHost host,
        IConfiguration configuration)
    {
        var scopeFactory = host.Services.GetRequiredService<IServiceScopeFactory>();
        await using var scope = scopeFactory.CreateAsyncScope();
        var jobService = scope.ServiceProvider.GetRequiredService<IJobService>();
        
        RegisterJob(KnownJobs.LoadDailyCandles, () => jobService.LoadDailyCandles());
        RegisterJob(KnownJobs.EarlyInTheDay, () => jobService.EarlyInTheDay());
        RegisterJob(KnownJobs.Every15Minutes, () => jobService.Every15Minutes());
        RegisterJob(KnownJobs.Every10Minutes, () => jobService.Every10Minutes());
        RegisterJob(KnownJobs.Every5Minutes, () => jobService.Every5Minutes());

        void RegisterJob(string configurationSection, Expression<Func<Task>> methodCall)
        {
            bool enable = configuration.GetValue<bool>($"Hangfire:{configurationSection}:Enable");
            string jobId = configuration.GetValue<string>($"Hangfire:{configurationSection}:JobId")!;
            string cron = configuration.GetValue<string>($"Hangfire:{configurationSection}:Cron")!;
        
            if (enable)
                RecurringJob.AddOrUpdate(jobId, methodCall, cron);
        }        
    }
}