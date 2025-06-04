using Oid85.FinMarket.Strategies.Models;
using WealthLab;

namespace Oid85.FinMarket.Strategies.Indicators
{
    public interface IBarIndicator
    {
        Bars Init(List<Candle> candles);
    }
}
