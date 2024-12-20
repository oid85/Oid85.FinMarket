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
        if (configuration.GetValue<bool>(KnownSettingsKeys.HangfireLoadInstrumentsEnable))
            RecurringJob.AddOrUpdate(
                KnownSettingsKeys.HangfireLoadInstrumentsJobId, 
                () => jobService.LoadInstrumentsAsync(), 
                KnownSettingsKeys.HangfireLoadInstrumentsCron);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}