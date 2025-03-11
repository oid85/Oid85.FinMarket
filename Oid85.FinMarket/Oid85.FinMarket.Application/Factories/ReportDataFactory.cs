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
    IBondCouponRepository bondCouponRepository,
    IBondRepository bondRepository,
    IShareRepository shareRepository,
    ICandleRepository candleRepository,
    IMultiplicatorRepository multiplicatorRepository,
    IForecastTargetRepository forecastTargetRepository,
    IForecastConsensusRepository forecastConsensusRepository,
    ColorHelper colorHelper,
    IInstrumentService instrumentService,
    ISpreadRepository spreadRepository,
    IMarketEventRepository marketEventRepository,
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

    private static List<DateOnly> GetDates(DateOnly from, DateOnly to)
    {
        var curDate = from;
        var dates = new List<DateOnly>();

        while (curDate <= to)
        {
            dates.Add(curDate);
            curDate = curDate.AddDays(1);
        }

        return dates;
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
    
    private static double CalculateNextBondCouponProfitPercent(Bond bond, BondCoupon? coupon)
    {
        if (coupon is null)
            return 0.0;

        if (bond.LastPrice == 0.0)
            return 0.0;
        
        if (coupon.CouponPeriod == 0)
            return 0.0;

        double yieldOfNextCoupon = coupon.PayOneBond / (bond.LastPrice * 10.0 + bond.Nkd) * 100.0;
        int daysToCouponDate = (coupon.CouponDate.ToDateTime(TimeOnly.MinValue) - DateTime.Today).Days;
        
        if (daysToCouponDate == 0)
            return 0.0;
        
        double yieldOfNextCouponByYear = yieldOfNextCoupon / daysToCouponDate * 365.0;

        return yieldOfNextCouponByYear;
    }
    
    private async ValueTask<double> CalculateDividendProfitPercentAsync(DividendInfo dividendInfo)
    {
        var share = await shareRepository.GetByInstrumentIdAsync(dividendInfo.InstrumentId);

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
        new (KnownDisplayTypes.Ruble, value.ToString("N2"), color);     
    
    private static ReportParameter GetPercent(double value, string color = KnownColors.White) =>
        new (KnownDisplayTypes.Percent, value.ToString("N2"), color);     
    
    private static ReportParameter GetNumber(double value, string color = KnownColors.White) =>
        new (KnownDisplayTypes.Number, value.ToString("N2"), color);
    
    private static ReportParameter GetCurrency(string value, string color = KnownColors.White) =>
        new (KnownDisplayTypes.Currency, value, color);  
    
    private static ReportParameter GetAnalyseResult(string value, string color = KnownColors.White) =>
        new (KnownDisplayTypes.AnalyseResult, value, color);    
    
    public async Task<ReportData> CreateReportDataAsync(
        List<Guid> instrumentIds, string analyseType, DateOnly from, DateOnly to)
    {
        var analyseResults = await GetAnalyseResults(
            instrumentIds, [analyseType], from, to);

        var dates = GetDates(from, to);
        var reportData = CreateNewReportDataWithHeaders(["Тикер", "Сектор", "Эмитент"], dates);
        reportData.Title = CreateTitleWithDates(analyseType, from, to);
        
        foreach (var instrumentId in instrumentIds)
        {
            var instrumentAnalyseResults = analyseResults
                .Where(x => x.InstrumentId == instrumentId).ToList();
            
            if (instrumentAnalyseResults is [])
                continue;
            
            var instrument = await instrumentRepository.GetByInstrumentIdAsync(instrumentId);
            
            if (instrument is null)
                continue;
            
            var data = new List<ReportParameter>
            {
                GetTicker(instrument.Ticker), 
                GetSector(instrument.Sector),
                GetString(normalizeService.NormalizeInstrumentName(instrument.Name))
            };
            
            foreach (var date in dates)
            {
                var analyseResult = instrumentAnalyseResults.FirstOrDefault(x => x.Date == date);

                if (analyseResult is not null)
                {
                    string color = await colorHelper.GetColorByAnalyseType(analyseType, analyseResult);

                    data.Add(analyseType is KnownAnalyseTypes.YieldLtm or KnownAnalyseTypes.DrawdownFromMaximum
                        ? GetPercent(analyseResult.ResultNumber, color)
                        : GetAnalyseResult(string.Empty, color));
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
            
        var dates = GetDates(from, to);
        var reportData = CreateNewReportDataWithHeaders(["Тикер", "Сектор", "Эмитент"], dates);
        reportData.Title = reportData.Title = CreateTitleWithDates("Aggregated", from, to);

        foreach (var instrumentId in instrumentIds)
        {
            var instrumentAnalyseResults = analyseResults
                .Where(x => x.InstrumentId == instrumentId).ToList();
            
            if (instrumentAnalyseResults is [])
                continue;
            
            var instrument = await instrumentRepository.GetByInstrumentIdAsync(instrumentId);
            
            if (instrument is null)
                continue;
            
            var data = new List<ReportParameter>
            {
                GetTicker(instrument.Ticker), 
                GetSector(instrument.Sector),
                GetString(normalizeService.NormalizeInstrumentName(instrument.Name))
            };

            foreach (var date in dates)
            {
                var instrumentAnalyseResultsByDate = instrumentAnalyseResults
                    .Where(x => x.Date == date).ToList();

                int resultNumber = CalculateAggregateAnalyseResult(instrumentAnalyseResultsByDate);
                string color = await colorHelper.GetColorAggregated(resultNumber);
                var candle = await candleRepository.GetAsync(instrumentId, date);
                string price = candle?.Close.ToString("N2") ?? string.Empty;
                data.Add(GetAnalyseResult(price, color));
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
        var dates = GetDates(from, to);
        
        var reportData = CreateNewReportDataWithHeaders(
            ["Тикер", "Эмитент", "Фикс. р.", "Объяв.", "Размер, руб", "Дох-ть, %", "Тек. дох-ть, %"], dates);
        
        foreach (var dividendInfo in dividendInfos)
        {
            var instrument = await instrumentRepository.GetByInstrumentIdAsync(dividendInfo.InstrumentId);
            var profitPrc = await CalculateDividendProfitPercentAsync(dividendInfo);
            string instrumentName = instrument?.Name ?? string.Empty;
            string color = await colorHelper.GetColorYieldDividend(profitPrc);

            var data = new List<ReportParameter>
            {
                GetTicker(dividendInfo.Ticker),
                GetString(normalizeService.NormalizeInstrumentName(instrumentName)),
                GetDate(dividendInfo.RecordDate),
                GetDate(dividendInfo.DeclaredDate),
                GetRuble(dividendInfo.Dividend),
                GetPercent(dividendInfo.DividendPrc, color),
                GetPercent(profitPrc, color)
            };
            
            foreach (var date in dates)
                data.Add(dividendInfo.RecordDate == date
                    ? GetPercent(profitPrc, color)
                    : GetString(string.Empty));

            reportData.Data.Add(data);
        }
        
        return reportData;
    }

    public async Task<ReportData> CreateMultiplicatorReportDataAsync()
    {
        var shares = (await instrumentService.GetSharesInWatchlist())
            .OrderBy(x => x.Sector);

        var reportData = CreateNewReportDataWithHeaders(
            [
                "Тикер", "Сектор", "Эмитент", "Рыноч. кап.", "Бета-коэфф.", "Чист. приб.", 
                "EBITDA", "EPS", "Своб. ден. поток", "EV/EBITDA", "Total Debt/EBITDA", "Net Debt/EBITDA"
            ]);

        reportData.Title = "Мультипликаторы";
        
        foreach (var share in shares)
        {
            var multiplicator = await multiplicatorRepository.GetAsync(share.Ticker);
            
            if (multiplicator is null)
                continue;

            string colorEvToEbitda = await colorHelper.GetColorEvToEbitda(multiplicator.EvToEbitda);
            string colorNetDebtToEbitda = await colorHelper.GetColorNetDebtToEbitda(multiplicator.NetDebtToEbitda);
            
            List<ReportParameter> data =
            [
                GetTicker(share.Ticker),
                GetSector(share.Sector),
                GetString(normalizeService.NormalizeInstrumentName(share.Name)),
                GetNumber(multiplicator.MarketCapitalization),
                GetNumber(multiplicator.Beta),
                GetNumber(multiplicator.NetIncome),
                GetNumber(multiplicator.Ebitda),
                GetNumber(multiplicator.Eps),
                GetNumber(multiplicator.FreeCashFlow),
                GetNumber(multiplicator.EvToEbitda, colorEvToEbitda),
                GetNumber(multiplicator.TotalDebtToEbitda),
                GetNumber(multiplicator.NetDebtToEbitda, colorNetDebtToEbitda)
            ];
            
            reportData.Data.Add(data);
        }
        
        return reportData;
    }

    public async Task<ReportData> CreateForecastTargetReportDataAsync()
    {
        var forecastTargets = (await forecastTargetRepository.GetAllAsync())
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

    public async Task<ReportData> CreateForecastConsensusReportDataAsync()
    {
        var forecastConsensuses = (await forecastConsensusRepository.GetAllAsync())
            .Where(x => !string.IsNullOrEmpty(x.Ticker)).ToList();
        
        var reportData = CreateNewReportDataWithHeaders(
        [
            "Тикер", "Эмитент", "Прогноз", "Валюта", "Тек. цена", "Мин. цена прогноза", 
            "Макс. цена прогноза", "Изм. цены", "Отн. изм. цены"
        ]);

        reportData.Title = "Консенсус-прогнозы";

        foreach (var forecastConsensus in forecastConsensuses)
        {
            var instrument = await instrumentRepository.GetByTickerAsync(forecastConsensus.Ticker);

            string colorRecommendation = await colorHelper.GetColorForecastRecommendation(forecastConsensus.RecommendationString);

            string colorCurrentPrice = colorHelper.GetColorForForecastPrice(
                forecastConsensus.CurrentPrice, forecastConsensus.MinTarget, forecastConsensus.MaxTarget);
            
            reportData.Data.Add(
            [
                GetTicker(forecastConsensus.Ticker),
                GetString(normalizeService.NormalizeInstrumentName(instrument?.Name ?? string.Empty)),
                GetString(forecastConsensus.RecommendationString, colorRecommendation),
                GetCurrency(forecastConsensus.Currency),
                GetRuble(forecastConsensus.CurrentPrice, colorCurrentPrice),
                GetRuble(forecastConsensus.MinTarget),
                GetRuble(forecastConsensus.MaxTarget),
                GetRuble(forecastConsensus.PriceChange),
                GetPercent(forecastConsensus.PriceChangeRel)
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
        var dates = GetDates(startDate, endDate);
        
        var reportData = CreateNewReportDataWithHeaders(
            [
                "Тикер", "Сектор", "Наименование", "Уровень риска", "Валюта", "Плав. купон", 
                "До погаш.", "Цена", "НКД", "Куп. период", "Дох-ть куп.", "Дох-ть след. куп."
            ], dates);
        
        reportData.Title = "Купоны";
        
        var bondCoupons = (await bondCouponRepository
            .GetByInstrumentIdsAsync(instrumentIds))
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
            var profitPrcNextCoupon = CalculateNextBondCouponProfitPercent(bond, bondCouponsByInstrument.FirstOrDefault());
            
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
            
            string colorNextCoupon = await colorHelper.GetColorYieldCoupon(profitPrcNextCoupon);
            data.Add(GetPercent(profitPrcNextCoupon, colorNextCoupon));   
            
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

    public async Task<ReportData> CreateSpreadReportDataAsync()
    {
        var spreads = await spreadRepository.GetAllAsync();
            
        var reportData = CreateNewReportDataWithHeaders(
            ["1", "2", "Тикер 1", "Тикер 2", "Цена 1", "Цена 2", "Спред", "Спред, %", "Конт./Бэкв."]);
        
        reportData.Title = "Спреды";

        foreach (var spread in spreads)
        {
            string color = await colorHelper.GetColorSpreadPricePosition(spread.SpreadPricePosition);
            
            reportData.Data.Add(
            [
                GetString(spread.FirstInstrumentRole),
                GetString(spread.SecondInstrumentRole),
                GetTicker(spread.FirstInstrumentTicker),
                GetTicker(spread.SecondInstrumentTicker),
                GetRuble(spread.FirstInstrumentPrice),
                GetRuble(spread.SecondInstrumentPrice),
                GetRuble(spread.PriceDifference),
                GetPercent(spread.PriceDifferencePrc),
                GetString(spread.SpreadPricePosition, color)
            ]);
        }
        
        return reportData;
    }
    
    public async Task<ReportData> CreateActiveMarketEventsReportDataAsync(List<Guid> instrumentIds)
    {
        var marketEvents = (await marketEventRepository.GetActivatedAsync())
            .Where(x => instrumentIds.Contains(x.InstrumentId))
            .OrderBy(x => x.Ticker);
        
        var reportData = CreateNewReportDataWithHeaders(
            ["Тикер", "Сектор", "Наименование", "Дата", "Время", "Текст"]);
        
        reportData.Title = "Активные рыночные события";
        
        foreach (var marketEvent in marketEvents)
        {
            var instrument = await instrumentRepository.GetByInstrumentIdAsync(marketEvent.InstrumentId);
            
            if (instrument is null)
                continue;
            
            string color = await colorHelper.GetColorMarketEvent(marketEvent.MarketEventType);
            
            reportData.Data.Add(
            [
                GetTicker(instrument.Ticker),
                GetSector(instrument.Sector),
                GetTicker(normalizeService.NormalizeInstrumentName(marketEvent.InstrumentName), color),
                GetString(marketEvent.Date.ToString(KnownDateTimeFormats.DateISO), color),
                GetString(marketEvent.Time.ToString(KnownDateTimeFormats.TimeISO), color),
                GetString(marketEvent.MarketEventText, color)
            ]);
        }
        
        return reportData;
    }
}