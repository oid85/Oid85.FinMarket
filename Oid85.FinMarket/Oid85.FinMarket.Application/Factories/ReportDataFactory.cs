﻿using Microsoft.Extensions.Configuration;
using Oid85.FinMarket.Application.Helpers;
using Oid85.FinMarket.Application.Interfaces.Factories;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Application.Models.Reports;
using Oid85.FinMarket.Common.Helpers;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.External.ResourceStore;

namespace Oid85.FinMarket.Application.Factories;

public class ReportDataFactory(
    IConfiguration configuration,
    IInstrumentRepository instrumentRepository,
    IAnalyseResultRepository analyseResultRepository,
    IDividendInfoRepository dividendInfoRepository,
    IBondCouponRepository bondCouponRepository,
    IBondRepository bondRepository,
    IShareRepository shareRepository,
    IDailyCandleRepository dailyCandleRepository,
    IShareMultiplicatorRepository shareMultiplicatorRepository,
    IBankMultiplicatorRepository bankMultiplicatorRepository,
    IForecastTargetRepository forecastTargetRepository,
    IForecastConsensusRepository forecastConsensusRepository,
    IAssetReportEventRepository assetReportEventRepository,
    IStrategySignalRepository strategySignalRepository,
    IBacktestResultRepository backtestResultRepository,
    IResourceStoreService resourceStoreService,
    ColorHelper colorHelper,
    IMarketEventRepository marketEventRepository,
    IFeerGreedRepository feerGreedRepository,
    INormalizeService normalizeService) 
    : IReportDataFactory
{
    private async Task<List<AnalyseResult>> GetAnalyseResults(
        List<Guid> instrumentIds, List<string> analyseTypes, DateOnly from, DateOnly to) =>
        (await analyseResultRepository.GetAsync(instrumentIds, from, to))
        .Where(x => analyseTypes.Contains(x.AnalyseType)).ToList();

    private static ReportData CreateNewReportDataWithHeaders(List<string> headers)
    {
        var reportData = new ReportData();
        
        foreach (var header in headers)
            reportData.Header.Add(GetString(header));
        
        return reportData;
    }

    private static ReportData CreateNewReportDataWithHeaders(List<string> headers, List<DateOnly> dates)
    {
        var reportData = CreateNewReportDataWithHeaders(headers);
        
        foreach (var date in dates)
            reportData.Header.Add(GetDate(date));
        
        return reportData;
    }

    private static string CreateTitleWithDates(string title, DateOnly from, DateOnly to)
    {
        string fromString = from.ToString(KnownDateTimeFormats.DateISO);
        string toString = to.ToString(KnownDateTimeFormats.DateISO);
        return $"{title} с {fromString} по {toString}";
    }

    private static double CalculateBondCouponProfitPercent(Bond bond, BondCoupon? coupon)
    {
        if (coupon is null)
            return 0.0;

        if (bond.LastPrice == 0.0)
            return 0.0;
        
        if (coupon.CouponPeriod == 0)
            return 0.0;

        double numberCouponsByYear = 365.0 / coupon.CouponPeriod;
        double yieldOfOneCoupon = coupon.PayOneBond / (bond.LastPrice * 10.0 + bond.Nkd);
        
        return numberCouponsByYear * yieldOfOneCoupon * 100.0;
    }
    
    private async ValueTask<double> CalculateDividendProfitPercentAsync(DividendInfo dividendInfo)
    {
        var share = await shareRepository.GetAsync(dividendInfo.InstrumentId);

        if (share is null)
            return 0.0;

        if (share.LastPrice == 0.0)
            return 0.0;
        
        return dividendInfo.Dividend / share.LastPrice * 100.0;
    }
    
    private int CalculateAggregateAnalyseResult(List<AnalyseResult> analyseResults)
    {
        var supertrend = analyseResults.FirstOrDefault(x => x.AnalyseType == KnownAnalyseTypes.Supertrend);

        if (supertrend is null)
            return 0;

        return supertrend.ResultNumber switch
        {
            > 0.0 => 
                (int) analyseResults
                    .Where(x => x.ResultNumber > 0.0)
                    .Select(x => x.ResultNumber)
                    .Sum(),
            
            < 0.0 => 
                (int) analyseResults
                    .Where(x => x.ResultNumber < 0.0)
                    .Select(x => x.ResultNumber)
                    .Sum(),
            
            _ => 0
        };
    }
    
    private static ReportParameter GetTicker(string value, string color = KnownColors.White) =>
        new (KnownDisplayTypes.Ticker, value, color);
    
    private static ReportParameter GetSector(string value, string color = KnownColors.White) =>
        new (KnownDisplayTypes.Sector, value, color);
    
    private static ReportParameter GetString(string value, string color = KnownColors.White) =>
        new (KnownDisplayTypes.String, value, color);    
    
    private static ReportParameter GetDate(DateOnly value, string color = KnownColors.White) =>
        new (KnownDisplayTypes.Date, value.ToString(KnownDateTimeFormats.DateISO), color);     
    
    private static ReportParameter GetRuble(double value, string color = KnownColors.White) =>
        new (KnownDisplayTypes.Ruble, value.ToString("N2").TrimEnd('0').TrimEnd(','), color);     
    
    private static ReportParameter GetPercent(double value, string color = KnownColors.White) =>
        new (KnownDisplayTypes.Percent, value.ToString("N2").TrimEnd('0').TrimEnd(','), color);     
    
    private static ReportParameter GetNumber(double value, string color = KnownColors.White) =>
        new (KnownDisplayTypes.Number, value.ToString("N2").TrimEnd('0').TrimEnd(','), color);
    
    private static ReportParameter GetCurrency(string value, string color = KnownColors.White) =>
        new (KnownDisplayTypes.Currency, value, color);  
    
    private static ReportParameter GetAnalyseResult(string value, string color = KnownColors.White) =>
        new (KnownDisplayTypes.AnalyseResult, value, color);   
    
    private static ReportParameter GetAssetReportEvent(string value, string color = KnownColors.White) =>
        new (KnownDisplayTypes.AssetReportEvent, value, color);   
    
    private static ReportParameter GetBacktestResultByIdButton(string value, string color = KnownColors.White) =>
        new (KnownDisplayTypes.BacktestResultByIdButton, value, color);    
    
    private static ReportParameter GetBacktestResultByTickerButton(string value, string color = KnownColors.White) =>
        new (KnownDisplayTypes.BacktestResultByTickerButton, value, color);    
    
    public async Task<ReportData> CreateReportDataAsync(
        List<Guid> instrumentIds, string analyseType, DateOnly from, DateOnly to)
    {
        var analyseResults = await GetAnalyseResults(
            instrumentIds, [analyseType], from, to);

        var dates = DateHelper.GetDates(from, to);
        var reportData = CreateNewReportDataWithHeaders(["Тикер", "Сектор", "Эмитент"], dates);
        reportData.Title = CreateTitleWithDates(analyseType, from, to);
        
        foreach (var instrumentId in instrumentIds)
        {
            var instrumentAnalyseResults = analyseResults
                .Where(x => x.InstrumentId == instrumentId).ToList();
            
            if (instrumentAnalyseResults is [])
                continue;
            
            var instrument = await instrumentRepository.GetAsync(instrumentId);
            
            if (instrument is null)
                continue;
            
            List<ReportParameter> data =
            [
                GetTicker(instrument.Ticker),
                GetSector(instrument.Sector),
                GetString(normalizeService.NormalizeInstrumentName(instrument.Name))
            ];
            
            foreach (var date in dates)
            {
                var analyseResult = instrumentAnalyseResults.FirstOrDefault(x => x.Date == date);

                if (analyseResult is not null)
                {
                    string color = await colorHelper.GetColorByAnalyseType(analyseType, analyseResult);

                    if (analyseType is KnownAnalyseTypes.YieldLtm or KnownAnalyseTypes.DrawdownFromMaximum)
                        data.Add(GetPercent(analyseResult.ResultNumber, color));
                    
                    else if (analyseType is KnownAnalyseTypes.Atr)
                        data.Add(GetNumber(analyseResult.ResultNumber, color));                    
                    
                    else if (analyseType is KnownAnalyseTypes.Hurst)
                        data.Add(GetNumber(analyseResult.ResultNumber, color));                     
                    
                    else
                        data.Add(GetAnalyseResult(string.Empty, color));
                }

                else
                    data.Add(GetString(string.Empty));
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
            
        var dates = DateHelper.GetDates(from, to);
        var reportData = CreateNewReportDataWithHeaders(["Тикер", "Сектор", "Эмитент"], dates);
        reportData.Title = reportData.Title = CreateTitleWithDates("Aggregated", from, to);

        foreach (var instrumentId in instrumentIds)
        {
            var instrumentAnalyseResults = analyseResults
                .Where(x => x.InstrumentId == instrumentId).ToList();
            
            if (instrumentAnalyseResults is [])
                continue;
            
            var instrument = await instrumentRepository.GetAsync(instrumentId);
            
            if (instrument is null)
                continue;
            
            List<ReportParameter> data =
            [
                GetTicker(instrument.Ticker),
                GetSector(instrument.Sector),
                GetString(normalizeService.NormalizeInstrumentName(instrument.Name))
            ];

            foreach (var date in dates)
            {
                var instrumentAnalyseResultsByDate = instrumentAnalyseResults
                    .Where(x => x.Date == date).ToList();

                int resultNumber = CalculateAggregateAnalyseResult(instrumentAnalyseResultsByDate);
                string color = await colorHelper.GetColorAggregated(resultNumber);
                var candle = await dailyCandleRepository.GetAsync(instrumentId, date);
                string price = candle?.Close.ToString("N5").TrimEnd('0').TrimEnd(',') ?? string.Empty;
                data.Add(GetAnalyseResult(price, color));
            }
                
            reportData.Data.Add(data);
        }
            
        return reportData;
    }

    public async Task<ReportData> CreateDividendInfoReportDataAsync(List<Guid> instrumentIds)
    {
        var dividendInfos = await dividendInfoRepository.GetAsync(instrumentIds);
        int days = configuration.GetValue<int>(KnownSettingsKeys.ApplicationSettingsOutputWindowInDays);
        var from = DateOnly.FromDateTime(DateTime.Today);
        var to = from.AddDays(days);
        var dates = DateHelper.GetDates(from, to);
        
        var reportData = CreateNewReportDataWithHeaders(
            ["Тикер", "Эмитент", "Фикс. р.", "Объяв.", "Размер, руб", "Дох-ть, %", "Тек. дох-ть, %"], dates);
        
        foreach (var dividendInfo in dividendInfos)
        {
            var instrument = await instrumentRepository.GetAsync(dividendInfo.InstrumentId);
            var profitPrc = await CalculateDividendProfitPercentAsync(dividendInfo);
            string instrumentName = instrument?.Name ?? string.Empty;
            string color = await colorHelper.GetColorYieldDividend(profitPrc);

            List<ReportParameter> data =
            [
                GetTicker(dividendInfo.Ticker),
                GetString(normalizeService.NormalizeInstrumentName(instrumentName)),
                GetDate(dividendInfo.RecordDate),
                GetDate(dividendInfo.DeclaredDate),
                GetRuble(dividendInfo.Dividend),
                GetPercent(dividendInfo.DividendPrc, color),
                GetPercent(profitPrc, color)
            ];
            
            foreach (var date in dates)
                data.Add(dividendInfo.RecordDate == date
                    ? GetPercent(profitPrc, color)
                    : GetString(string.Empty));

            reportData.Data.Add(data);
        }
        
        return reportData;
    }

    public async Task<ReportData> CreateShareMultiplicatorReportDataAsync(List<Guid> instrumentIds)
    {
        var shares = (await shareRepository.GetAsync(instrumentIds))
            .OrderBy(x => x.Sector);

        var reportData = CreateNewReportDataWithHeaders(
            [
                "Тикер", 
                "Сектор", 
                "Эмитент", 
                "Капит-ция",
                "EV",
                "Выручка",
                "ЧП",
                "ДД АО",
                "ДД АП",
                "ДД/ЧП",
                "P/E",
                "P/S",
                "P/B",
                "EV/EBITDA",
                "EBITDA margin",
                "NetDebt/EBITDA"
            ]);

        reportData.Title = "Мультипликаторы";
        
        foreach (var share in shares)
        {
            var multiplicator = await shareMultiplicatorRepository.GetAsync(share.Ticker);
            
            if (multiplicator is null)
                continue;

            List<ReportParameter> data =
            [
                GetTicker(share.Ticker),
                GetSector(share.Sector),
                GetString(normalizeService.NormalizeInstrumentName(share.Name)),
                GetNumber(multiplicator.MarketCap),
                GetNumber(multiplicator.Ev),
                GetNumber(multiplicator.Revenue),
                GetNumber(multiplicator.NetIncome),
                GetNumber(multiplicator.DdAo),
                GetNumber(multiplicator.DdAp),
                GetNumber(multiplicator.DdNetIncome),
                GetNumber(multiplicator.Pe, await colorHelper.GetColorPeAsync(multiplicator.Pe)),
                GetNumber(multiplicator.Ps),
                GetNumber(multiplicator.Pb),
                GetNumber(multiplicator.EvEbitda, await colorHelper.GetColorEvEbitda(multiplicator.EvEbitda)),
                GetNumber(multiplicator.EbitdaMargin),
                GetNumber(multiplicator.NetDebtEbitda, await colorHelper.GetColorNetDebtEbitda(multiplicator.NetDebtEbitda))
            ];
            
            reportData.Data.Add(data);
        }
        
        return reportData;
    }

    public async Task<ReportData> CreateBankMultiplicatorReportDataAsync(List<Guid> instrumentIds)
    {
        var shares = (await shareRepository.GetAsync(instrumentIds))
            .OrderBy(x => x.Sector);

        var reportData = CreateNewReportDataWithHeaders(
            [
                "Тикер", 
                "Сектор", 
                "Эмитент", 
                "Капит-ция",
                "Чист.опер.доход",
                "ЧП",
                "ДД АО",
                "ДД АП",
                "ДД/ЧП",
                "P/E",
                "P/B",
                "Чист.проц.маржа",
                "ROE",
                "ROA"
            ]);

        reportData.Title = "Мультипликаторы";
        
        foreach (var share in shares)
        {
            var multiplicator = await bankMultiplicatorRepository.GetAsync(share.Ticker);
            
            if (multiplicator is null)
                continue;

            List<ReportParameter> data =
            [
                GetTicker(share.Ticker),
                GetSector(share.Sector),
                GetString(normalizeService.NormalizeInstrumentName(share.Name)),
                GetNumber(multiplicator.MarketCap),
                GetNumber(multiplicator.NetOperatingIncome),
                GetNumber(multiplicator.NetIncome),
                GetNumber(multiplicator.DdAo),
                GetNumber(multiplicator.DdAp),
                GetNumber(multiplicator.DdNetIncome),
                GetNumber(multiplicator.Pe, await colorHelper.GetColorPeAsync(multiplicator.Pe)),
                GetNumber(multiplicator.Pb),
                GetNumber(multiplicator.NetInterestMargin),
                GetNumber(multiplicator.Roe),
                GetNumber(multiplicator.Roa)
            ];
            
            reportData.Data.Add(data);
        }
        
        return reportData;
    }    
    
    public async Task<ReportData> CreateForecastTargetReportDataAsync(List<Guid> instrumentIds)
    {
        var forecastTargets = (await forecastTargetRepository.GetAsync(instrumentIds))
            .Where(x => !string.IsNullOrEmpty(x.Ticker)).ToList();
        
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
        
        var reportData = CreateNewReportDataWithHeaders(
        [
            "Тикер", "Эмитент", "Компания", "Прогноз", "Дата прогноза", "Валюта", 
            "Тек. цена", "Прогноз. цена", "Изм. цены", "Изм. цены, %"
        ]);

        reportData.Title = "Прогнозы";

        foreach (var forecastTarget in actualForecastTargets)
        {
            string color = await colorHelper.GetColorForecastRecommendation(forecastTarget.RecommendationString);
            
            reportData.Data.Add(
            [
                GetTicker(forecastTarget.Ticker),
                GetString(normalizeService.NormalizeInstrumentName(forecastTarget.ShowName)),
                GetString(forecastTarget.Company),
                GetString(forecastTarget.RecommendationString, color),
                GetDate(forecastTarget.RecommendationDate),
                GetCurrency(forecastTarget.Currency),
                GetRuble(forecastTarget.CurrentPrice),
                GetRuble(forecastTarget.TargetPrice),
                GetRuble(forecastTarget.PriceChange),
                GetPercent(forecastTarget.PriceChangeRel)
            ]);
        }
        
        return reportData;
    }

    public async Task<ReportData> CreateForecastConsensusReportDataAsync(List<Guid> instrumentIds)
    {
        var forecastConsensuses = (await forecastConsensusRepository.GetAsync(instrumentIds))
            .Where(x => !string.IsNullOrEmpty(x.Ticker)).ToList();
        
        var reportData = CreateNewReportDataWithHeaders(
        [
            "Тикер", "Секторо", "Эмитент", "Прогноз", "Валюта", "Тек. цена", "Мин. цена прогноза", "Макс. цена прогноза",
			"Мин. потенциал", "Макс. потенциал"
        ]);

        reportData.Title = "Консенсус-прогнозы";

        foreach (var forecastConsensus in forecastConsensuses)
        {
            var instrument = await instrumentRepository.GetAsync(forecastConsensus.Ticker);

            if (instrument is null)
                continue;
            
            string colorRecommendation = await colorHelper.GetColorForecastRecommendation(forecastConsensus.RecommendationString);

            string colorCurrentPrice = ColorHelper.GetColorForForecastPrice(
                forecastConsensus.CurrentPrice, forecastConsensus.MinTarget, forecastConsensus.MaxTarget);
            
			double minTargetPrc = ((forecastConsensus.MinTarget - forecastConsensus.CurrentPrice) / forecastConsensus.CurrentPrice) * 100.0;
			double maxTargetPrc = ((forecastConsensus.MaxTarget - forecastConsensus.CurrentPrice) / forecastConsensus.CurrentPrice) * 100.0;
			
            reportData.Data.Add(
            [
                GetTicker(instrument.Ticker),
                GetSector(instrument.Sector),
                GetString(normalizeService.NormalizeInstrumentName(instrument.Name)),
                GetString(forecastConsensus.RecommendationString, colorRecommendation),
                GetCurrency(forecastConsensus.Currency),
                GetRuble(forecastConsensus.CurrentPrice, colorCurrentPrice),
                GetRuble(forecastConsensus.MinTarget),
                GetRuble(forecastConsensus.MaxTarget),
				GetPercent(minTargetPrc, minTargetPrc > 0.0 ? KnownColors.Green : KnownColors.Red),
                GetPercent(maxTargetPrc, maxTargetPrc > 0.0 ? KnownColors.Green : KnownColors.Red)
            ]);
        }

        return reportData;
    }

    public async Task<ReportData> CreateBondCouponReportDataAsync(List<Guid> instrumentIds)
    {
        int days = configuration.GetValue<int>(KnownSettingsKeys.ApplicationSettingsOutputWindowInDays);
        var bonds = await bondRepository.GetAsync(instrumentIds);
        var startDate = DateOnly.FromDateTime(DateTime.Today);
        var endDate = DateOnly.FromDateTime(DateTime.Today).AddDays(days);
        var dates = DateHelper.GetDates(startDate, endDate);
        
        var reportData = CreateNewReportDataWithHeaders(
            [
                "Тикер", "Сектор", "Наименование", "Уровень риска", "Валюта", "Плав. купон", 
                "До погаш.", "Цена", "НКД", "Куп. период", "Дох-ть куп."
            ], dates);
        
        reportData.Title = "Купоны";
        
        var bondCoupons = (await bondCouponRepository
            .GetAsync(instrumentIds))
            .Where(x =>
                x.CouponDate >= startDate &&
                x.CouponDate <= endDate)
            .ToList();

        var instrumentIdsWithCoupon = bondCoupons.Select(x => x.InstrumentId).Distinct().ToList();

        DateOnly GetNearCouponDate(Guid instrumentId)
        {
            var coupon = bondCoupons
                .OrderBy(x => x.CouponDate)
                .FirstOrDefault(x => x.InstrumentId == instrumentId);

            if (coupon is not null)
                return coupon.CouponDate;
            
            return DateOnly.MaxValue;
        }
        
        var bondsWithCoupons = bonds
            .Where(x => instrumentIdsWithCoupon.Contains(x.InstrumentId))
            .OrderBy(x => GetNearCouponDate(x.InstrumentId))
            .ToList();
            
        foreach (var bond in bondsWithCoupons)
        {
            var bondCouponsByInstrument = bondCoupons
                .Where(x => x.InstrumentId == bond.InstrumentId)
                .OrderBy(x => x.CouponDate).ToList();
            
            var profitPrc = CalculateBondCouponProfitPercent(bond, bondCouponsByInstrument.FirstOrDefault());
            
            var riskLevel = bond.RiskLevel switch
            {
                0 => KnownRiskLevels.Low,
                1 => KnownRiskLevels.Middle,
                2 => KnownRiskLevels.High,
                3 => KnownRiskLevels.VeryHigh,
                _ => string.Empty
            };

            string riskLevelColor = await colorHelper.GetColorRiskLevel(riskLevel);
            
            List<ReportParameter> data =
            [
                GetTicker(bond.Ticker),
                GetSector(bond.Sector),
                GetString(normalizeService.NormalizeInstrumentName(bond.Name)),
                GetString(riskLevel, riskLevelColor),
                GetCurrency(bond.Currency),
                GetString(bond.FloatingCouponFlag ? "Да" : string.Empty),
                GetString((bond.MaturityDate.ToDateTime(TimeOnly.MinValue) - DateTime.Today).Days.ToString()),
                GetNumber(bond.LastPrice * 10.0),
                GetNumber(bond.Nkd)
            ];
            
            string couponPeriod = bondCouponsByInstrument.FirstOrDefault()?.CouponPeriod.ToString() ?? string.Empty;
            data.Add(GetString(couponPeriod));
            
            string color = await colorHelper.GetColorYieldCoupon(profitPrc);
            data.Add(GetPercent(profitPrc, color));         

            foreach (var date in dates)
            {
                var bondCoupon = bondCouponsByInstrument.FirstOrDefault(x => x.CouponDate == date);
                
                data.Add(bondCoupon is not null 
                    ? GetRuble(bondCoupon.PayOneBond, color) 
                    : GetString(string.Empty));
            }
                
            reportData.Data.Add(data);
        }
            
        return reportData;
    }
    
    public async Task<ReportData> CreateActiveMarketEventsReportDataAsync(List<Guid> instrumentIds)
    {
        var marketEvents = (await marketEventRepository.GetActivatedAsync())
            .Where(x => instrumentIds.Contains(x.InstrumentId))
            .OrderByDescending(x => new DateTime(x.Date, x.Time))
            .ToList();
        
        var reportData = CreateNewReportDataWithHeaders(
            [
                "Тикер", "Сектор", "Наименование", 
                "ST Up", "ST Down", "Vol Up", "Candle White", "Candle Black",
                "RSI O/B In", "RSI O/B Out", "RSI O/S In", "RSI O/S Out",
                "Cross", "Forecast", "IntDay Impulse"
            ]);
        
        reportData.Title = "Активные рыночные события";

        foreach (var instrumentId in instrumentIds)
        {
            var instrument = await instrumentRepository.GetAsync(instrumentId);
            
            if (instrument is null)
                continue;

            reportData.Data.Add(
            [
                GetTicker(instrument.Ticker),
                GetSector(instrument.Sector),
                GetString(normalizeService.NormalizeInstrumentName(instrument.Name)),
                await GetMarketEventReportParameter(KnownMarketEventTypes.SupertrendUp),
                await GetMarketEventReportParameter(KnownMarketEventTypes.SupertrendDown),
                await GetMarketEventReportParameter(KnownMarketEventTypes.CandleVolumeUp),
                await GetMarketEventReportParameter(KnownMarketEventTypes.CandleSequenceWhite),
                await GetMarketEventReportParameter(KnownMarketEventTypes.CandleSequenceBlack),
                await GetMarketEventReportParameter(KnownMarketEventTypes.RsiOverBoughtInput),
                await GetMarketEventReportParameter(KnownMarketEventTypes.RsiOverBoughtOutput),
                await GetMarketEventReportParameter(KnownMarketEventTypes.RsiOverSoldInput),
                await GetMarketEventReportParameter(KnownMarketEventTypes.RsiOverSoldOutput),
                await GetMarketEventReportParameter(KnownMarketEventTypes.CrossPriceLevel),
                await GetMarketEventReportParameter(KnownMarketEventTypes.ForecastReleased)
            ]);
            
            continue;

            async Task<ReportParameter> GetMarketEventReportParameter(string marketEventType)
            {
                var marketEvent = marketEvents
                    .FirstOrDefault(x => 
                        x.InstrumentId == instrumentId &&
                        x.MarketEventType == marketEventType);
                
                if (marketEvent is null)
                    return GetString(string.Empty);
                
                string color = await colorHelper.GetColorMarketEvent(marketEventType);
                
                return GetString(string.Empty, color);
            }
        }
        
        return reportData;
    }

    public async Task<ReportData> CreateAssetReportEventsReportDataAsync(List<Guid> instrumentIds)
    {
        var assetReportEvents = await assetReportEventRepository.GetAsync(instrumentIds);
        int days = configuration.GetValue<int>(KnownSettingsKeys.ApplicationSettingsOutputWindowInDays);
        var from = DateOnly.FromDateTime(DateTime.Today);
        var to = from.AddDays(days);
        var dates = DateHelper.GetDates(from, to);
        
        var reportData = CreateNewReportDataWithHeaders(
            ["Тикер", "Сектор", "Наименование"], dates);
        
        foreach (var assetReportEvent in assetReportEvents)
        {
            var instrument = await instrumentRepository.GetAsync(assetReportEvent.InstrumentId);
            string ticker = instrument?.Ticker ?? string.Empty;
            string sector = instrument?.Sector ?? string.Empty;
            string instrumentName = instrument?.Name ?? string.Empty;

            List<ReportParameter> data =
            [
                GetTicker(ticker),
                GetSector(sector),
                GetString(normalizeService.NormalizeInstrumentName(instrumentName))
            ];

            // Цвет для типов отчетов
            string color = await colorHelper.GetColorForAssetReporType(assetReportEvent.Type);
            
            foreach (var date in dates)
                data.Add(assetReportEvent.ReportDate == date
                    ? GetAssetReportEvent(string.Empty, color)
                    : GetString(string.Empty));

            reportData.Data.Add(data);
        }
        
        return reportData;
    }

    public async Task<ReportData> CreateFearGreedIndexReportDataAsync(DateOnly from, DateOnly to)
    {
        
        var feerGreedIndexes = await feerGreedRepository.GetAsync(from, to);

        var dates = DateHelper.GetDates(from, to);
        var reportData = CreateNewReportDataWithHeaders(["Параметр"], dates);
        reportData.Title = CreateTitleWithDates("Индекс страха и жадности", from, to);

        List<string> parameters = 
        [
            KnownFeedGreedIndexFields.FeerGreedIndex, 
            KnownFeedGreedIndexFields.MarketMomentum, 
            KnownFeedGreedIndexFields.MarketVolatility, 
            KnownFeedGreedIndexFields.StockPriceBreadth, 
            KnownFeedGreedIndexFields.StockPriceStrength
        ];
        
        foreach (var parameter in parameters)
        {
            List<ReportParameter> data = [GetString(parameter)];
            
            foreach (var date in dates)
            {
                // Не выводим за сегодняшний день, т.к. дневные свечи еще не подгружены
                if (date == DateOnly.FromDateTime(DateTime.Today))
                {
                    data.Add(GetString(string.Empty));
                    continue;
                }

                var feerGreedIndex = feerGreedIndexes.FirstOrDefault(x => x.Date == date);

                if (feerGreedIndex is not null)
                {
                    string color;
                    
                    switch (parameter)
                    {
                        case KnownFeedGreedIndexFields.FeerGreedIndex:
                            color = ColorHelper.RedYellowGreenScale(feerGreedIndex.Value);
                            data.Add(GetPercent(feerGreedIndex.Value, color));
                            break;
                        
                        case KnownFeedGreedIndexFields.MarketMomentum:
                            color = ColorHelper.RedYellowGreenScale(feerGreedIndex.Value);
                            data.Add(GetPercent(feerGreedIndex.MarketMomentum, color));
                            break;
                        
                        case KnownFeedGreedIndexFields.MarketVolatility:
                            color = ColorHelper.RedYellowGreenScale(feerGreedIndex.Value);
                            data.Add(GetPercent(feerGreedIndex.MarketVolatility, color));
                            break;
                        
                        case KnownFeedGreedIndexFields.StockPriceBreadth:
                            color = ColorHelper.RedYellowGreenScale(feerGreedIndex.Value);
                            data.Add(GetPercent(feerGreedIndex.StockPriceBreadth, color));
                            break;
                        
                        case KnownFeedGreedIndexFields.StockPriceStrength:
                            color = ColorHelper.RedYellowGreenScale(feerGreedIndex.Value);
                            data.Add(GetPercent(feerGreedIndex.StockPriceStrength, color));
                            break;
                        
                        default:
                            data.Add(GetString(string.Empty));
                            break;
                    }
                }

                else
                    data.Add(GetString(string.Empty));
            }
                
            reportData.Data.Add(data);
        }
            
        return reportData;
    }

    public async Task<ReportData> CreateStrategySignalsReportDataAsync()
    {
        var reportData = CreateNewReportDataWithHeaders(
            ["№", "", "Тикер", "Наименование", "Сигналы", "Тек. цена", "Позиция, шт", "Позиция (юнит), руб", ""]);

        reportData.Title = "Сигналы";
        
        var strategySignals = await strategySignalRepository.GetAllAsync();

        int count = 0;
        
        reportData.Data.Add(
        [
            GetString(string.Empty),
            GetString(string.Empty),
            GetString(string.Empty),
            GetString(string.Empty),
            GetString(string.Empty),
            GetString(string.Empty),
            GetString(string.Empty),            
            GetNumber(strategySignals.Sum(x => x.PositionCost))
        ]); 
        
        foreach (var strategySignal in strategySignals.OrderByDescending(x => x.CountSignals))
        {
            count++;
            
            var instrument = await instrumentRepository.GetAsync(strategySignal.Ticker);
            string instrumentName = instrument?.Name ?? string.Empty;           
            
            reportData.Data.Add(
            [
                GetNumber(count),
                GetTicker(strategySignal.Ticker),
                GetString(strategySignal.Ticker),
                GetString(normalizeService.NormalizeInstrumentName(instrumentName)),
                GetNumber(strategySignal.CountSignals),
                GetNumber(strategySignal.LastPrice),
                GetNumber(strategySignal.PositionSize),
                GetNumber(strategySignal.PositionCost),
                GetBacktestResultByTickerButton(strategySignal.Ticker)
            ]);            
        }
        
        return reportData;
    }

    public async Task<ReportData> CreateBacktestResultsReportDataAsync(string ticker, string strategyName)
    {
        var reportData = CreateNewReportDataWithHeaders(
            [
                "№", "Стратегия", "", "Тикер", "TF", "Параметры", "PF", "RF", 
                "Макс. пр., %", "Ср. профит, %", "Дох. год., %", "Позиций, шт", 
                "Приб. позиций, %", "Тек. позиция, шт", "Тек. позиция, руб", ""]);
        
        reportData.Title = "Результаты бэктеста";
        
        var algoConfigResource = await resourceStoreService.GetAlgoConfigAsync();
        var backtestResults = (await backtestResultRepository.GetAsync(algoConfigResource.BacktestResultFilterResource))
            .Where(x => x.Ticker == ticker)
            .Where(x => x.StrategyName == strategyName)
            .OrderByDescending(x => x.AnnualYieldReturn)
            .ToList();

        int count = 0;
        
        foreach (var backtestResult in backtestResults)
        {
            count++;
            
            reportData.Data.Add(
            [
                GetNumber(count),
                GetString(backtestResult.StrategyName),
                GetTicker(backtestResult.Ticker),
                GetString(backtestResult.Ticker),
                GetString(backtestResult.Timeframe),
                GetString(backtestResult.StrategyParams),
                GetNumber(backtestResult.ProfitFactor),
                GetNumber(backtestResult.RecoveryFactor),
                GetNumber(backtestResult.MaxDrawdownPercent),
                GetNumber(backtestResult.AverageProfitPercent),
                GetNumber(backtestResult.AnnualYieldReturn),
                GetNumber(backtestResult.NumberPositions),
                GetNumber(backtestResult.WinningTradesPercent),
                GetNumber(backtestResult.CurrentPosition),
                GetNumber(backtestResult.CurrentPositionCost),
                GetBacktestResultByIdButton(backtestResult.Id.ToString())
            ]);            
        }
        
        return reportData;
    }

    public async Task<ReportData> CreateBacktestResultReportDataAsync(Guid backtestResultId)
    {
        var reportData = CreateNewReportDataWithHeaders(
        [
            "№", "Стратегия", "", "Тикер", "TF", "Параметры", "PF", "RF", 
            "Макс. пр., %", "Ср. профит, %", "Дох. год., %", "Позиций, шт", 
            "Приб. позиций, %", "Тек. позиция, шт", "Тек. позиция, руб"]);
        
        reportData.Title = "Результаты бэктеста";
        
        var backtestResult = await backtestResultRepository.GetAsync(backtestResultId);
        
        if (backtestResult is null)
            return reportData;
        
        reportData.Data.Add(
        [
            GetNumber(1),
            GetString(backtestResult.StrategyName),
            GetTicker(backtestResult.Ticker),
            GetString(backtestResult.Ticker),
            GetString(backtestResult.Timeframe),
            GetString(backtestResult.StrategyParams),
            GetNumber(backtestResult.ProfitFactor),
            GetNumber(backtestResult.RecoveryFactor),
            GetNumber(backtestResult.MaxDrawdownPercent),
            GetNumber(backtestResult.AverageProfitPercent),
            GetNumber(backtestResult.AnnualYieldReturn),
            GetNumber(backtestResult.NumberPositions),
            GetNumber(backtestResult.WinningTradesPercent),
            GetNumber(backtestResult.CurrentPosition),
            GetNumber(backtestResult.CurrentPositionCost)
        ]); 
        
        return reportData;
    }
}