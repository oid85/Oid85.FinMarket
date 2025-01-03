﻿using System.Globalization;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Application.Models.Reports;
using Oid85.FinMarket.Application.Models.Requests;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Services;

/// <inheritdoc />
public class ReportService(
    IAnalyseResultRepository analyseResultRepository,
    IBondCouponRepository bondCouponRepository,
    IBondRepository bondRepository,
    ICurrencyRepository currencyRepository,
    IDividendInfoRepository dividendInfoRepository,
    IIndexRepository indexRepository,
    IShareRepository shareRepository,
    IFutureRepository futureRepository,
    IAssetFundamentalRepository assetFundamentalRepository,
    ISpreadRepository spreadRepository)
    : IReportService
{
    private const int WindowInDays = 180;
        
    #region Shares
    
    /// <inheritdoc />
    public async Task<ReportData> GetReportShareTotalAnalyseAsync(
        GetReportAnalyseByTickerRequest request)
    {
        var share = await shareRepository.GetByTickerAsync(request.Ticker);
            
        if (share is null)
            return new ();
            
        var reportData = new ReportData();

        var dates = GetDates(request.From, request.To.AddDays(WindowInDays));

        reportData.Header =
        [
            new ReportParameter(KnownDisplayTypes.String, "Тикер"),
            new ReportParameter(KnownDisplayTypes.String, "Сектор")
        ];
            
        reportData.Header.AddRange(dates);

        reportData.Data = 
        [
            (await GetReportDataSharesByAnalyseType(
                [share], request.From, request.To, KnownAnalyseTypes.Supertrend))
            .Data.First(),
                
            (await GetReportDataSharesByAnalyseType(
                [share], request.From, request.To, KnownAnalyseTypes.CandleSequence))
            .Data.First(),
                
            (await GetReportDataSharesByAnalyseType(
                [share], request.From, request.To, KnownAnalyseTypes.CandleVolume))
            .Data.First(),
                
            (await GetReportDataSharesByAnalyseType(
                [share], request.From, request.To, KnownAnalyseTypes.Rsi))
            .Data.First(),
        ];
            
        reportData.Title =
            $"Анализ {request.Ticker} " +
            $"с {request.From.ToString(KnownDateTimeFormats.DateISO)} " +
            $"по {request.To.ToString(KnownDateTimeFormats.DateISO)}";
            
        return reportData;
    }

    /// <inheritdoc />
    public async Task<ReportData> GetReportSharesAnalyseSupertrendAsync(
        GetReportAnalyseRequest request) =>
        await GetReportDataSharesByAnalyseType(
            await shareRepository.GetWatchListAsync(),
            request.From,
            request.To,
            KnownAnalyseTypes.Supertrend);

    /// <inheritdoc />
    public async Task<ReportData> GetReportSharesAnalyseCandleSequenceAsync(
        GetReportAnalyseRequest request) =>
        await GetReportDataSharesByAnalyseType(
            await shareRepository.GetWatchListAsync(),
            request.From,
            request.To, 
            KnownAnalyseTypes.CandleSequence);

    /// <inheritdoc />
    public async Task<ReportData> GetReportSharesAnalyseCandleVolumeAsync(
        GetReportAnalyseRequest request) =>
        await GetReportDataSharesByAnalyseType(
            await shareRepository.GetWatchListAsync(),
            request.From,
            request.To,
            KnownAnalyseTypes.CandleVolume);

    /// <inheritdoc />
    public async Task<ReportData> GetReportSharesAnalyseRsiAsync(
        GetReportAnalyseRequest request) =>
        await GetReportDataSharesByAnalyseType(
            await shareRepository.GetWatchListAsync(),
            request.From,
            request.To,
            KnownAnalyseTypes.Rsi);

    #endregion

    #region Dividends
    
    /// <inheritdoc />
    public async Task<ReportData> GetReportDividendsAsync()
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
                    dividendInfo.Dividend.ToString(CultureInfo.InvariantCulture)), 
                    
                new ReportParameter(
                    KnownDisplayTypes.Percent, 
                    dividendInfo.DividendPrc.ToString(CultureInfo.InvariantCulture))
            ]);
        }
            
        return reportData;
    }
        
    #endregion
    
    #region Bonds
    
    /// <inheritdoc />
    public async Task<ReportData> GetReportBondsAsync()
    {
        var bonds = await bondRepository
            .GetWatchListAsync();
            
        var reportData = new ReportData
        {
            Title = "Информация по облигациям",
            Header =
            [
                new ReportParameter(KnownDisplayTypes.String, "Тикер"),
                new ReportParameter(KnownDisplayTypes.String, "Сектор"),
                new ReportParameter(KnownDisplayTypes.String, "Плав. купон"),
                new ReportParameter(KnownDisplayTypes.String, "До погаш., дней"),
                new ReportParameter(KnownDisplayTypes.String, "Дох-ть., %")
            ]
        };
            
        var bondCoupons = await bondCouponRepository
            .GetAsync(
                DateOnly.FromDateTime(DateTime.Today), 
                DateOnly.FromDateTime(DateTime.Today).AddDays(WindowInDays));
            
        var dates = GetDates(
            DateOnly.FromDateTime(DateTime.Today), 
            DateOnly.FromDateTime(DateTime.Today).AddDays(WindowInDays));
            
        reportData.Header.AddRange(dates);
            
        foreach (var bond in bonds)
        {
            List<ReportParameter> data =
            [
                new (KnownDisplayTypes.Ticker, 
                    bond.Ticker),
                    
                new (KnownDisplayTypes.Sector, bond.Sector),
                    
                new (KnownDisplayTypes.String, 
                    bond.FloatingCouponFlag ? "Да" : string.Empty),
                    
                new (KnownDisplayTypes.Number, 
                    (bond.MaturityDate.ToDateTime(TimeOnly.MinValue) - DateTime.Today).Days.ToString())
            ];
                
            // Вычисляем полную доходность облигации
            var nextCoupon = bondCoupons
                .FirstOrDefault(x => x.Ticker == bond.Ticker);
                
            double profitPrc = 0.0;
                
            if (nextCoupon is not null)
                profitPrc = (bond.LastPrice / (365.0 / nextCoupon.CouponPeriod) * nextCoupon.PayOneBond) / 100.0;
                
            data.Add(new ReportParameter(
                KnownDisplayTypes.Percent, 
                profitPrc.ToString(CultureInfo.InvariantCulture)));
                
            foreach (var date in dates)
            {
                var bondCoupon = bondCoupons
                    .FirstOrDefault(x => 
                        x.Ticker == bond.Ticker &&
                        x.CouponDate.ToString(KnownDateTimeFormats.DateISO) == date.Value);

                data.Add(bondCoupon is not null 
                    ? new ReportParameter(
                        KnownDisplayTypes.Ruble, 
                        bondCoupon.PayOneBond.ToString(CultureInfo.InvariantCulture)) 
                    : new ReportParameter(
                        KnownDisplayTypes.Ruble, 
                        string.Empty));
            }
                
            reportData.Data.Add(data);
        }
            
        return reportData;
    }

    /// <inheritdoc />
    public async Task<ReportData> GetReportBondsAnalyseSupertrendAsync(
        GetReportAnalyseRequest request) =>
        await GetReportDataBondsByAnalyseType(
            await bondRepository.GetWatchListAsync(),
            request.From,
            request.To,
            KnownAnalyseTypes.Supertrend);

    /// <inheritdoc />
    public async Task<ReportData> GetReportBondsAnalyseCandleSequenceAsync(
        GetReportAnalyseRequest request) =>
        await GetReportDataBondsByAnalyseType(
            await bondRepository.GetWatchListAsync(),
            request.From,
            request.To,
            KnownAnalyseTypes.CandleSequence);

    #endregion
    
    #region AssetFundamentals
    
    /// <inheritdoc />
    public async Task<ReportData> GetReportAssetFundamentalsAsync()
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

    #endregion
    
    #region Spreads
    
    /// <inheritdoc />
    public async Task<ReportData> GetReportSpreadsAsync()
    {
        var spreads = await spreadRepository
            .GetWatchListAsync();
            
        var reportData = new ReportData
        {
            Title = "Спреды",
            Header =
            [
                new ReportParameter(KnownDisplayTypes.String, "Тикер 1"),
                new ReportParameter(KnownDisplayTypes.String, "Цена"),
                new ReportParameter(KnownDisplayTypes.String, "Описание"),
                new ReportParameter(KnownDisplayTypes.String, "Тикер 2"),
                new ReportParameter(KnownDisplayTypes.String, "Цена"),
                new ReportParameter(KnownDisplayTypes.String, "Описание"),
                new ReportParameter(KnownDisplayTypes.String, "Спред"),
                new ReportParameter(KnownDisplayTypes.String, "Спред, %"),
                new ReportParameter(KnownDisplayTypes.String, "Фандинг")
            ]
        };

        foreach (var spread in spreads)
        {
            List<ReportParameter> data =
            [
                new (KnownDisplayTypes.Ticker, spread.FirstInstrumentTicker),
                new (KnownDisplayTypes.Ruble, spread.FirstInstrumentPrice.ToString("N5")),
                new (KnownDisplayTypes.String, spread.FirstInstrumentRole),
                new (KnownDisplayTypes.Ticker, spread.SecondInstrumentTicker),
                new (KnownDisplayTypes.Ruble, spread.SecondInstrumentPrice.ToString("N5")),
                new (KnownDisplayTypes.String, spread.SecondInstrumentRole),
                new (KnownDisplayTypes.Ruble, spread.PriceDifference.ToString("N5")),
                new (KnownDisplayTypes.Percent, spread.PriceDifferencePrc.ToString("N5")),
                new (KnownDisplayTypes.Ruble, spread.Funding.ToString("N5"))
            ];
            
            reportData.Data.Add(data);
        }
        
        return reportData;
    }

    #endregion
    
    #region Currencies
    
    /// <inheritdoc />
    public async Task<ReportData> GetReportCurrenciesAnalyseSupertrendAsync(
        GetReportAnalyseRequest request) =>
        await GetReportDataCurrenciesByAnalyseType(
            await currencyRepository.GetWatchListAsync(),
            request.From,
            request.To,
            KnownAnalyseTypes.Supertrend);

    /// <inheritdoc />
    public async Task<ReportData> GetReportCurrenciesAnalyseCandleSequenceAsync(
        GetReportAnalyseRequest request) =>
        await GetReportDataCurrenciesByAnalyseType(
            await currencyRepository.GetWatchListAsync(),
            request.From,
            request.To,
            KnownAnalyseTypes.CandleSequence);

    #endregion
    
    #region Futures
    
    /// <inheritdoc />
    public async Task<ReportData> GetReportFuturesAnalyseRsiAsync(
        GetReportAnalyseRequest request) =>
        await GetReportDataFuturesByAnalyseType(
            await futureRepository.GetWatchListAsync(),
            request.From,
            request.To,
            KnownAnalyseTypes.Rsi);

    /// <inheritdoc />
    public async Task<ReportData> GetReportFuturesAnalyseCandleVolumeAsync(
        GetReportAnalyseRequest request) =>
        await GetReportDataFuturesByAnalyseType(
            await futureRepository.GetWatchListAsync(),
            request.From,
            request.To,
            KnownAnalyseTypes.CandleVolume);

    /// <inheritdoc />
    public async Task<ReportData> GetReportFuturesAnalyseCandleSequenceAsync(
        GetReportAnalyseRequest request) =>
        await GetReportDataFuturesByAnalyseType(
            await futureRepository.GetWatchListAsync(),
            request.From,
            request.To,
            KnownAnalyseTypes.CandleSequence);

    /// <inheritdoc />
    public async Task<ReportData> GetReportFuturesAnalyseSupertrendAsync(
        GetReportAnalyseRequest request) =>
        await GetReportDataFuturesByAnalyseType(
            await futureRepository.GetWatchListAsync(),
            request.From,
            request.To,
            KnownAnalyseTypes.Supertrend);

    #endregion
    
    #region Indexes
    
    /// <inheritdoc />
    public async Task<ReportData> GetReportIndexesAnalyseSupertrendAsync(
        GetReportAnalyseRequest request) =>
        await GetReportDataIndexesByAnalyseType(
            await indexRepository.GetWatchListAsync(),
            request.From,
            request.To,
            KnownAnalyseTypes.Supertrend);

    /// <inheritdoc />
    public async Task<ReportData> GetReportIndexesAnalyseCandleSequenceAsync(
        GetReportAnalyseRequest request) =>
        await GetReportDataIndexesByAnalyseType(
            await indexRepository.GetWatchListAsync(),
            request.From,
            request.To,
            KnownAnalyseTypes.CandleSequence);

    /// <inheritdoc />
    public async Task<ReportData> GetReportIndexesAnalyseYieldLtmAsync(
        GetReportAnalyseRequest request) =>
        await GetReportDataIndexesByAnalyseType(
            await indexRepository.GetWatchListAsync(),
            request.From,
            request.To,
            KnownAnalyseTypes.YieldLtm);
    
    #endregion
    
    private List<ReportParameter> GetDates(
        DateOnly from, DateOnly to)
    {
        var curDate = from;
        var dates = new List<ReportParameter>();

        while (curDate <= to)
        {
            dates.Add(new ReportParameter(
                KnownDisplayTypes.Date,
                curDate.ToString(KnownDateTimeFormats.DateISO)));
                
            curDate = curDate.AddDays(1);
        }
            
        return dates;
    }
        
    private async Task<ReportData> GetReportDataSharesByAnalyseType(
        List<Share> shares,
        DateOnly from,
        DateOnly to,
        string analyseType)
    {
        var instrumentIds = shares
            .Select(x => x.InstrumentId)
            .ToList();        
        
        var analyseResults = (await analyseResultRepository
                .GetAsync(instrumentIds, from, to))
            .Where(x => x.AnalyseType == analyseType)
            .ToList();

        var dividendInfos = await dividendInfoRepository
            .GetAsync(instrumentIds, to.AddDays(1), to.AddDays(WindowInDays));
            
        var dates = GetDates(from, to.AddDays(WindowInDays));
            
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

        foreach (var share in shares)
        {
            var data = new List<ReportParameter>
            {
                new (KnownDisplayTypes.Ticker, share.Ticker), 
                new (KnownDisplayTypes.Sector, share.Sector)
            };

            foreach (var date in dates)
            {
                if (DateOnly.FromDateTime(Convert.ToDateTime(date.Value)) <= to)
                {
                    var analyseResult = analyseResults
                        .FirstOrDefault(x =>
                            x.InstrumentId == share.InstrumentId &&
                            x.Date.ToString(KnownDateTimeFormats.DateISO) == date.Value);

                    data.Add(analyseResult is not null
                        ? new ReportParameter(
                            KnownDisplayTypes.AnalyseResult,
                            analyseResult.Result)
                        : new ReportParameter(
                            KnownDisplayTypes.AnalyseResult,
                            string.Empty));
                }

                else
                {
                    var dividendInfo = dividendInfos
                        .FirstOrDefault(x => 
                            x.Ticker == share.Ticker && 
                            x.RecordDate.ToString(KnownDateTimeFormats.DateISO) == date.Value);

                    data.Add(dividendInfo is not null 
                        ? new ReportParameter(
                            KnownDisplayTypes.Percent, 
                            dividendInfo.DividendPrc.ToString(CultureInfo.InvariantCulture)) 
                        : new ReportParameter(
                            KnownDisplayTypes.Percent, 
                            string.Empty));
                }
            }
                
            reportData.Data.Add(data);
        }
            
        return reportData;
    }
    
    private async Task<ReportData> GetReportDataIndexesByAnalyseType(
        List<FinIndex> indexes,
        DateOnly from,
        DateOnly to,
        string analyseType)
    {
        var instrumentIds = indexes
            .Select(x => x.InstrumentId)
            .ToList();        
        
        var analyseResults = (await analyseResultRepository
                .GetAsync(instrumentIds, from, to))
            .Where(x => x.AnalyseType == analyseType)
            .ToList();
            
        var dates = GetDates(from, to);
            
        var reportData = new ReportData
        {
            Title = $"Анализ {analyseType} " +
                    $"с {from.ToString(KnownDateTimeFormats.DateISO)} " +
                    $"по {to.ToString(KnownDateTimeFormats.DateISO)}",
                
            Header = 
            [
                new ReportParameter(KnownDisplayTypes.String, "Тикер")
            ]
        };

        reportData.Header.AddRange(dates);

        foreach (var indicative in indexes)
        {
            var data = new List<ReportParameter>
            {
                new (KnownDisplayTypes.Ticker, indicative.Ticker)
            };

            foreach (var date in dates)
            {
                var analyseResult = analyseResults
                    .FirstOrDefault(x => 
                        x.InstrumentId == indicative.InstrumentId && 
                        x.Date.ToString(KnownDateTimeFormats.DateISO) == date.Value);

                data.Add(analyseResult is not null 
                    ? new ReportParameter(
                        KnownDisplayTypes.AnalyseResult, 
                        analyseResult.Result) 
                    : new ReportParameter(
                        KnownDisplayTypes.AnalyseResult, 
                        string.Empty));
            }
                
            reportData.Data.Add(data);
        }
            
        return reportData;
    }
    
    private async Task<ReportData> GetReportDataFuturesByAnalyseType(
        List<Future> futures,
        DateOnly from,
        DateOnly to,
        string analyseType)
    {
        var instrumentIds = futures
            .Select(x => x.InstrumentId)
            .ToList();        
        
        var analyseResults = (await analyseResultRepository
                .GetAsync(instrumentIds, from, to))
            .Where(x => x.AnalyseType == analyseType)
            .ToList();
            
        var dates = GetDates(from, to);
            
        var reportData = new ReportData
        {
            Title = $"Анализ {analyseType} " +
                    $"с {from.ToString(KnownDateTimeFormats.DateISO)} " +
                    $"по {to.ToString(KnownDateTimeFormats.DateISO)}",
                
            Header = 
            [
                new ReportParameter(KnownDisplayTypes.String, "Тикер")
            ]
        };

        reportData.Header.AddRange(dates);

        foreach (var future in futures)
        {
            var data = new List<ReportParameter>
            {
                new (KnownDisplayTypes.Ticker, future.Ticker)
            };

            foreach (var date in dates)
            {
                var analyseResult = analyseResults
                    .FirstOrDefault(x => 
                        x.InstrumentId == future.InstrumentId && 
                        x.Date.ToString(KnownDateTimeFormats.DateISO) == date.Value);

                data.Add(analyseResult is not null 
                    ? new ReportParameter(
                        KnownDisplayTypes.AnalyseResult, 
                        analyseResult.Result) 
                    : new ReportParameter(
                        KnownDisplayTypes.AnalyseResult, 
                        string.Empty));
            }
                
            reportData.Data.Add(data);
        }
            
        return reportData;
    }
    
    private async Task<ReportData> GetReportDataBondsByAnalyseType(
        List<Bond> bonds,
        DateOnly from,
        DateOnly to,
        string analyseType)
    {
        var instrumentIds = bonds
            .Select(x => x.InstrumentId)
            .ToList();        
        
        var analyseResults = (await analyseResultRepository
                .GetAsync(instrumentIds, from, to))
            .Where(x => x.AnalyseType == analyseType)
            .ToList();
            
        var dates = GetDates(from, to);
            
        var reportData = new ReportData
        {
            Title = $"Анализ {analyseType} " +
                    $"с {from.ToString(KnownDateTimeFormats.DateISO)} " +
                    $"по {to.ToString(KnownDateTimeFormats.DateISO)}",
                
            Header = 
            [
                new ReportParameter(KnownDisplayTypes.String, "Тикер")
            ]
        };

        reportData.Header.AddRange(dates);

        foreach (var bond in bonds)
        {
            var data = new List<ReportParameter>
            {
                new (KnownDisplayTypes.Ticker, bond.Ticker)
            };

            foreach (var date in dates)
            {
                var analyseResult = analyseResults
                    .FirstOrDefault(x => 
                        x.InstrumentId == bond.InstrumentId && 
                        x.Date.ToString(KnownDateTimeFormats.DateISO) == date.Value);

                data.Add(analyseResult is not null 
                    ? new ReportParameter(
                        KnownDisplayTypes.AnalyseResult, 
                        analyseResult.Result) 
                    : new ReportParameter(
                        KnownDisplayTypes.AnalyseResult, 
                        string.Empty));
            }
                
            reportData.Data.Add(data);
        }
            
        return reportData;
    }
    
    private async Task<ReportData> GetReportDataCurrenciesByAnalyseType(
        List<Currency> currencies,
        DateOnly from,
        DateOnly to,
        string analyseType)
    {
        var instrumentIds = currencies
            .Select(x => x.InstrumentId)
            .ToList();        
        
        var analyseResults = (await analyseResultRepository
                .GetAsync(instrumentIds, from, to))
            .Where(x => x.AnalyseType == analyseType)
            .ToList();
            
        var dates = GetDates(from, to);
            
        var reportData = new ReportData
        {
            Title = $"Анализ {analyseType} " +
                    $"с {from.ToString(KnownDateTimeFormats.DateISO)} " +
                    $"по {to.ToString(KnownDateTimeFormats.DateISO)}",
                
            Header = 
            [
                new ReportParameter(KnownDisplayTypes.String, "Тикер")
            ]
        };

        reportData.Header.AddRange(dates);

        foreach (var currency in currencies)
        {
            var data = new List<ReportParameter>
            {
                new (KnownDisplayTypes.Ticker, currency.Ticker)
            };

            foreach (var date in dates)
            {
                var analyseResult = analyseResults
                    .FirstOrDefault(x => 
                        x.InstrumentId == currency.InstrumentId && 
                        x.Date.ToString(KnownDateTimeFormats.DateISO) == date.Value);

                data.Add(analyseResult is not null 
                    ? new ReportParameter(
                        KnownDisplayTypes.AnalyseResult, 
                        analyseResult.Result) 
                    : new ReportParameter(
                        KnownDisplayTypes.AnalyseResult, 
                        string.Empty));
            }
                
            reportData.Data.Add(data);
        }
            
        return reportData;
    }
}