using NLog;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.External.Tinkoff;

namespace Oid85.FinMarket.Application.Services
{
    public class LoadService : ILoadService
    {
        private readonly ILogger _logger;
        private readonly ITinkoffService _tinkoffService;
        private readonly IShareRepository _shareRepository;
        private readonly IBondRepository _bondRepository;
        private readonly ICandleRepository _candleRepository;
        private readonly IDividendInfoRepository _dividendInfoRepository;
        private readonly IBondCouponRepository _bondCouponRepository;

        public LoadService(
            ILogger logger,
            ITinkoffService tinkoffService, 
            IShareRepository shareRepository, 
            IBondRepository bondRepository, 
            ICandleRepository candleRepository, 
            IDividendInfoRepository dividendInfoRepository)
        {
            _logger = logger;
            _tinkoffService = tinkoffService;
            _shareRepository = shareRepository;
            _bondRepository = bondRepository;
            _candleRepository = candleRepository;
            _dividendInfoRepository = dividendInfoRepository;
        }

        public async Task LoadBondsAsync()
        {
            var bonds = await _tinkoffService.GetBondsAsync();
            await _bondRepository.AddOrUpdateAsync(bonds);
        }

        public async Task LoadStocksAsync()
        {
            var shares = await _tinkoffService.GetSharesAsync();
            await _shareRepository.AddOrUpdateAsync(shares);
        }

        public async Task LoadCandlesAsync()
        {
            var shares = await _shareRepository.GetSharesAsync();

            foreach (var share in shares)
            {
                var timeframe = KnownTimeframes.Daily;
                var candles = await _tinkoffService.GetCandlesAsync(share, timeframe);
                await _candleRepository.AddOrUpdateAsync(candles);
            }
        }

        public async Task LoadCandlesAsync(int year)
        {
            var shares = (await _shareRepository.GetSharesAsync())
                .OrderBy(share => share.Ticker)
                .ToList();

            for (int i = 0; i < shares.Count; i++)
            {
                _logger.Trace($"Loading '{shares[i].Ticker}'. {i + 1} of {shares.Count}");
                
                var timeframe = KnownTimeframes.Daily;
                var candles = await _tinkoffService.GetCandlesAsync(shares[i], timeframe, year);
                await _candleRepository.AddOrUpdateAsync(candles);
                
                double percent = ((i + 1) / (double) shares.Count) * 100;
                
                _logger.Trace($"Loaded '{shares[i].Ticker}'. {i + 1} of {shares.Count}. {percent:N2} % completed");
            }
        }

        public async Task LoadDividendInfosAsync()
        {
            var shares = await _shareRepository.GetSharesAsync();
            var dividendInfos = await _tinkoffService.GetDividendInfoAsync(shares);
            await _dividendInfoRepository.AddOrUpdateAsync(dividendInfos);
        }

        public async Task LoadBondCouponsAsync()
        {
            var bonds = await _bondRepository.GetBondsAsync();
            var bondCoupons = await _tinkoffService.GetBondCouponsAsync(bonds);
            await _bondCouponRepository.AddOrUpdateAsync(bondCoupons);
        }
    }
}
