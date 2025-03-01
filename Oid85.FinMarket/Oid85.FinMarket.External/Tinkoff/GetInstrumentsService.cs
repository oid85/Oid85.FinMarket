using NLog;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.External.Mapping;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;
using Share = Oid85.FinMarket.Domain.Models.Share;
using Bond = Oid85.FinMarket.Domain.Models.Bond;
using Currency = Oid85.FinMarket.Domain.Models.Currency;
using Future = Oid85.FinMarket.Domain.Models.Future;
using TinkoffShare = Tinkoff.InvestApi.V1.Share;
using TinkoffFuture = Tinkoff.InvestApi.V1.Future;
using TinkoffBond = Tinkoff.InvestApi.V1.Bond;

namespace Oid85.FinMarket.External.Tinkoff;

public class GetInstrumentsService(
    ILogger logger,
    InvestApiClient client)
{
    private const int DelayInMilliseconds = 50;
    
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
                var share = TinkoffMapper.Map(tinkoffShare);
                    
                if (share is not null)
                    result.Add(share);
            }

            return result;
        }

        catch (Exception exception)
        {
            logger.Error(exception, "Ошибка получения данных");
            return [];
        }
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
                var future = TinkoffMapper.Map(tinkoffFuture);
                    
                if (future is not null)
                    result.Add(future);
            }

            return result;
        }

        catch (Exception exception)
        {
            logger.Error(exception, "Ошибка получения данных");
            return [];
        }
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
                var bond = TinkoffMapper.Map(tinkoffBond);

                if (bond is not null)
                    result.Add(bond);
            }

            return result;
        }

        catch (Exception exception)
        {
            logger.Error(exception, "Ошибка получения данных");
            return [];
        }
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
                var finIndex = TinkoffMapper.Map(indicative);

                if (finIndex is not null)
                    result.Add(finIndex);
            }

            return result;
        }

        catch (Exception exception)
        {
            logger.Error(exception, "Ошибка получения данных");
            return [];
        }
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
                var currency = TinkoffMapper.Map(tinkoffCurrency);

                if (currency is not null)
                    result.Add(currency);
            }

            return result;
        }

        catch (Exception exception)
        {
            logger.Error(exception, "Ошибка получения данных");
            return [];
        }
    }
}