﻿using System.Linq.Expressions;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Application.Services;
using Oid85.FinMarket.Common.KnownConstants;

namespace Oid85.FinMarket.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void ConfigureApplicationServices(
        this IServiceCollection services)
    {
        services.AddTransient<ILoadService, LoadService>();
        services.AddTransient<IAnalyseService, AnalyseService>();
        services.AddTransient<IReportService, ReportService>();
        services.AddTransient<IJobService, JobService>();
        services.AddTransient<ISpreadService, SpreadService>();
    }
    
    public static async Task RegisterHangfireJobs(
        this IHost host,
        IConfiguration configuration)
    {
        var scopeFactory = host.Services.GetRequiredService<IServiceScopeFactory>();
        await using var scope = scopeFactory.CreateAsyncScope();
        var jobService = scope.ServiceProvider.GetRequiredService<IJobService>();
        
        RegisterJob(KnownJobs.LoadInstruments, () => jobService.LoadInstrumentsAsync());
        RegisterJob(KnownJobs.LoadPrices, () => jobService.LoadLastPricesAsync());
        RegisterJob(KnownJobs.LoadBondCoupons, () => jobService.LoadBondCouponsAsync());
        RegisterJob(KnownJobs.LoadDividendInfos, () => jobService.LoadDividendInfosAsync());
        RegisterJob(KnownJobs.LoadDailyCandles, () => jobService.LoadDailyCandlesAsync());
        RegisterJob(KnownJobs.Analyse, () => jobService.AnalyseAsync());
        
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