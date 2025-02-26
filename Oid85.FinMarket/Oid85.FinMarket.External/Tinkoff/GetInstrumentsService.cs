using NLog;
using Oid85.FinMarket.Common.Helpers;
using Oid85.FinMarket.Domain.Models;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;
using Share = Oid85.FinMarket.Domain.Models.Share;
using Bond = Oid85.FinMarket.Domain.Models.Bond;
using Currency = Oid85.FinMarket.Domain.Models.Currency;
using Future = Oid85.FinMarket.Domain.Models.Future;
using TinkoffShare = Tinkoff.InvestApi.V1.Share;
using TinkoffFuture = Tinkoff.InvestApi.V1.Future;
using TinkoffBond = Tinkoff.InvestApi.V1.Bond;
using TinkoffCurrency = Tinkoff.InvestApi.V1.Currency;

namespace Oid85.FinMarket.External.Tinkoff;

public class GetInstrumentsService(
    ILogger logger,
    InvestApiClient client)
{
    private const int DelayInMilliseconds = 50;
    private readonly List<string> _badTickerSymbols = [ "@", "-" ]; 
    
    public async Task<List<Share>> GetSharesAsync()
    {
        try
        {
            await Task.Delay(DelayInMilliseconds);
            
            List<TinkoffShare> tinkoffShares = (await client.Instruments
                    .SharesAsync()).Instruments
                .Where(x => x.CountryOfRisk.ToLower() == "ru")
                .ToList(); 

            var result = new List<Share>();

            foreach (var tinkoffShare in tinkoffShares)
            {
                var share = GetModel(tinkoffShare);
                    
                if (share is not null)
                    result.Add(share);
            }

            return result;
        }

        catch (Exception exception)
        {
            logger.Error(exception);
            return [];
        }
    }

    private Share? GetModel(TinkoffShare tinkoffShare)
    {
        bool containsBadSymbol = _badTickerSymbols
            .Any(x => tinkoffShare.Ticker.Contains(x));
        
        if (containsBadSymbol)
        {
            logger.Warn("Тикер с небуквенными символами '{tinkoffShare.Ticker}'", tinkoffShare.Ticker);
            return null;
        }

        return new Share
        {
            Ticker = tinkoffShare.Ticker,
            Figi = tinkoffShare.Figi,
            InstrumentId = Guid.Parse(tinkoffShare.Uid),
            Isin = tinkoffShare.Isin,
            Name = tinkoffShare.Name,
            Sector = tinkoffShare.Sector
        };
    }

    public async Task<List<Future>> GetFuturesAsync()
    {
        try
        {
            await Task.Delay(DelayInMilliseconds);
            
            List<TinkoffFuture> tinkoffFutures = (await client.Instruments
                    .FuturesAsync()).Instruments
                .Where(x => x.CountryOfRisk.ToLower() == "ru")
                .ToList(); 

            var result = new List<Future>();

            foreach (var tinkoffFuture in tinkoffFutures)
            {
                var future = GetModel(tinkoffFuture);
                    
                if (future is not null)
                    result.Add(future);
            }

            return result;
        }

        catch (Exception exception)
        {
            logger.Error(exception);
            return [];
        }
    }
    
    private Future? GetModel(TinkoffFuture tinkoffFuture)
    {
        bool containsBadSymbol = _badTickerSymbols
            .Any(x => tinkoffFuture.Ticker.Contains(x));
        
        if (containsBadSymbol)
        {
            logger.Warn("Тикер с небуквенными символами '{tinkoffFuture.Ticker}'", tinkoffFuture.Ticker);
            return null;
        }

        return new Future
        {
            Ticker = tinkoffFuture.Ticker,
            Figi = tinkoffFuture.Figi,
            Name = tinkoffFuture.Name,
            InstrumentId = Guid.Parse(tinkoffFuture.Uid),
            ExpirationDate = ConvertHelper.TimestampToDateOnly(tinkoffFuture.ExpirationDate),
            Lot = tinkoffFuture.Lot,
            FirstTradeDate = ConvertHelper.TimestampToDateOnly(tinkoffFuture.FirstTradeDate),
            LastTradeDate = ConvertHelper.TimestampToDateOnly(tinkoffFuture.LastTradeDate),
            FutureType = tinkoffFuture.FuturesType,
            AssetType = tinkoffFuture.AssetType,
            BasicAsset = tinkoffFuture.BasicAsset,
            BasicAssetSize = ConvertHelper.QuotationToDouble(tinkoffFuture.BasicAssetSize),
            InitialMarginOnBuy = ConvertHelper.MoneyValueToDouble(tinkoffFuture.InitialMarginOnBuy),
            InitialMarginOnSell = ConvertHelper.MoneyValueToDouble(tinkoffFuture.InitialMarginOnSell),
            MinPriceIncrementAmount = ConvertHelper.QuotationToDouble(tinkoffFuture.MinPriceIncrementAmount)
        };
    }
    
    public async Task<List<Bond>> GetBondsAsync()
    {
        try
        {
            await Task.Delay(DelayInMilliseconds);
            
            List<TinkoffBond> tinkoffBonds = (await client.Instruments.BondsAsync())
                .Instruments
                .ToList();

            var result = new List<Bond>();

            foreach (var tinkoffBond in tinkoffBonds)
            {
                var bond = GetModel(tinkoffBond);

                if (bond is not null)
                    result.Add(bond);
            }

            return result;
        }

        catch (Exception exception)
        {
            logger.Error(exception);
            return [];
        }
    }
    
    private Bond? GetModel(TinkoffBond tinkoffBond)
    {
        bool containsBadSymbol = _badTickerSymbols
            .Any(x => tinkoffBond.Ticker.Contains(x));
        
        if (containsBadSymbol)
        {
            logger.Warn("Тикер с небуквенными символами '{tinkoffBond.Ticker}'", tinkoffBond.Ticker);
            return null;
        }

        return new Bond
        {
            Ticker = tinkoffBond.Ticker,
            Figi = tinkoffBond.Figi,
            Isin = tinkoffBond.Isin,
            Name = tinkoffBond.Name,
            InstrumentId = Guid.Parse(tinkoffBond.Uid),
            Sector = tinkoffBond.Sector,
            Currency = tinkoffBond.Currency,
            Nkd = ConvertHelper.MoneyValueToDouble(tinkoffBond.AciValue),
            MaturityDate = ConvertHelper.TimestampToDateOnly(tinkoffBond.MaturityDate),
            FloatingCouponFlag = tinkoffBond.FloatingCouponFlag,
            RiskLevel = tinkoffBond.RiskLevel switch
            {
                RiskLevel.Low => 1,
                RiskLevel.Moderate => 2,
                RiskLevel.High => 3,
                RiskLevel.Unspecified => 0,
                _ => 0
            }
        };
    }
    
    public async Task<List<FinIndex>> GetIndexesAsync()
    {
        try
        {
            await Task.Delay(DelayInMilliseconds);
            
            var request = new IndicativesRequest();
                
            var indicatives = (await client.Instruments
                    .IndicativesAsync(request))
                .Instruments
                .ToList();

            var result = new List<FinIndex>();

            foreach (var indicative in indicatives)
            {
                var finIndex = GetModel(indicative);

                if (finIndex is not null)
                    result.Add(finIndex);
            }

            return result;
        }

        catch (Exception exception)
        {
            logger.Error(exception);
            return [];
        }
    }
    
    private FinIndex? GetModel(IndicativeResponse indicativeResponse)
    {
        bool containsBadSymbol = _badTickerSymbols
            .Any(x => indicativeResponse.Ticker.Contains(x));
        
        if (containsBadSymbol)
        {
            logger.Warn("Тикер с небуквенными символами '{indicativeResponse.Ticker}'", indicativeResponse.Ticker);
            return null;
        }

        return new FinIndex
        {
            Figi = indicativeResponse.Figi,
            Ticker = indicativeResponse.Ticker,
            ClassCode = indicativeResponse.ClassCode,
            Currency = indicativeResponse.Currency,
            InstrumentKind = indicativeResponse.InstrumentKind.ToString(),
            Name = indicativeResponse.Name,
            Exchange = indicativeResponse.Exchange,
            InstrumentId = Guid.Parse(indicativeResponse.Uid)
        };
    }
    
    public async Task<List<Currency>> GetCurrenciesAsync()
    {
        try
        {
            await Task.Delay(DelayInMilliseconds);
            
            var request = new InstrumentsRequest();
                
            var tinkoffCurrencies = (await client.Instruments
                    .CurrenciesAsync(request))
                .Instruments
                .ToList();

            var result = new List<Currency>();

            foreach (var tinkoffCurrency in tinkoffCurrencies)
            {
                var currency = GetModel(tinkoffCurrency);

                if (currency is not null)
                    result.Add(currency);
            }

            return result;
        }

        catch (Exception exception)
        {
            logger.Error(exception);
            return [];
        }
    }

    private Currency? GetModel(TinkoffCurrency tinkoffCurrency)
    {
        bool containsBadSymbol = _badTickerSymbols
            .Any(x => tinkoffCurrency.Ticker.Contains(x));

        if (containsBadSymbol)
        {
            logger.Warn("Тикер с небуквенными символами '{tinkoffCurrency.Ticker}'", tinkoffCurrency.Ticker);
            return null;
        }

        return new Currency
        {
            Ticker = tinkoffCurrency.Ticker,
            Isin = tinkoffCurrency.Isin,
            Figi = tinkoffCurrency.Figi,
            ClassCode = tinkoffCurrency.ClassCode,
            Name = tinkoffCurrency.Name,
            IsoCurrencyName = tinkoffCurrency.IsoCurrencyName,
            InstrumentId = Guid.Parse(tinkoffCurrency.Uid)
        };
    }
}