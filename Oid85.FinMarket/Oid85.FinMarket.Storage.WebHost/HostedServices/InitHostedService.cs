using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Configuration.Common;
using Oid85.FinMarket.DAL;
using Oid85.FinMarket.Storage.WebHost.Repositories;
using Oid85.FinMarket.Storage.WebHost.Services;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;
using ILogger = NLog.ILogger;

namespace Oid85.FinMarket.Storage.WebHost.HostedServices
{
    public class InitHostedService : IHostedService
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly DownloadCandlesService _downloadCandlesService;
        private readonly InvestApiClient _investApiClient;
        private readonly AssetRepository _assetRepository;

        public InitHostedService(
            ILogger logger,
            IConfiguration configuration, 
            IServiceScopeFactory scopeFactory, 
            DownloadCandlesService downloadCandlesService, 
            InvestApiClient investApiClient, 
            AssetRepository assetRepository)
        {
            _logger = logger;
            _configuration = configuration;
            _scopeFactory = scopeFactory;
            _downloadCandlesService = downloadCandlesService;
            _investApiClient = investApiClient;
            _assetRepository = assetRepository;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            if (_logger == null)
                throw new NullReferenceException(nameof(_logger));

            _logger.Info($"Запуск сервиса {AppDomain.CurrentDomain.FriendlyName}, {AppCommon.AppVersion}");

            if (_configuration == null)
                throw new NullReferenceException(nameof(_configuration));

            bool debugMode = _configuration.GetValue<bool>(ConfigDebugParameterNames.DebugMode);

            if (debugMode)
                _logger.Warn($"Cервис {AppDomain.CurrentDomain.FriendlyName} запущен в режиме отладки");

            using (var scope = _scopeFactory.CreateScope())
            {
                var dataBaseContext = scope.ServiceProvider.GetRequiredService<StorageDataBaseContext>();
                
                await dataBaseContext.Database.MigrateAsync(cancellationToken);
            }

            var response = await _investApiClient.Instruments.SharesAsync();
            
            var instruments = response.Instruments
                .Where(item => item.RealExchange == RealExchange.Moex)
                .ToList();
                
            for (int i = 0; i < instruments.Count; i++)
            {
                var asset = new Oid85.FinMarket.Models.Asset()
                {
                    Ticker = instruments[i].Ticker,
                    Name = instruments[i].Name,
                    Figi = instruments[i].Figi,
                    Sector = instruments[i].Sector
                };

                await _assetRepository.CreateOrUpdateAsync(asset);
            }

            // await _downloadCandlesService.ProcessAssets(TimeframeNames.D);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            
        }
    }
}