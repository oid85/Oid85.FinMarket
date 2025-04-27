using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.External.ResourceStore;

namespace Oid85.FinMarket.Application.Services;

public class MultiplicatorService(
    IShareRepository shareRepository,
    IMultiplicatorRepository multiplicatorRepository,
    IResourceStoreService resourceStoreService) 
    : IMultiplicatorService
{
    public async Task<List<Multiplicator>> ProcessMultiplicatorsAsync()
    {
        await FillingMultiplicatorInstrumentsAsync();
        return await CalculateMultiplicatorsAsync();
    }
    
    private async Task FillingMultiplicatorInstrumentsAsync()
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
                TotalSharesAo = multiplicatorResource.TotalSharesAo.Ltm,
                TotalSharesAp = multiplicatorResource.TotalSharesAp.Ltm,
                Beta = multiplicatorResource.Beta.Value,
                Revenue = multiplicatorResource.Revenue.Ltm,
                NetIncome = multiplicatorResource.NetIncome.Ltm,
                OperatingIncome = multiplicatorResource.OperatingIncome.Ltm,
                Ebitda = multiplicatorResource.Ebitda.Ltm,
                Pe = multiplicatorResource.Pe.Ltm,
                Pb = multiplicatorResource.Pb.Ltm,
                Pbv = multiplicatorResource.Pbv.Ltm,
                Ev = multiplicatorResource.Ev.Ltm,
                Roe = multiplicatorResource.Roe.Ltm,
                Roa = multiplicatorResource.Roa.Ltm,
                Eps = multiplicatorResource.Eps.Ltm,
                NetInterestMargin = multiplicatorResource.NetInterestMargin.Ltm,
                TotalDebt = multiplicatorResource.TotalDebt.Ltm,
                NetDebt = multiplicatorResource.NetDebt.Ltm
            };

            multiplicators.Add(multiplicator);
        }

        await multiplicatorRepository.AddOrUpdateAsync(multiplicators);
    }
    
    private async Task<List<Multiplicator>> CalculateMultiplicatorsAsync()
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
                    .GetAsync(multiplicator.TickerAo);
            
                var shareAp = await shareRepository
                    .GetAsync(multiplicator.TickerAp);

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
}