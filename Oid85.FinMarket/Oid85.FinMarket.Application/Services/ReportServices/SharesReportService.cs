using Microsoft.Extensions.Configuration;
using Oid85.FinMarket.Application.Helpers;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services.ReportServices;
using Oid85.FinMarket.Application.Models.Reports;
using Oid85.FinMarket.Application.Models.Requests;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Services.ReportServices;

/// <inheritdoc />
public class SharesReportService(
    IConfiguration configuration,
    IAnalyseResultRepository analyseResultRepository,
    IShareRepository shareRepository,
    IDividendInfoRepository dividendInfoRepository,
    IAssetFundamentalRepository assetFundamentalRepository,
    IMultiplicatorRepository multiplicatorRepository,
    ReportHelper reportHelper)
    : ISharesReportService
{
    /// <inheritdoc />
    public async Task<ReportData> GetAggregatedAnalyseAsync(GetAnalyseRequest request) =>
        await GetReportDataAggregatedAnalyse(
            await shareRepository.GetWatchListAsync(), 
            request.From, 
            request.To);

    /// <inheritdoc />
    public async Task<ReportData> GetSupertrendAnalyseAsync(GetAnalyseRequest request) =>
        await GetReportDataByAnalyseType(
            await shareRepository.GetWatchListAsync(), 
            request.From, 
            request.To, 
            KnownAnalyseTypes.Supertrend);

    /// <inheritdoc />
    public async Task<ReportData> GetCandleSequenceAnalyseAsync(GetAnalyseRequest request) =>
        await GetReportDataByAnalyseType(
            await shareRepository.GetWatchListAsync(), 
            request.From, 
            request.To, 
            KnownAnalyseTypes.CandleSequence);

    /// <inheritdoc />
    public async Task<ReportData> GetCandleVolumeAnalyseAsync(GetAnalyseRequest request) =>
        await GetReportDataByAnalyseType(
            await shareRepository.GetWatchListAsync(), 
            request.From, 
            request.To, 
            KnownAnalyseTypes.CandleVolume);

    /// <inheritdoc />
    public async Task<ReportData> GetRsiAnalyseAsync(GetAnalyseRequest request) =>
        await GetReportDataByAnalyseType(
            await shareRepository.GetWatchListAsync(), 
            request.From, 
            request.To, 
            KnownAnalyseTypes.Rsi);

    /// <inheritdoc />
    public async Task<ReportData> GetYieldLtmAnalyseAsync(GetAnalyseRequest request) =>
        await GetReportDataYieldLtmAnalyse(
            await shareRepository.GetWatchListAsync(), 
            request.From, 
            request.To);

    /// <inheritdoc />
    public async Task<ReportData> GetDividendAnalyseAsync()
    {
        var dividendInfos = await dividendInfoRepository
            .GetAllAsync();
            
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
            
        foreach (var dividendInfo in dividendInfos)
        {
            reportData.Data.Add(
            [
                new ReportParameter(
                    KnownDisplayTypes.Ticker, 
                    dividendInfo.Ticker),
                    
                new ReportParameter(
                    KnownDisplayTypes.Date, 
                    dividendInfo.RecordDate.ToString(KnownDateTimeFormats.DateISO)),
                    
                new ReportParameter(
                    KnownDisplayTypes.Date, 
                    dividendInfo.DeclaredDate.ToString(KnownDateTimeFormats.DateISO)),
                    
                new ReportParameter(
                    KnownDisplayTypes.Ruble, 
                    dividendInfo.Dividend.ToString("N1")), 
                    
                new ReportParameter(
                    KnownDisplayTypes.Percent, 
                    dividendInfo.DividendPrc.ToString("N1"))
            ]);
        }
            
        return reportData;
    }

    /// <inheritdoc />
    public async Task<ReportData> GetAssetFundamentalAnalyseAsync()
    {
        var shares = await shareRepository
            .GetWatchListAsync();
            
        var reportData = new ReportData
        {
            Title = "Фундаментальный анализ",
            Header =
            [
                new ReportParameter(KnownDisplayTypes.String, "Тикер"),
                new ReportParameter(KnownDisplayTypes.String, "Сектор"),
                new ReportParameter(KnownDisplayTypes.String, "Рыночная капитализация"),
                new ReportParameter(KnownDisplayTypes.String, "Минимум за год"),
                new ReportParameter(KnownDisplayTypes.String, "Максимум за год"),
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
            var assetFundamental = await assetFundamentalRepository.GetLastAsync(share.InstrumentId);
            
            if (assetFundamental is null)
                continue;
            
            List<ReportParameter> data =
            [
                new (KnownDisplayTypes.Ticker, share.Ticker),
                new (KnownDisplayTypes.Sector, share.Sector),
                new (KnownDisplayTypes.Number, assetFundamental.MarketCapitalization.ToString("N2")),
                new (KnownDisplayTypes.Number, assetFundamental.LowPriceLast52Weeks.ToString("N2")),
                new (KnownDisplayTypes.Number, assetFundamental.HighPriceLast52Weeks.ToString("N2")),
                new (KnownDisplayTypes.Number, assetFundamental.Beta.ToString("N2")),
                new (KnownDisplayTypes.Number, assetFundamental.NetIncomeTtm.ToString("N2")),
                new (KnownDisplayTypes.Number, assetFundamental.EbitdaTtm.ToString("N2")),
                new (KnownDisplayTypes.Number, assetFundamental.EpsTtm.ToString("N2")),
                new (KnownDisplayTypes.Number, assetFundamental.FreeCashFlowTtm.ToString("N2")),
                new (KnownDisplayTypes.Number, assetFundamental.EvToEbitdaMrq.ToString("N2")),
                new (KnownDisplayTypes.Number, assetFundamental.TotalDebtToEbitdaMrq.ToString("N2")),
                new (KnownDisplayTypes.Number, assetFundamental.NetDebtToEbitda.ToString("N2"))
            ];
            
            reportData.Data.Add(data);
        }
        
        return reportData;
    }

    /// <inheritdoc />
    public async Task<ReportData> GetMultiplicatorAnalyseAsync()
    {
        var shares = await shareRepository
            .GetWatchListAsync();
            
        var reportData = new ReportData
        {
            Title = "Мультипликаторы",
            Header =
            [
                new ReportParameter(KnownDisplayTypes.String, "Тикер"),
                new ReportParameter(KnownDisplayTypes.String, "Сектор"),
                new ReportParameter(KnownDisplayTypes.String, "Рыночная капитализация"),
                new ReportParameter(KnownDisplayTypes.String, "Минимум за год"),
                new ReportParameter(KnownDisplayTypes.String, "Максимум за год"),
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
            var multiplicator = await multiplicatorRepository.GetAsync(share.InstrumentId);
            
            if (multiplicator is null)
                continue;
            
            List<ReportParameter> data =
            [
                new (KnownDisplayTypes.Ticker, share.Ticker),
                new (KnownDisplayTypes.Sector, share.Sector),
                new (KnownDisplayTypes.Number, multiplicator.MarketCapitalization.ToString("N2")),
                new (KnownDisplayTypes.Number, multiplicator.LowOfYear.ToString("N2")),
                new (KnownDisplayTypes.Number, multiplicator.HighOfYear.ToString("N2")),
                new (KnownDisplayTypes.Number, multiplicator.Beta.ToString("N2")),
                new (KnownDisplayTypes.Number, multiplicator.NetIncome.ToString("N2")),
                new (KnownDisplayTypes.Number, multiplicator.Ebitda.ToString("N2")),
                new (KnownDisplayTypes.Number, multiplicator.Eps.ToString("N2")),
                new (KnownDisplayTypes.Number, multiplicator.FreeCashFlow.ToString("N2")),
                new (KnownDisplayTypes.Number, multiplicator.EvToEbitda.ToString("N2")),
                new (KnownDisplayTypes.Number, multiplicator.TotalDebtToEbitda.ToString("N2")),
                new (KnownDisplayTypes.Number, multiplicator.NetDebtToEbitda.ToString("N2"))
            ];
            
            reportData.Data.Add(data);
        }
        
        return reportData;
    }

    private async Task<ReportData> GetReportDataByAnalyseType(
        List<Share> instruments, 
        DateOnly from, 
        DateOnly to, 
        string analyseType)
    {
        var instrumentIds = instruments
            .Select(x => x.InstrumentId)
            .ToList();        
        
        var analyseResults = (await analyseResultRepository
                .GetAsync(instrumentIds, from, to))
            .Where(x => x.AnalyseType == analyseType)
            .ToList();

        int outputWindowInDays = configuration
            .GetValue<int>(KnownSettingsKeys.ApplicationSettingsOutputWindowInDays);
        
        var dividendInfos = await dividendInfoRepository
            .GetAsync(instrumentIds, to.AddDays(1), to.AddDays(outputWindowInDays));
            
        var dates = reportHelper.GetDates(from, to.AddDays(outputWindowInDays));
            
        var reportData = new ReportData
        {
            Title = $"Анализ {analyseType} " +
                    $"с {from.ToString(KnownDateTimeFormats.DateISO)} " +
                    $"по {to.ToString(KnownDateTimeFormats.DateISO)}",
                
            Header = 
            [
                new ReportParameter(KnownDisplayTypes.String, "Тикер"),
                new ReportParameter(KnownDisplayTypes.String, "Сектор")
            ]
        };

        reportData.Header.AddRange(dates);

        foreach (var instrument in instruments)
        {
            var data = new List<ReportParameter>
            {
                new (KnownDisplayTypes.Ticker, instrument.Ticker), 
                new (KnownDisplayTypes.Sector, instrument.Sector)
            };

            foreach (var date in dates)
            {
                var currentDate = DateOnly.FromDateTime(Convert.ToDateTime(date.Value));
                
                if (currentDate <= to)
                {
                    var analyseResult = analyseResults
                        .FirstOrDefault(x =>
                            x.InstrumentId == instrument.InstrumentId &&
                            x.Date.ToString(KnownDateTimeFormats.DateISO) == date.Value);

                    data.Add(analyseResult is not null
                        ? new ReportParameter(
                            $"AnalyseResult{analyseType}",
                            analyseResult.ResultString)
                        : new ReportParameter(
                            $"AnalyseResult{analyseType}",
                            string.Empty));
                }

                else
                {
                    var dividendInfo = dividendInfos
                        .FirstOrDefault(x => 
                            x.Ticker == instrument.Ticker && 
                            x.RecordDate.ToString(KnownDateTimeFormats.DateISO) == date.Value);

                    data.Add(dividendInfo is not null 
                        ? new ReportParameter(
                            KnownDisplayTypes.Percent, 
                            dividendInfo.DividendPrc.ToString("N1")) 
                        : new ReportParameter(
                            KnownDisplayTypes.Percent, 
                            string.Empty));
                }
            }
                
            reportData.Data.Add(data);
        }
            
        return reportData;
    }
    
    private async Task<ReportData> GetReportDataAggregatedAnalyse(
        List<Share> instruments, 
        DateOnly from, 
        DateOnly to)
    {
        var instrumentIds = instruments
            .Select(x => x.InstrumentId)
            .ToList();

        var analyseTypes = new List<string>()
        {
            KnownAnalyseTypes.Supertrend,
            KnownAnalyseTypes.CandleSequence,
            KnownAnalyseTypes.CandleVolume,
            KnownAnalyseTypes.Rsi
        };
        
        var analyseResults = (await analyseResultRepository
                .GetAsync(instrumentIds, from, to))
            .Where(x => analyseTypes.Contains(x.AnalyseType))
            .ToList();
        
        int outputWindowInDays = configuration
            .GetValue<int>(KnownSettingsKeys.ApplicationSettingsOutputWindowInDays);
        
        var dividendInfos = await dividendInfoRepository
            .GetAsync(instrumentIds, to.AddDays(1), to.AddDays(outputWindowInDays));
            
        var dates = reportHelper.GetDates(from, to.AddDays(outputWindowInDays));
            
        var reportData = new ReportData
        {
            Title = $"Анализ Aggregated " +
                    $"с {from.ToString(KnownDateTimeFormats.DateISO)} " +
                    $"по {to.ToString(KnownDateTimeFormats.DateISO)}",
                
            Header = 
            [
                new ReportParameter(KnownDisplayTypes.String, "Тикер"),
                new ReportParameter(KnownDisplayTypes.String, "Сектор")
            ]
        };

        reportData.Header.AddRange(dates);

        foreach (var instrument in instruments)
        {
            var data = new List<ReportParameter>
            {
                new (KnownDisplayTypes.Ticker, instrument.Ticker), 
                new (KnownDisplayTypes.Sector, instrument.Sector)
            };

            foreach (var date in dates)
            {
                var currentDate = DateOnly.FromDateTime(Convert.ToDateTime(date.Value));
                
                if (currentDate <= to)
                {
                    double resultNumber = analyseResults
                        .Where(x => 
                            x.InstrumentId == instrument.InstrumentId && 
                            analyseTypes.Contains(x.AnalyseType) &&
                            x.Date.ToString(KnownDateTimeFormats.DateISO) == date.Value)
                        .Select(x => x.ResultNumber)
                        .Sum();
                    
                    data.Add(new ReportParameter(
                        $"AnalyseResult{KnownAnalyseTypes.Aggregated}",
                        resultNumber.ToString("N2")));
                }

                else
                {
                    var dividendInfo = dividendInfos
                        .FirstOrDefault(x => 
                            x.Ticker == instrument.Ticker && 
                            x.RecordDate.ToString(KnownDateTimeFormats.DateISO) == date.Value);

                    data.Add(dividendInfo is not null 
                        ? new ReportParameter(
                            KnownDisplayTypes.Percent, 
                            dividendInfo.DividendPrc.ToString("N1")) 
                        : new ReportParameter(
                            KnownDisplayTypes.Percent, 
                            string.Empty));
                }
            }
                
            reportData.Data.Add(data);
        }
            
        return reportData;
    }

    private async Task<ReportData> GetReportDataYieldLtmAnalyse(
        List<Share> instruments, 
        DateOnly from, 
        DateOnly to)
    {
        var instrumentIds = instruments
            .Select(x => x.InstrumentId)
            .ToList();        
        
        var analyseResults = (await analyseResultRepository
                .GetAsync(instrumentIds, from, to))
            .Where(x => x.AnalyseType == KnownAnalyseTypes.YieldLtm)
            .ToList();

        int outputWindowInDays = configuration
            .GetValue<int>(KnownSettingsKeys.ApplicationSettingsOutputWindowInDays);
        
        var dividendInfos = await dividendInfoRepository
            .GetAsync(instrumentIds, to.AddDays(1), to.AddDays(outputWindowInDays));
            
        var dates = reportHelper.GetDates(from, to.AddDays(outputWindowInDays));
            
        var reportData = new ReportData
        {
            Title = $"Анализ {KnownAnalyseTypes.YieldLtm} " +
                    $"с {from.ToString(KnownDateTimeFormats.DateISO)} " +
                    $"по {to.ToString(KnownDateTimeFormats.DateISO)}",
                
            Header = 
            [
                new ReportParameter(KnownDisplayTypes.String, "Тикер"),
                new ReportParameter(KnownDisplayTypes.String, "Сектор")
            ]
        };

        reportData.Header.AddRange(dates);

        foreach (var instrument in instruments)
        {
            var data = new List<ReportParameter>
            {
                new (KnownDisplayTypes.Ticker, instrument.Ticker), 
                new (KnownDisplayTypes.Sector, instrument.Sector)
            };

            foreach (var date in dates)
            {
                var currentDate = DateOnly.FromDateTime(Convert.ToDateTime(date.Value));
                
                if (currentDate <= to)
                {
                    var analyseResult = analyseResults
                        .FirstOrDefault(x =>
                            x.InstrumentId == instrument.InstrumentId &&
                            x.Date.ToString(KnownDateTimeFormats.DateISO) == date.Value);

                    data.Add(analyseResult is not null
                        ? new ReportParameter(
                            $"AnalyseResult{KnownAnalyseTypes.YieldLtm}",
                            analyseResult.ResultString)
                        : new ReportParameter(
                            $"AnalyseResult{KnownAnalyseTypes.YieldLtm}",
                            string.Empty));
                }

                else
                {
                    var dividendInfo = dividendInfos
                        .FirstOrDefault(x => 
                            x.Ticker == instrument.Ticker && 
                            x.RecordDate.ToString(KnownDateTimeFormats.DateISO) == date.Value);

                    data.Add(dividendInfo is not null 
                        ? new ReportParameter(
                            KnownDisplayTypes.Percent, 
                            dividendInfo.DividendPrc.ToString("N1")) 
                        : new ReportParameter(
                            KnownDisplayTypes.Percent, 
                            string.Empty));
                }
            }
                
            reportData.Data.Add(data);
        }
            
        return reportData;
    }
}