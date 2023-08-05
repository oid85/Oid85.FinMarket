using Hangfire;
using Oid85.FinMarket.Configuration.Common;
using Oid85.FinMarket.Storage.WebHost.Services;

namespace Oid85.FinMarket.Storage.WebHost.HostedServices
{
    public class _1H_DownloadCandlesHostedService : IHostedService
    {
        private readonly DownloadCandlesService _downloadCandlesService;
        private readonly IConfiguration _configuration;

        public _1H_DownloadCandlesHostedService(
            DownloadCandlesService downloadCandlesService, 
            IConfiguration configuration)
        {
            _downloadCandlesService = downloadCandlesService;
            _configuration = configuration;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            string cron = _configuration.GetValue<string>(ConfigParameterNames.Load_1H_CandlesCronExpression)!;
            RecurringJob.AddOrUpdate($"download-_1H_candles", () => DownloadCandlesAsync(), cron);
        }

        public async Task DownloadCandlesAsync()
        {
            // await _downloadCandlesService.ProcessAssets(TimeframeNames.H);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}