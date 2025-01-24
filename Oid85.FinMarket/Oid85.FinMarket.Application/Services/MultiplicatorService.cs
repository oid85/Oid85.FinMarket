using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Services;

public class MultiplicatorService(
    IShareRepository shareRepository,
    IMultiplicatorRepository multiplicatorRepository) : IMultiplicatorService
{
    public async Task<List<Multiplicator>> CalculateMultiplicatorsAsync()
    {
        var multiplicators = await multiplicatorRepository.GetAllAsync();

        await multiplicatorRepository.AddAsync(multiplicators);
        
        return multiplicators;
    }

    public async Task FillingMultiplicatorInstrumentsAsync()
    {
        var shares = await shareRepository.GetWatchListAsync();

        var multiplicators = new List<Multiplicator>();

        foreach (var share in shares)
        {
            var multiplicator = new Multiplicator
            {
                InstrumentId = share.InstrumentId,
                Ticker = share.Ticker
            };

            multiplicators.Add(multiplicator);
        }
        
        await multiplicatorRepository.AddAsync(multiplicators);
    }
}