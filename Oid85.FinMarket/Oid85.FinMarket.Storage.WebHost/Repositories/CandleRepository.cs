using Oid85.FinMarket.Models;

namespace Oid85.FinMarket.Storage.WebHost.Repositories;

public class CandleRepository
{
    public async Task<Candle> GetLastOneDayCandleByAsset()
    {
        return new Candle();
    }
    
    public async Task<Candle> GetLastOneMinuteCandleByAsset()
    {
        return new Candle();
    }
}