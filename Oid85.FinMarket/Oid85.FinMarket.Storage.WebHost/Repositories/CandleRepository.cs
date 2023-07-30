using Oid85.FinMarket.Models;

namespace Oid85.FinMarket.Storage.WebHost.Repositories;

public class CandleRepository
{
    public async Task<Candle> GetLastOneDayCandleByAssetAsync()
    {
        return new Candle();
    }
    
    public async Task<Candle> GetLastOneMinuteCandleByAssetAsync()
    {
        return new Candle();
    }
}