using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.External.ResourceStore;

namespace Oid85.FinMarket.Application.Services;

public class MultiplicatorService(
    IInstrumentRepository instrumenRepository,
    IMultiplicatorRepository multiplicatorRepository,
    IResourceStoreService resourceStoreService) 
    : IMultiplicatorService
{
    public async Task FillingMultiplicatorInstrumentsAsync()
    {
        var tickers = await resourceStoreService.GetSharesWatchlistAsync();

        var multiplicators = new List<Multiplicator>();
        
        foreach (var ticker in tickers)
        {
            var multiplicatorResource = await resourceStoreService
                .GetMultiplicatorLtmAsync(ticker);

            if (multiplicatorResource is not null)
            {
                var multiplicator = new Multiplicator();

                multiplicator.InstrumentId = (await instrumenRepository.GetByTickerAsync(ticker))!.InstrumentId;
                multiplicator.Ticker = ticker;
                multiplicator.TotalSharesAo = multiplicatorResource.TotalSharesAo.Value;
                multiplicator.TotalSharesAp = multiplicatorResource.TotalSharesAp.Value;
                multiplicator.Beta = multiplicatorResource.Beta.Value;
                multiplicator.Revenue = multiplicatorResource.Revenue.Value;
                multiplicator.NetIncome = multiplicatorResource.NetIncome.Value;
                multiplicator.OperatingIncome = multiplicatorResource.OperatingIncome.Value;
                multiplicator.Ebitda = multiplicatorResource.Ebitda.Value;
                multiplicator.Pe = multiplicatorResource.Pe.Value;
                multiplicator.Pb = multiplicatorResource.Pb.Value;
                multiplicator.Pbv = multiplicatorResource.Pbv.Value;
                multiplicator.Ev = multiplicatorResource.Ev.Value;
                multiplicator.Roe = multiplicatorResource.Roe.Value;
                multiplicator.Roa = multiplicatorResource.Roa.Value;
                multiplicator.Eps = multiplicatorResource.Eps.Value;
                multiplicator.NetInterestMargin = multiplicatorResource.NetInterestMargin.Value;
                multiplicator.TotalDebt = multiplicatorResource.TotalDebt.Value;
                multiplicator.NetDebt = multiplicatorResource.NetDebt.Value;
                
                multiplicators.Add(multiplicator);
            }
        }

        await multiplicatorRepository.AddOrUpdateAsync(multiplicators);
    }
    
    public async Task<List<Multiplicator>> CalculateMultiplicatorsAsync()
    {
        var multiplicators = await multiplicatorRepository.GetAllAsync();

        foreach (var multiplicator in multiplicators)
        {
            FillMarketCapitalization(multiplicator);
            FillLowOfYear(multiplicator);
            FillHighOfYear(multiplicator);
            FillEvToEbitda(multiplicator);
            FillTotalDebtToEbitda(multiplicator);
            FillNetDebtToEbitda(multiplicator);
        }
        
        return multiplicators;
        
        void FillMarketCapitalization(Multiplicator multiplicator)
        {
            throw new NotImplementedException();
        }
        
        void FillLowOfYear(Multiplicator multiplicator)
        {
            throw new NotImplementedException();
        }
        
        void FillHighOfYear(Multiplicator multiplicator)
        {
            throw new NotImplementedException();
        }
        
        void FillEvToEbitda(Multiplicator multiplicator)
        {
            throw new NotImplementedException();
        }
        
        void FillTotalDebtToEbitda(Multiplicator multiplicator)
        {
            throw new NotImplementedException();
        }
        
        void FillNetDebtToEbitda(Multiplicator multiplicator)
        {
            throw new NotImplementedException();
        }
    }
}