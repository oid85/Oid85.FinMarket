namespace Oid85.FinMarket.Domain.Models.Algo;

public struct ArbitrageTrade
{
    public DateTime DateTime { get; set; }
    public (int First, int Second) Quantity { get; set; }
    public (double First, double Second) Price { get; set; }
    public int CandleIndex { get; set; }
}