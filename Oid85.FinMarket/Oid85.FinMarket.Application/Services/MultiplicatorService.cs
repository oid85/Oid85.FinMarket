using NLog;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.External.ResourceStore;

namespace Oid85.FinMarket.Application.Services;

public class MultiplicatorService(
    ILogger logger,
    IShareRepository shareRepository,
    IMultiplicatorRepository multiplicatorRepository,
    IResourceStoreService resourceStoreService) 
    : IMultiplicatorService
{
    public async Task FillingMultiplicatorInstrumentsAsync()
    {
        try
        {
            var multiplicators = new List<Multiplicator>();

            var multiplicatorResources = await resourceStoreService
                .GetMultiplicatorsLtmAsync();
        
            foreach (var multiplicatorResource in multiplicatorResources)
            {
                var multiplicator = new Multiplicator
                {
                    TickerAo = multiplicatorResource.TickerAo.Value ?? string.Empty,
                    TickerAp = multiplicatorResource.TickerAp.Value ?? string.Empty,
                    TotalSharesAo = multiplicatorResource.TotalSharesAo.Value,
                    TotalSharesAp = multiplicatorResource.TotalSharesAp.Value,
                    Beta = multiplicatorResource.Beta.Value,
                    Revenue = multiplicatorResource.Revenue.Value,
                    NetIncome = multiplicatorResource.NetIncome.Value,
                    OperatingIncome = multiplicatorResource.OperatingIncome.Value,
                    Ebitda = multiplicatorResource.Ebitda.Value,
                    Pe = multiplicatorResource.Pe.Value,
                    Pb = multiplicatorResource.Pb.Value,
                    Pbv = multiplicatorResource.Pbv.Value,
                    Ev = multiplicatorResource.Ev.Value,
                    Roe = multiplicatorResource.Roe.Value,
                    Roa = multiplicatorResource.Roa.Value,
                    Eps = multiplicatorResource.Eps.Value,
                    NetInterestMargin = multiplicatorResource.NetInterestMargin.Value,
                    TotalDebt = multiplicatorResource.TotalDebt.Value,
                    NetDebt = multiplicatorResource.NetDebt.Value
                };

                multiplicators.Add(multiplicator);
            }

            await multiplicatorRepository.AddOrUpdateAsync(multiplicators);
        }
        
        catch (Exception exception)
        {
            logger.Error(exception);
        }
    }
    
    public async Task<List<Multiplicator>> CalculateMultiplicatorsAsync()
    {
        try
        {
            var multiplicators = await multiplicatorRepository.GetAllAsync();

            foreach (var multiplicator in multiplicators)
            {
                multiplicator.MarketCapitalization = await GetMarketCapitalization(multiplicator);
                multiplicator.EvToEbitda = GetEvToEbitda(multiplicator);
                multiplicator.TotalDebtToEbitda = GetTotalDebtToEbitda(multiplicator);
                multiplicator.NetDebtToEbitda = GetNetDebtToEbitda(multiplicator);
            }
        
            foreach (var multiplicator in multiplicators) 
                await multiplicatorRepository.UpdateCalculateFieldsAsync(multiplicator);
            
            return multiplicators;
        
            // Расчет рыночной капитализации
            async Task<double> GetMarketCapitalization(Multiplicator multiplicator)
            {
                var shareAo = await shareRepository
                    .GetByTickerAsync(multiplicator.TickerAo);
            
                var shareAp = await shareRepository
                    .GetByTickerAsync(multiplicator.TickerAp);

                double priceAo = shareAo?.LastPrice ?? 0.0;
                double priceAp = shareAp?.LastPrice ?? 0.0;

                double mCap = multiplicator.TotalSharesAo * priceAo + multiplicator.TotalSharesAp * priceAp;
            
                return mCap;
            }
        
            // Расчет EV / EBITDA
            double GetEvToEbitda(Multiplicator multiplicator)
            {
                if (multiplicator.Ev == 0.0)
                    return 0.0;
            
                if (multiplicator.Ebitda == 0.0)
                    return 0.0;
            
                return multiplicator.EvToEbitda = multiplicator.Ev / multiplicator.Ebitda;
            }
        
            // Расчет TotalDebt / EBITDA
            double GetTotalDebtToEbitda(Multiplicator multiplicator)
            {
                if (multiplicator.TotalDebt == 0.0)
                    return 0.0;
            
                if (multiplicator.Ebitda == 0.0)
                    return 0.0;
            
                return multiplicator.TotalDebtToEbitda = multiplicator.TotalDebt / multiplicator.Ebitda;
            }
        
            // Расчет NetDebt / EBITDA
            double GetNetDebtToEbitda(Multiplicator multiplicator)
            {
                if (multiplicator.NetDebt == 0.0)
                    return 0.0;
            
                if (multiplicator.Ebitda == 0.0)
                    return 0.0;
            
                return multiplicator.NetDebtToEbitda = multiplicator.NetDebt / multiplicator.Ebitda;
            }
        }
        
        catch (Exception exception)
        {
            logger.Error(exception);
            return [];
        }
    }
}