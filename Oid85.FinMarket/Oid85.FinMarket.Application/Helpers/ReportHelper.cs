using Oid85.FinMarket.Application.Models.Reports;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.External.ResourceStore;

namespace Oid85.FinMarket.Application.Helpers;

public class ReportHelper(
    IResourceStoreService resourceStoreService)
{
    public List<ReportParameter> GetDates(
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
    
    public async Task<string> GetColor(string analyseType, AnalyseResult analyseResult)
    {
        switch (analyseType)
        {
            case KnownAnalyseTypes.Supertrend:
                return await GetColorSupertrend(analyseResult.ResultString);
            
            case KnownAnalyseTypes.CandleSequence:
                return await GetColorCandleSequence(analyseResult.ResultString);
            
            case KnownAnalyseTypes.CandleVolume:
                return await GetColorCandleVolume(analyseResult.ResultString);
            
            case KnownAnalyseTypes.Rsi:
                return await GetColorRsi(analyseResult.ResultString);
            
            case KnownAnalyseTypes.YieldLtm:
                return await GetColorYieldLtm(analyseResult.ResultNumber);
            
            case KnownAnalyseTypes.DrawdownFromMaximum:
                return await GetColorDrawdownFromMaximum(analyseResult.ResultNumber);            
            
            case KnownAnalyseTypes.Aggregated:
                return await GetColorAggregated((int) analyseResult.ResultNumber);
            
            default:
                return KnownColors.White;
        }
    }

    public async Task<string> GetColorAggregated(int value)
    {
        var colorPalette = await resourceStoreService
            .GetColorPaletteAggregatedAnalyseAsync();

        var resource = colorPalette
            .FirstOrDefault(
                x => x.Value == value);
        
        if (resource is null)
            return KnownColors.White;
        
        return resource.ColorCode;
    }

    public async Task<string> GetColorYieldLtm(double value)
    {
        var colorPalette = await resourceStoreService
            .GetColorPaletteYieldLtmAsync();

        var resource = colorPalette
            .FirstOrDefault(
                x => 
                    value >= x.LowLevel && 
                    value <= x.HighLevel);
        
        if (resource is null)
            return KnownColors.White;
        
        return resource.ColorCode;
    }

    public async Task<string> GetColorDrawdownFromMaximum(double value)
    {
        var colorPalette = await resourceStoreService
            .GetColorPaletteDrawdownFromMaximumAsync();

        var resource = colorPalette
            .FirstOrDefault(
                x => 
                    value >= x.LowLevel && 
                    value <= x.HighLevel);
        
        if (resource is null)
            return KnownColors.White;
        
        return resource.ColorCode;
    }    
    
    public async Task<string> GetColorYieldCoupon(double value)
    {
        var colorPalette = await resourceStoreService
            .GetColorPaletteYieldCouponAsync();

        var resource = colorPalette
            .FirstOrDefault(
                x => 
                    value >= x.LowLevel && 
                    value <= x.HighLevel);
        
        if (resource is null)
            return KnownColors.White;
        
        return resource.ColorCode;
    }
    
    public async Task<string> GetColorYieldDividend(double value)
    {
        var colorPalette = await resourceStoreService
            .GetColorPaletteYieldDividendAsync();

        var resource = colorPalette
            .FirstOrDefault(
                x => 
                    value >= x.LowLevel && 
                    value <= x.HighLevel);
        
        if (resource is null)
            return KnownColors.White;
        
        return resource.ColorCode;
    }
    
    public async Task<string> GetColorRsi(string value)
    {
        var colorPalette = await resourceStoreService
            .GetColorPaletteRsiInterpretationAsync();

        var resource = colorPalette
            .FirstOrDefault(
                x => x.Value == value);
        
        if (resource is null)
            return KnownColors.White;
        
        return resource.ColorCode;
    }

    public async Task<string> GetColorCandleVolume(string value)
    {
        var colorPalette = await resourceStoreService
            .GetColorPaletteVolumeDirectionAsync();

        var resource = colorPalette
            .FirstOrDefault(
                x => x.Value == value);
        
        if (resource is null)
            return KnownColors.White;
        
        return resource.ColorCode;
    }

    public async Task<string> GetColorCandleSequence(string value)
    {
        var colorPalette = await resourceStoreService
            .GetColorPaletteCandleSequenceAsync();

        var resource = colorPalette
            .FirstOrDefault(
                x => x.Value == value);
        
        if (resource is null)
            return KnownColors.White;
        
        return resource.ColorCode;
    }

    public async Task<string> GetColorSupertrend(string value)
    {
        var colorPalette = await resourceStoreService
            .GetColorPaletteTrendDirectionAsync();

        var resource = colorPalette
            .FirstOrDefault(
                x => x.Value == value);
        
        if (resource is null)
            return KnownColors.White;
        
        return resource.ColorCode;
    }
    
    public async Task<string> GetColorEvToEbitda(double value)
    {
        var colorPalette = await resourceStoreService
            .GetColorPaletteEvToEbitdaAsync();

        var resource = colorPalette
            .FirstOrDefault(
                x => 
                    value >= x.LowLevel &&
                    value <= x.HighLevel);

        if (resource is null)
            return KnownColors.White;
        
        return resource.ColorCode;
    }
    
    public async Task<string> GetColorNetDebtToEbitda(double value)
    {
        var colorPalette = await resourceStoreService
            .GetColorPaletteNetDebtToEbitdaAsync();

        var resource = colorPalette
            .FirstOrDefault(
                x => 
                    value >= x.LowLevel &&
                    value <= x.HighLevel);

        if (resource is null)
            return KnownColors.White;
        
        return resource.ColorCode;
    }
}