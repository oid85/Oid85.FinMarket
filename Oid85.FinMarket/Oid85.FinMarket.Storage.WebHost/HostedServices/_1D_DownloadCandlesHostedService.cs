using Hangfire;
using Oid85.FinMarket.Configuration.Common;
using Oid85.FinMarket.Storage.WebHost.Services;

namespace Oid85.FinMarket.Storage.WebHost.HostedServices
{
    public class _1D_DownloadCandlesHostedService : IHostedService
    {
        private readonly DownloadCandlesService _downloadCandlesService;
        private readonly IConfiguration _configuration;

        public _1D_DownloadCandlesHostedService(
            DownloadCandlesService downloadCandlesService, 
            IConfiguration configuration)
        {
            _downloadCandlesService = downloadCandlesService;
            _configuration = configuration;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            string cron = _configuration.GetValue<string>(ConfigParameterNames.Load_1D_CandlesCronExpression)!;
            RecurringJob.AddOrUpdate($"_1D_DownloadCandles", () => DownloadCandlesAsync(), cron);
        }

        public async Task DownloadCandlesAsync()
        {
            await _downloadCandlesService._1D_DownloadCandles();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}