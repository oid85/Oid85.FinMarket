using System.Linq.Expressions;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Common.KnownConstants;

namespace Oid85.FinMarket.Application.HostedService;

public class RegisterHangfireJobs(
    IJobService jobService,
    IConfiguration configuration) 
    : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        RegisterJob(KnownJobs.LoadInstruments, () => jobService.LoadInstrumentsAsync());
        RegisterJob(KnownJobs.LoadPrices, () => jobService.LoadPricesAsync());
        RegisterJob(KnownJobs.LoadBondCoupons, () => jobService.LoadBondCouponsAsync());
        RegisterJob(KnownJobs.LoadDividendInfos, () => jobService.LoadDividendInfosAsync());
        RegisterJob(KnownJobs.LoadAssetFundamentals, () => jobService.LoadAssetFundamentalsAsync());
        RegisterJob(KnownJobs.LoadDailyCandles, () => jobService.LoadDailyCandlesAsync());
        RegisterJob(KnownJobs.Analyse, () => jobService.AnalyseAsync());

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private void RegisterJob(string configurationSection, Expression<Func<Task>> methodCall)
    {
        bool enable = configuration.GetValue<bool>($"Hangfire:{configurationSection}:Enable");
        string jobId = configuration.GetValue<string>($"Hangfire:{configurationSection}:JobId")!;
        string cron = configuration.GetValue<string>($"Hangfire:{configurationSection}:Cron")!;
        
        if (enable)
            RecurringJob.AddOrUpdate(jobId, methodCall, cron);
    }
}