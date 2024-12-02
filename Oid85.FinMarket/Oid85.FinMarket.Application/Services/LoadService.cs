using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.External.Tinkoff;
using Oid85.FinMarket.Logging.Services;

namespace Oid85.FinMarket.Application.Services
{
    public class LoadService(
        ILogService logService,
        ITinkoffService tinkoffService,
        IShareRepository shareRepository,
        IBondRepository bondRepository,
        ICandleRepository candleRepository,
        IDividendInfoRepository dividendInfoRepository,
        IBondCouponRepository bondCouponRepository)
        : ILoadService
    {
        public async Task LoadBondsAsync()
        {
            var bonds = await tinkoffService.GetBondsAsync();
            await bondRepository.AddOrUpdateAsync(bonds);
        }

        public async Task LoadStocksAsync()
        {
            var shares = await tinkoffService.GetSharesAsync();
            await shareRepository.AddOrUpdateAsync(shares);
        }

        public async Task LoadCandlesAsync()
        {
            var shares = await shareRepository.GetSharesAsync();

            foreach (var share in shares)
            {
                var timeframe = KnownTimeframes.Daily;
                var candles = await tinkoffService.GetCandlesAsync(share, timeframe);
                await candleRepository.AddOrUpdateAsync(candles);
            }
        }

        public async Task LoadCandlesAsync(int year)
        {
            var shares = (await shareRepository.GetSharesAsync())
                .OrderBy(share => share.Ticker)
                .ToList();

            for (int i = 0; i < shares.Count; i++)
            {
                await logService.LogTrace($"Loading '{shares[i].Ticker}'. {i + 1} of {shares.Count}");
                
                var timeframe = KnownTimeframes.Daily;
                var candles = await tinkoffService.GetCandlesAsync(shares[i], timeframe, year);
                await candleRepository.AddOrUpdateAsync(candles);
                
                double percent = ((i + 1) / (double) shares.Count) * 100;
                
                await logService.LogTrace($"Loaded '{shares[i].Ticker}'. {i + 1} of {shares.Count}. {percent:N2} % completed");
            }
        }

        public async Task LoadDividendInfosAsync()
        {
            var shares = await shareRepository.GetSharesAsync();
            var dividendInfos = await tinkoffService.GetDividendInfoAsync(shares);
            await dividendInfoRepository.AddOrUpdateAsync(dividendInfos);
        }

        public async Task LoadBondCouponsAsync()
        {
            var bonds = await bondRepository.GetBondsAsync();
            var bondCoupons = await tinkoffService.GetBondCouponsAsync(bonds);
            await bondCouponRepository.AddOrUpdateAsync(bondCoupons);
        }
    }
}
