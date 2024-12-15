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
        IFutureRepository futureRepository,
        IBondRepository bondRepository,
        ICandleRepository candleRepository,
        IDividendInfoRepository dividendInfoRepository,
        IBondCouponRepository bondCouponRepository)
        : ILoadService
    {
        public async Task LoadStocksAsync()
        {
            var shares = await tinkoffService.GetSharesAsync();
            await shareRepository.AddOrUpdateAsync(shares);
            
            await logService.LogTrace($"Загружены акции. {shares.Count} шт.");
        }
        
        public async Task LoadFuturesAsync()
        {
            var futures = await tinkoffService.GetFuturesAsync();
            await futureRepository.AddOrUpdateAsync(futures);
            
            await logService.LogTrace($"Загружены фьючерсы. {futures.Count} шт.");
        }
        
        public async Task LoadBondsAsync()
        {
            var bonds = await tinkoffService.GetBondsAsync();
            await bondRepository.AddOrUpdateAsync(bonds);
            
            await logService.LogTrace($"Загружены облигации. {bonds.Count} шт.");
        }

        public async Task LoadCandlesAsync()
        {
            var shares = await shareRepository.GetAllAsync();

            for (int i = 0; i < shares.Count; i++)
            {
                await logService.LogTrace($"Загрузка свечей '{shares[i].Ticker}'. {i + 1} из {shares.Count}");
                
                var timeframe = KnownTimeframes.Daily;
                
                var candles = await tinkoffService.GetCandlesAsync(
                    shares[i].Figi, shares[i].Ticker, timeframe);
                
                await candleRepository.AddOrUpdateAsync(candles);
                
                double percent = ((i + 1) / (double) shares.Count) * 100;
                
                await logService.LogTrace($"Загружены свечи '{shares[i].Ticker}'. {i + 1} из {shares.Count}. {percent:N2} % загружено");
            }
        }

        public async Task LoadCandlesAsync(int year)
        {
            var shares = (await shareRepository.GetAllAsync())
                .OrderBy(share => share.Ticker)
                .ToList();

            for (int i = 0; i < shares.Count; i++)
            {
                await logService.LogTrace($"Загрузка свечей за {year} год '{shares[i].Ticker}'. {i + 1} из {shares.Count}");
                
                var timeframe = KnownTimeframes.Daily;
                
                var candles = await tinkoffService.GetCandlesAsync(
                    shares[i].Figi, shares[i].Ticker, timeframe, year);
                
                await candleRepository.AddOrUpdateAsync(candles);
                
                double percent = ((i + 1) / (double) shares.Count) * 100;
                
                await logService.LogTrace($"Загружены свечи за {year} год '{shares[i].Ticker}'. {i + 1} из {shares.Count}. {percent:N2} % загружено");
            }
        }

        public async Task LoadDividendInfosAsync()
        {
            var shares = await shareRepository.GetAllAsync();
            var dividendInfos = await tinkoffService.GetDividendInfoAsync(shares);
            await dividendInfoRepository.AddOrUpdateAsync(dividendInfos);
            
            await logService.LogTrace($"Загружена информация по дивидендам. {dividendInfos.Count} шт.");
        }

        public async Task LoadBondCouponsAsync()
        {
            var bonds = await bondRepository.GetAllAsync();
            var bondCoupons = await tinkoffService.GetBondCouponsAsync(bonds);
            await bondCouponRepository.AddOrUpdateAsync(bondCoupons);
            
            await logService.LogTrace($"Загружена информация по купонам облигаций. {bondCoupons.Count} шт.");
        }
    }
}
