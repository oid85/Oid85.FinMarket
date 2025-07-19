namespace Oid85.FinMarket.Domain.Models.Algo;

public class GraphPoint
{
    public double?[] ChannelBands { get; set; } = [null, null];
    
    public double? Indicator { get; set; } = null;
    
    public double? Filter { get; set; } = null;
}