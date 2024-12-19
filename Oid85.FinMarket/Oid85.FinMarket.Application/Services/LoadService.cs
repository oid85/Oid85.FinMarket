using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;
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
        IIndicativeRepository indicativeRepository,
        ICurrencyRepository currencyRepository,
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

        public async Task LoadStockPricesAsync()
        {
            var shares = await shareRepository.GetAllAsync();
            
            var figiList = shares.Select(x => x.Figi).ToList();
            
            var lastPrices = await tinkoffService.GetPricesAsync(figiList);

            for (int i = 0; i < shares.Count; i++) 
                shares[i].Price = lastPrices[i];
            
            await shareRepository.AddOrUpdateAsync(shares);
            
            await logService.LogTrace($"Загружены последние цены по акциям. {shares.Count} шт.");
        }

        public async Task LoadFuturesAsync()
        {
            var futures = await tinkoffService.GetFuturesAsync();
            await futureRepository.AddOrUpdateAsync(futures);
            
            await logService.LogTrace($"Загружены фьючерсы. {futures.Count} шт.");
        }

        public async Task LoadFuturePricesAsync()
        {
            var futures = await futureRepository.GetAllAsync();
            
            var figiList = futures.Select(x => x.Figi).ToList();
            
            var lastPrices = await tinkoffService.GetPricesAsync(figiList);

            for (int i = 0; i < futures.Count; i++) 
                futures[i].Price = lastPrices[i];
            
            await futureRepository.AddOrUpdateAsync(futures);
            
            await logService.LogTrace($"Загружены последние цены по фьючерсам. {futures.Count} шт.");
        }

        public async Task LoadIndicativePricesAsync()
        {
            var indicatives = await indicativeRepository.GetAllAsync();
            
            var figiList = indicatives.Select(x => x.Figi).ToList();
            
            var lastPrices = await tinkoffService.GetPricesAsync(figiList);

            for (int i = 0; i < indicatives.Count; i++) 
                indicatives[i].Price = lastPrices[i];
            
            await indicativeRepository.AddOrUpdateAsync(indicatives);
            
            await logService.LogTrace($"Загружены последние цены по индикативам. {indicatives.Count} шт.");
        }

        public async Task LoadCurrenciesAsync()
        {
            var currencies = await tinkoffService.GetCurrenciesAsync();
            await currencyRepository.AddOrUpdateAsync(currencies);
            
            await logService.LogTrace($"Загружены валюты. {currencies.Count} шт.");
        }

        public async Task LoadCurrencyPricesAsync()
        {
            var currencies = await currencyRepository.GetAllAsync();
            
            var figiList = currencies.Select(x => x.Figi).ToList();
            
            var lastPrices = await tinkoffService.GetPricesAsync(figiList);

            for (int i = 0; i < currencies.Count; i++) 
                currencies[i].Price = lastPrices[i];
            
            await currencyRepository.AddOrUpdateAsync(currencies);
            
            await logService.LogTrace($"Загружены последние цены по валютам. {currencies.Count} шт.");
        }

        public async Task LoadBondsAsync()
        {
            var bonds = await tinkoffService.GetBondsAsync();
            await bondRepository.AddOrUpdateAsync(bonds);
            
            await logService.LogTrace($"Загружены облигации. {bonds.Count} шт.");
        }

        public async Task LoadIndicativesAsync()
        {
            var indicatives = await tinkoffService.GetIndicativesAsync();
            await indicativeRepository.AddOrUpdateAsync(indicatives);
            
            await logService.LogTrace($"Загружены индикативные инструменты. {indicatives.Count} шт.");
        }
        
        public async Task LoadStocksDailyCandlesAsync()
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

        public async Task LoadStocksDailyCandlesAsync(Share share)
        {
            await logService.LogTrace($"Загрузка свечей '{share.Ticker}'");
                
            var timeframe = KnownTimeframes.Daily;
                
            var candles = await tinkoffService.GetCandlesAsync(
                share.Figi, share.Ticker, timeframe);
                
            await candleRepository.AddOrUpdateAsync(candles);
                
            await logService.LogTrace($"Загружены свечи '{share.Ticker}'");
        }         
        
        public async Task LoadStocksDailyCandlesAsync(int year)
        {
            var shares = await shareRepository.GetAllAsync();
            
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

        public async Task LoadFuturesDailyCandlesAsync()
        {
            var futures = await futureRepository.GetAllAsync();

            for (int i = 0; i < futures.Count; i++)
            {
                await logService.LogTrace($"Загрузка свечей '{futures[i].Ticker}'. {i + 1} из {futures.Count}");
                
                var timeframe = KnownTimeframes.Daily;
                
                var candles = await tinkoffService.GetCandlesAsync(
                    futures[i].Figi, futures[i].Ticker, timeframe);
                
                await candleRepository.AddOrUpdateAsync(candles);
                
                double percent = ((i + 1) / (double) futures.Count) * 100;
                
                await logService.LogTrace($"Загружены свечи '{futures[i].Ticker}'. {i + 1} из {futures.Count}. {percent:N2} % загружено");
            }
        }

        public async Task LoadFuturesDailyCandlesAsync(Future future)
        {
            await logService.LogTrace($"Загрузка свечей '{future.Ticker}'");
                
            var timeframe = KnownTimeframes.Daily;
                
            var candles = await tinkoffService.GetCandlesAsync(
                future.Figi, future.Ticker, timeframe);
                
            await candleRepository.AddOrUpdateAsync(candles);
                
            await logService.LogTrace($"Загружены свечи '{future.Ticker}'");
        }        
        
        public async Task LoadFuturesDailyCandlesAsync(int year)
        {
            var futures = await futureRepository.GetAllAsync();
            
            for (int i = 0; i < futures.Count; i++)
            {
                await logService.LogTrace($"Загрузка свечей за {year} год '{futures[i].Ticker}'. {i + 1} из {futures.Count}");
                
                var timeframe = KnownTimeframes.Daily;
                
                var candles = await tinkoffService.GetCandlesAsync(
                    futures[i].Figi, futures[i].Ticker, timeframe, year);
                
                await candleRepository.AddOrUpdateAsync(candles);
                
                double percent = ((i + 1) / (double) futures.Count) * 100;
                
                await logService.LogTrace($"Загружены свечи за {year} год '{futures[i].Ticker}'. {i + 1} из {futures.Count}. {percent:N2} % загружено");
            }
        }

        public async Task LoadBondPricesAsync()
        {
            var bonds = await bondRepository.GetAllAsync();
            
            var figiList = bonds.Select(x => x.Figi).ToList();
            
            var lastPrices = await tinkoffService.GetPricesAsync(figiList);

            for (int i = 0; i < bonds.Count; i++) 
                bonds[i].Price = lastPrices[i];
            
            await bondRepository.AddOrUpdateAsync(bonds);
            
            await logService.LogTrace($"Загружены последние цены по облигациям. {bonds.Count} шт.");
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
