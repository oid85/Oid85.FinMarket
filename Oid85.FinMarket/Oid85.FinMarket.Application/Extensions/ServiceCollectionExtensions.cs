﻿using System.Linq.Expressions;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Oid85.FinMarket.Application.Factories;
using Oid85.FinMarket.Application.Helpers;
using Oid85.FinMarket.Application.Interfaces.Factories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Application.Interfaces.Services.ReportServices;
using Oid85.FinMarket.Application.Services;
using Oid85.FinMarket.Application.Services.AnalyseServices;
using Oid85.FinMarket.Application.Services.ReportServices;
using Oid85.FinMarket.Common.KnownConstants;

namespace Oid85.FinMarket.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void ConfigureApplicationServices(
        this IServiceCollection services)
    {
        services.AddTransient<ILoadService, LoadService>();
        services.AddTransient<IAnalyseService, AnalyseService>();
        services.AddTransient<CandleSequenceAnalyseService>();
        services.AddTransient<CandleVolumeAnalyseService>();
        services.AddTransient<DrawdownFromMaximumAnalyseService>();
        services.AddTransient<RsiAnalyseService>();
        services.AddTransient<SupertrendAnalyseService>();
        services.AddTransient<YieldLtmAnalyseService>();
        services.AddTransient<ISharesReportService, SharesReportService>();
        services.AddTransient<IIndexesReportService, IndexesReportService>();
        services.AddTransient<IFuturesReportService, FuturesReportService>();
        services.AddTransient<ICurrenciesReportService, CurrenciesReportService>();
        services.AddTransient<IBondsReportService, BondsReportService>();
        services.AddTransient<IMarketEventsReportService, MarketEventsReportService>();
        services.AddTransient<ISendService, SendService>();
        services.AddTransient<ColorHelper>();
        services.AddTransient<IJobService, JobService>();
        services.AddTransient<ISpreadService, SpreadService>();
        services.AddTransient<IMultiplicatorService, MultiplicatorService>();
        services.AddTransient<IMarketEventService, MarketEventService>();
        services.AddTransient<IInstrumentService, InstrumentService>();
        services.AddTransient<INormalizeService, NormalizeService>();
        
        services.AddTransient<ITelegramMessageFactory, TelegramMessageFactory>();
        services.AddTransient<IReportDataFactory, ReportDataFactory>();
    }
    
    public static async Task RegisterHangfireJobs(
        this IHost host,
        IConfiguration configuration)
    {
        var scopeFactory = host.Services.GetRequiredService<IServiceScopeFactory>();
        await using var scope = scopeFactory.CreateAsyncScope();
        var jobService = scope.ServiceProvider.GetRequiredService<IJobService>();
        
        RegisterJob(KnownJobs.EarlyInTheDay, () => jobService.EarlyInTheDay());
        RegisterJob(KnownJobs.Every15Minutes, () => jobService.Every15Minutes());

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