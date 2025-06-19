﻿namespace Oid85.FinMarket.Domain.Models.Algo;

public class Position
{
    public string Ticker { get; set; } = string.Empty;
    public double EntryPrice { get; set; }
    public double ExitPrice { get; set; }
    public DateTime EntryDateTime { get; set; }
    public DateTime ExitDateTime { get; set; }
    public int EntryCandleIndex { get; set; }
    public int ExitCandleIndex { get; set; }
    public bool IsActive { get; set; }
    public bool IsLong { get; set; }
    public bool IsShort { get; set; }
    public int Quantity { get; set; }
    public double Cost { get; set; }
    public double Profit { get; set; }
    public double ProfitPercent { get; set; }
    public double TotalProfit { get; set; }
    public double TotalProfitPct { get; set; }
}