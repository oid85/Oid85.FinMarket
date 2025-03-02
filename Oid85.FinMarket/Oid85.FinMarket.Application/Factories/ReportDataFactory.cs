using Microsoft.Extensions.Configuration;
using Oid85.FinMarket.Application.Helpers;
using Oid85.FinMarket.Application.Interfaces.Factories;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Application.Models.Reports;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Factories;

public class ReportDataFactory(
    IConfiguration configuration,
    IInstrumentRepository instrumentRepository,
    IAnalyseResultRepository analyseResultRepository,
    IDividendInfoRepository dividendInfoRepository,
    IMultiplicatorRepository multiplicatorRepository,
    IForecastTargetRepository forecastTargetRepository,
    IForecastConsensusRepository forecastConsensusRepository,
    ReportHelper reportHelper,
    IInstrumentService instrumentService) 
    : IReportDataFactory
{
    private async Task<List<AnalyseResult>> GetAnalyseResults(
        List<Guid> instrumentIds, List<string> analyseTypes, DateOnly from, DateOnly to) =>
        (await analyseResultRepository.GetAsync(instrumentIds, from, to))
        .Where(x => analyseTypes.Contains(x.AnalyseType)).ToList();
    
    public async Task<ReportData> CreateReportDataAsync(
        List<Guid> instrumentIds, string analyseType, DateOnly from, DateOnly to)
    {
        var analyseResults = await GetAnalyseResults(
            instrumentIds, [analyseType], from, to);
        
        var reportData = new ReportData
        {
            Title = $"{from.ToString(KnownDateTimeFormats.DateISO)} - {to.ToString(KnownDateTimeFormats.DateISO)}",
            Header = 
            [
                new ReportParameter(KnownDisplayTypes.String, "Тикер"),
                new ReportParameter(KnownDisplayTypes.String, "Сектор"),
                new ReportParameter(KnownDisplayTypes.String, "Наименование")
            ]
        };
        
        var dates = reportHelper.GetDates(from, to);
        reportData.Header.AddRange(dates);

        foreach (var instrumentId in instrumentIds)
        {
            var instrument = await instrumentRepository.GetByInstrumentIdAsync(instrumentId);
            
            if (instrument is null)
                continue;
            
            var data = new List<ReportParameter>
            {
                new (KnownDisplayTypes.Ticker, instrument.Ticker), 
                new (KnownDisplayTypes.Sector, instrument.Sector),
                new (KnownDisplayTypes.String, instrument.Name)
            };

            foreach (var date in dates)
            {
                var analyseResult = analyseResults
                    .FirstOrDefault(x =>
                        x.InstrumentId == instrument.InstrumentId &&
                        x.Date.ToString(KnownDateTimeFormats.DateISO) == date.Value);

                data.Add(analyseResult is not null 
                    ? new ReportParameter(KnownDisplayTypes.String, analyseResult.ResultString,
                        await reportHelper.GetColorByAnalyseType(analyseType, analyseResult)) 
                    : new ReportParameter(KnownDisplayTypes.String, string.Empty));
            }
                
            reportData.Data.Add(data);
        }
            
        return reportData;
    }

    public async Task<ReportData> CreateAggregatedReportDataAsync(
        List<Guid> instrumentIds, List<string> analyseTypes, DateOnly from, DateOnly to)
    {
        var analyseResults = await GetAnalyseResults(
            instrumentIds, analyseTypes, from, to);
            
        var reportData = new ReportData
        {
            Title = $"{from.ToString(KnownDateTimeFormats.DateISO)} - {to.ToString(KnownDateTimeFormats.DateISO)}",
            Header = 
            [
                new ReportParameter(KnownDisplayTypes.String, "Тикер"),
                new ReportParameter(KnownDisplayTypes.String, "Сектор"),
                new ReportParameter(KnownDisplayTypes.String, "Наименование")
            ]
        };

        var dates = reportHelper.GetDates(from, to);
        reportData.Header.AddRange(dates);
        
        foreach (var instrumentId in instrumentIds)
        {
            var instrument = await instrumentRepository.GetByInstrumentIdAsync(instrumentId);
            
            if (instrument is null)
                continue;
            
            var data = new List<ReportParameter>
            {
                new (KnownDisplayTypes.Ticker, instrument.Ticker), 
                new (KnownDisplayTypes.Sector, instrument.Sector),
                new (KnownDisplayTypes.String, instrument.Name)
            };

            foreach (var date in dates)
            {
                double resultNumber = analyseResults
                    .Where(x => 
                        x.InstrumentId == instrument.InstrumentId && 
                        analyseTypes.Contains(x.AnalyseType) &&
                        x.Date.ToString(KnownDateTimeFormats.DateISO) == date.Value)
                    .Select(x => x.ResultNumber)
                    .Sum();
                    
                data.Add(new ReportParameter($"AnalyseResult{KnownAnalyseTypes.Aggregated}", resultNumber.ToString("N0"),
                    await reportHelper.GetColorAggregated((int) resultNumber)));
            }
                
            reportData.Data.Add(data);
        }
            
        return reportData;
    }

    public async Task<ReportData> CreateDividendInfoReportDataAsync()
    {
        var dividendInfos = await dividendInfoRepository.GetAllAsync();
        int days = configuration.GetValue<int>(KnownSettingsKeys.ApplicationSettingsOutputWindowInDays);
        var from = DateOnly.FromDateTime(DateTime.Today);
        var to = from.AddDays(days);
        var dates = reportHelper.GetDates(from, to);
        
        var reportData = new ReportData
        {
            Title = "Информация по дивидендам",
            Header =
            [
                new ReportParameter(KnownDisplayTypes.String, "Тикер"),
                new ReportParameter(KnownDisplayTypes.String, "Фикс. р."),
                new ReportParameter(KnownDisplayTypes.String, "Объяв."),
                new ReportParameter(KnownDisplayTypes.String, "Размер, руб"),
                new ReportParameter(KnownDisplayTypes.String, "Дох-ть, %")
            ]
        };
        
        reportData.Header.AddRange(dates);
        
        foreach (var dividendInfo in dividendInfos)
        {
            var data = new List<ReportParameter>
            {
                new(KnownDisplayTypes.Ticker, dividendInfo.Ticker),
                new(KnownDisplayTypes.Date, dividendInfo.RecordDate.ToString(KnownDateTimeFormats.DateISO)),
                new(KnownDisplayTypes.Date, dividendInfo.DeclaredDate.ToString(KnownDateTimeFormats.DateISO)),
                new(KnownDisplayTypes.Ruble, dividendInfo.Dividend.ToString("N1")),
                new(KnownDisplayTypes.Percent, dividendInfo.DividendPrc.ToString("N1"), 
                    await reportHelper.GetColorYieldDividend(dividendInfo.DividendPrc))
            };
            
            foreach (var date in dates)
                data.Add(date.Value == dividendInfo.RecordDate.ToString(KnownDateTimeFormats.DateISO)
                    ? new ReportParameter(KnownDisplayTypes.Percent, dividendInfo.DividendPrc.ToString("N1"), 
                        await reportHelper.GetColorYieldDividend(dividendInfo.DividendPrc))
                    : new ReportParameter(KnownDisplayTypes.Percent, string.Empty));

            reportData.Data.Add(data);
        }
        
        return reportData;
    }

    public async Task<ReportData> CreateMultiplicatorReportDataAsync()
    {
        var shares = (await instrumentService.GetSharesInWatchlist())
            .OrderBy(x => x.Sector);
            
        var reportData = new ReportData
        {
            Title = "Мультипликаторы",
            Header =
            [
                new ReportParameter(KnownDisplayTypes.String, "Тикер"),
                new ReportParameter(KnownDisplayTypes.String, "Сектор"),
                new ReportParameter(KnownDisplayTypes.String, "Рыночная капитализация"),
                new ReportParameter(KnownDisplayTypes.String, "Бета-коэффициент"),
                new ReportParameter(KnownDisplayTypes.String, "Чистая прибыль"),
                new ReportParameter(KnownDisplayTypes.String, "EBITDA"),
                new ReportParameter(KnownDisplayTypes.String, "EPS"),
                new ReportParameter(KnownDisplayTypes.String, "Свободный денежный поток"),
                new ReportParameter(KnownDisplayTypes.String, "EV/EBITDA"),
                new ReportParameter(KnownDisplayTypes.String, "Total Debt/EBITDA"),
                new ReportParameter(KnownDisplayTypes.String, "Net Debt/EBITDA")
            ]
        };
        
        foreach (var share in shares)
        {
            var multiplicator = await multiplicatorRepository.GetAsync(share.Ticker);
            
            if (multiplicator is null)
                continue;
            
            List<ReportParameter> data =
            [
                new (KnownDisplayTypes.Ticker, share.Ticker),
                new (KnownDisplayTypes.Sector, share.Sector),
                new (KnownDisplayTypes.Number, multiplicator.MarketCapitalization.ToString("N2")),
                new (KnownDisplayTypes.Number, multiplicator.Beta.ToString("N2")),
                new (KnownDisplayTypes.Number, multiplicator.NetIncome.ToString("N2")),
                new (KnownDisplayTypes.Number, multiplicator.Ebitda.ToString("N2")),
                new (KnownDisplayTypes.Number, multiplicator.Eps.ToString("N2")),
                new (KnownDisplayTypes.Number, multiplicator.FreeCashFlow.ToString("N2")),
                new (KnownDisplayTypes.Number, multiplicator.EvToEbitda.ToString("N2"), 
                    await reportHelper.GetColorEvToEbitda(multiplicator.EvToEbitda)),
                new (KnownDisplayTypes.Number, multiplicator.TotalDebtToEbitda.ToString("N2")),
                new (KnownDisplayTypes.Number, multiplicator.NetDebtToEbitda.ToString("N2"), 
                    await reportHelper.GetColorNetDebtToEbitda(multiplicator.NetDebtToEbitda))
            ];
            
            reportData.Data.Add(data);
        }
        
        return reportData;
    }

    public async Task<ReportData> CreateForecastTargetReportDataAsync()
    {
        var forecastTargets = await forecastTargetRepository.GetAllAsync();
        var actualForecastTargets = new List<ForecastTarget>();

        foreach (var forecastTarget in forecastTargets)
        {
            var target = forecastTargets
                .Where(x =>
                    x.InstrumentId == forecastTarget.InstrumentId &&
                    x.Company == forecastTarget.Company)
                .MaxBy(x => x.RecommendationDate);

            if (target is not null)
            {
                var addedTarget = actualForecastTargets
                    .FirstOrDefault(x =>
                        x.InstrumentId == target.InstrumentId &&
                        x.Company == target.Company &&
                        x.RecommendationDate == target.RecommendationDate);

                if (addedTarget is null) 
                    actualForecastTargets.Add(target);
            }
        }
        
        var reportData = new ReportData
        {
            Title = "Прогнозы",
            Header =
            [
                new ReportParameter(KnownDisplayTypes.String, "Тикер"),
                new ReportParameter(KnownDisplayTypes.String, "Прогноз от компании"),
                new ReportParameter(KnownDisplayTypes.String, "Прогноз"),
                new ReportParameter(KnownDisplayTypes.String, "Дата прогноза"),
                new ReportParameter(KnownDisplayTypes.String, "Валюта"),
                new ReportParameter(KnownDisplayTypes.String, "Текущая цена"),
                new ReportParameter(KnownDisplayTypes.String, "Прогнозируемая цена"),
                new ReportParameter(KnownDisplayTypes.String, "Изменение цены"),
                new ReportParameter(KnownDisplayTypes.String, "Относительное изменение цены"),
                new ReportParameter(KnownDisplayTypes.String, "Наименование инструмента")
            ]
        };

        foreach (var forecastTarget in actualForecastTargets)
        {
            List<ReportParameter> data =
            [
                new (KnownDisplayTypes.Ticker, forecastTarget.Ticker),
                new (KnownDisplayTypes.String, forecastTarget.Company),
                new (KnownDisplayTypes.String, forecastTarget.RecommendationString, 
                    await reportHelper.GetColorForecastRecommendation(forecastTarget.RecommendationString)),
                new (KnownDisplayTypes.Date, forecastTarget.RecommendationDate.ToString(KnownDateTimeFormats.DateISO)),
                new (KnownDisplayTypes.String, forecastTarget.Currency),
                new (KnownDisplayTypes.Ruble, forecastTarget.CurrentPrice.ToString("N2")),
                new (KnownDisplayTypes.Ruble, forecastTarget.TargetPrice.ToString("N2")),
                new (KnownDisplayTypes.Ruble, forecastTarget.PriceChange.ToString("N2")),
                new (KnownDisplayTypes.Percent, forecastTarget.PriceChangeRel.ToString("N2")),
                new (KnownDisplayTypes.String, forecastTarget.ShowName)
            ];
            
            reportData.Data.Add(data);
        }
        
        return reportData;
    }

    public async Task<ReportData> CreateForecastConsensusReportDataAsync()
    {
        var forecastConsensuses = await forecastConsensusRepository.GetAllAsync();
        
        var reportData = new ReportData
        {
            Title = "Консенсус-прогнозы",
            Header =
            [
                new ReportParameter(KnownDisplayTypes.String, "Тикер"),
                new ReportParameter(KnownDisplayTypes.String, "Прогноз"),
                new ReportParameter(KnownDisplayTypes.String, "Валюта"),
                new ReportParameter(KnownDisplayTypes.String, "Текущая цена"),
                new ReportParameter(KnownDisplayTypes.String, "Прогнозируемая цена"),
                new ReportParameter(KnownDisplayTypes.String, "Минимальная цена прогноза"),
                new ReportParameter(KnownDisplayTypes.String, "Максимальная цена прогноза"),
                new ReportParameter(KnownDisplayTypes.String, "Изменение цены"),
                new ReportParameter(KnownDisplayTypes.String, "Относительное изменение цены")
            ]
        };

        foreach (var forecastConsensus in forecastConsensuses)
        {
            List<ReportParameter> data =
            [
                new (KnownDisplayTypes.Ticker, forecastConsensus.Ticker),
                new (KnownDisplayTypes.String, forecastConsensus.RecommendationString, 
                    await reportHelper.GetColorForecastRecommendation(forecastConsensus.RecommendationString)),
                new (KnownDisplayTypes.String, forecastConsensus.Currency),
                new (KnownDisplayTypes.Ruble, forecastConsensus.CurrentPrice.ToString("N2")),
                new (KnownDisplayTypes.Ruble, forecastConsensus.ConsensusPrice.ToString("N2")),
                new (KnownDisplayTypes.Ruble, forecastConsensus.MinTarget.ToString("N2")),
                new (KnownDisplayTypes.Ruble, forecastConsensus.MaxTarget.ToString("N2")),
                new (KnownDisplayTypes.Ruble, forecastConsensus.PriceChange.ToString("N2")),
                new (KnownDisplayTypes.Percent, forecastConsensus.PriceChangeRel.ToString("N2"))
            ];
            
            reportData.Data.Add(data);
        }
        
        return reportData;
    }
}