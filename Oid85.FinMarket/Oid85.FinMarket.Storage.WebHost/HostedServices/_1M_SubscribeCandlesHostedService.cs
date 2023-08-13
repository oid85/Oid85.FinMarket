using Grpc.Core;
using Newtonsoft.Json;
using Oid85.FinMarket.Configuration.Common;
using Oid85.FinMarket.Storage.WebHost.Helpers;
using Oid85.FinMarket.Storage.WebHost.Repositories;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;
using Candle = Oid85.FinMarket.Models.Candle;

namespace Oid85.FinMarket.Storage.WebHost.HostedServices
{
    public class _1M_SubscribeCandlesHostedService : IHostedService
    {
        private readonly InvestApiClient _investApiClient;
        private readonly TranslateModelHelper _translateModelHelper;
        private readonly ToolsHelper _toolsHelper;
        private readonly AssetRepository _assetRepository;
        private readonly CandleRepository _candleRepository;
        private readonly ILogger _logger;

        public _1M_SubscribeCandlesHostedService(
            InvestApiClient investApiClient, 
            TranslateModelHelper translateModelHelper,
            ToolsHelper toolsHelper,
            AssetRepository assetRepository,
            CandleRepository candleRepository,
            ILogger logger)
        {
            _investApiClient = investApiClient;
            _translateModelHelper = translateModelHelper;
            _toolsHelper = toolsHelper;
            _assetRepository = assetRepository;
            _candleRepository = candleRepository;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var assets = await _assetRepository.GetAllAssetsAsync();
        
            var stream = _investApiClient.MarketDataStream.MarketDataStream();
        
            for (int i = 0; i < assets.Count; i++)
            {
                // Отправляем запрос в стрим
                await stream.RequestStream.WriteAsync(new MarketDataRequest
                {
                    SubscribeCandlesRequest = new SubscribeCandlesRequest
                    {
                        Instruments =
                        {
                            new CandleInstrument
                            {
                                Figi = assets[i].Figi,
                                Interval = SubscriptionInterval.OneMinute
                            }
                        },
                        SubscriptionAction = SubscriptionAction.Subscribe
                    }
                });
            }
            
            // Обрабатываем все приходящие из стрима ответы
            await foreach (var response in stream.ResponseStream.ReadAllAsync())
            {
                var asset = assets.First(item => item.Figi == response.Candle.Figi);
                var candle = _translateModelHelper.CandleToCandle(response.Candle, asset.Ticker);
                await _candleRepository.SaveCandlesAsync(new List<Candle>() { candle }, TableNames.M1);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}