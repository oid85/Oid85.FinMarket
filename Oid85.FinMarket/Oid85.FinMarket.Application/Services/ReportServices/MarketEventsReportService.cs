using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services.ReportServices;
using Oid85.FinMarket.Application.Models.Reports;
using Oid85.FinMarket.Common.KnownConstants;

namespace Oid85.FinMarket.Application.Services.ReportServices;

/// <inheritdoc />
public class MarketEventsReportService(
    IMarketEventRepository marketEventRepository) 
    : IMarketEventsReportService
{
    /// <inheritdoc />
    public async Task<ReportData> GetActiveMarketEventsAnalyseAsync()
    {
        var marketEvents = await marketEventRepository
            .GetActivatedAsync();
            
        var reportData = new ReportData
        {
            Title = "Активные рыночные события",
            Header =
            [
                new ReportParameter(KnownDisplayTypes.String, "Тикер"),
                new ReportParameter(KnownDisplayTypes.String, "SupertrendUp"),
                new ReportParameter(KnownDisplayTypes.String, "SupertrendDown"),
                new ReportParameter(KnownDisplayTypes.String, "CandleVolumeUp"),
                new ReportParameter(KnownDisplayTypes.String, "CandleSequenceWhite"),
                new ReportParameter(KnownDisplayTypes.String, "CandleSequenceBlack"),
                new ReportParameter(KnownDisplayTypes.String, "RsiOverBoughtInput"),
                new ReportParameter(KnownDisplayTypes.String, "RsiOverBoughtOutput"),
                new ReportParameter(KnownDisplayTypes.String, "RsiOverOverSoldInput"),
                new ReportParameter(KnownDisplayTypes.String, "RsiOverOverSoldOutput")
            ]
        };

        var tickers = marketEvents
            .Select(x => x.Ticker)
            .Distinct()
            .Order()
            .ToList();
        
        foreach (var ticker in tickers)
        {
            reportData.Data.Add(
            [
                new ReportParameter(KnownDisplayTypes.Ticker, ticker),
                new ReportParameter(KnownDisplayTypes.CheckBox, MarketEventIsActive(KnownMarketEventTypes.SupertrendUp)),
                new ReportParameter(KnownDisplayTypes.CheckBox, MarketEventIsActive(KnownMarketEventTypes.SupertrendDown)),
                new ReportParameter(KnownDisplayTypes.CheckBox, MarketEventIsActive(KnownMarketEventTypes.CandleVolumeUp)),
                new ReportParameter(KnownDisplayTypes.CheckBox, MarketEventIsActive(KnownMarketEventTypes.CandleSequenceWhite)),
                new ReportParameter(KnownDisplayTypes.CheckBox, MarketEventIsActive(KnownMarketEventTypes.CandleSequenceBlack)),
                new ReportParameter(KnownDisplayTypes.CheckBox, MarketEventIsActive(KnownMarketEventTypes.RsiOverBoughtInput)),
                new ReportParameter(KnownDisplayTypes.CheckBox, MarketEventIsActive(KnownMarketEventTypes.RsiOverBoughtOutput)),
                new ReportParameter(KnownDisplayTypes.CheckBox, MarketEventIsActive(KnownMarketEventTypes.RsiOverOverSoldInput)),
                new ReportParameter(KnownDisplayTypes.CheckBox, MarketEventIsActive(KnownMarketEventTypes.RsiOverOverSoldOutput))
            ]);
            
            continue;

            string MarketEventIsActive(string marketEventType) => 
                (marketEvents
                        .FirstOrDefault(x => 
                            x.Ticker == ticker && 
                            x.MarketEventType == marketEventType) 
                    is not null).ToString();
        }
            
        return reportData;
    }
}