using Hangfire;
using Oid85.FinMarket.Configuration.Common;
using Oid85.FinMarket.Storage.WebHost.Services;

namespace Oid85.FinMarket.Storage.WebHost.HostedServices
{
    public class _1M_DownloadCandlesHostedService : IHostedService
    {
        private readonly DownloadCandlesService _downloadCandlesService;
        private readonly IConfiguration _configuration;

        public _1M_DownloadCandlesHostedService(
            DownloadCandlesService downloadCandlesService, 
            IConfiguration configuration)
        {
            _downloadCandlesService = downloadCandlesService;
            _configuration = configuration;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            string cron = _configuration.GetValue<string>(ConfigParameterNames.Load_1M_CandlesCronExpression)!;
            RecurringJob.AddOrUpdate($"download-_1M_candles", () => DownloadCandlesAsync(), cron);
        }

        public async Task DownloadCandlesAsync()
        {
            // await _downloadCandlesService.ProcessAssets(TimeframeNames.M1);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}