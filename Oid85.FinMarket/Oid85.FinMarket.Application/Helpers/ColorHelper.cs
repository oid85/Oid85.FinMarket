using Oid85.FinMarket.Common.Helpers;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.External.ResourceStore;

namespace Oid85.FinMarket.Application.Helpers;

public class ColorHelper(
    IResourceStoreService resourceStoreService)
{
    public async Task<string> GetColorByAnalyseType(string analyseType, AnalyseResult analyseResult)
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
                return GetColorYieldLtm(analyseResult.ResultNumber);
            
            case KnownAnalyseTypes.DrawdownFromMaximum:
                return GetColorDrawdownFromMaximum(analyseResult.ResultNumber);            
            
            case KnownAnalyseTypes.Aggregated:
                return await GetColorAggregated((int) analyseResult.ResultNumber);
            
            default:
                return KnownColors.White;
        }
    }

    public async Task<string> GetColorAggregated(int value)
    {
        var colorPalette = await resourceStoreService.GetColorPaletteAggregatedAnalyseAsync();
        var resource = colorPalette.FirstOrDefault(x => x.Value == value);
        
        if (resource is null)
            return KnownColors.White;
        
        return resource.Color;
    }

    public string GetColorYieldLtm(double value)
    {
        double h = value > 0 ? 128.0 : 0.0;
        double step = 0.006;
        var colorHsl = ConvertHelper.HsLtoRgb(h, 1, double.Max(0.2, 0.8 - Math.Abs(value) * step));
        return ConvertHelper.RgbToHex(colorHsl.r, colorHsl.g, colorHsl.b);
    }

    public string GetColorDrawdownFromMaximum(double value)
    {
        double h = 0.0;
        double step = 0.006;
        var colorHsl = ConvertHelper.HsLtoRgb(h, 1, double.Max(0.2, 0.8 - Math.Abs(value) * step));
        return ConvertHelper.RgbToHex(colorHsl.r, colorHsl.g, colorHsl.b);
    }    
    
    public async Task<string> GetColorYieldCoupon(double value)
    {
        var colorPalette = await resourceStoreService.GetColorPaletteYieldCouponAsync();
        var resource = colorPalette.FirstOrDefault(x => value >= x.LowLevel && value <= x.HighLevel);
        
        if (resource is null)
            return KnownColors.White;
        
        return resource.Color;
    }
    
    public async Task<string> GetColorYieldDividend(double value)
    {
        var colorPalette = await resourceStoreService.GetColorPaletteYieldDividendAsync();
        var resource = colorPalette.FirstOrDefault(x => value >= x.LowLevel && value <= x.HighLevel);
        
        if (resource is null)
            return KnownColors.White;
        
        return resource.Color;
    }
    
    public async Task<string> GetColorRsi(string value)
    {
        var colorPalette = await resourceStoreService.GetColorPaletteRsiInterpretationAsync();
        var resource = colorPalette.FirstOrDefault(x => x.Value == value);
        
        if (resource is null)
            return KnownColors.White;
        
        return resource.Color;
    }

    public async Task<string> GetColorCandleVolume(string value)
    {
        var colorPalette = await resourceStoreService.GetColorPaletteVolumeDirectionAsync();
        var resource = colorPalette.FirstOrDefault(x => x.Value == value);
        
        if (resource is null)
            return KnownColors.White;
        
        return resource.Color;
    }

    public async Task<string> GetColorCandleSequence(string value)
    {
        var colorPalette = await resourceStoreService.GetColorPaletteCandleSequenceAsync();
        var resource = colorPalette.FirstOrDefault(x => x.Value == value);
        
        if (resource is null)
            return KnownColors.White;
        
        return resource.Color;
    }

    public async Task<string> GetColorSupertrend(string value)
    {
        var colorPalette = await resourceStoreService.GetColorPaletteTrendDirectionAsync();
        var resource = colorPalette.FirstOrDefault(x => x.Value == value);
        
        if (resource is null)
            return KnownColors.White;
        
        return resource.Color;
    }
    
    public async Task<string> GetColorEvToEbitda(double value)
    {
        var colorPalette = await resourceStoreService.GetColorPaletteEvToEbitdaAsync();
        var resource = colorPalette.FirstOrDefault(x => value >= x.LowLevel && value <= x.HighLevel);

        if (resource is null)
            return KnownColors.White;
        
        return resource.Color;
    }
    
    public async Task<string> GetColorNetDebtToEbitda(double value)
    {
        var colorPalette = await resourceStoreService.GetColorPaletteNetDebtToEbitdaAsync();
        var resource = colorPalette.FirstOrDefault(x => value >= x.LowLevel && value <= x.HighLevel);

        if (resource is null)
            return KnownColors.White;
        
        return resource.Color;
    }
    
    public async Task<string> GetColorForecastRecommendation(string value)
    {
        var colorPalette = await resourceStoreService.GetColorPaletteForecastRecommendationAsync();
        var resource = colorPalette.FirstOrDefault(x => x.Value == value);
        
        if (resource is null)
            return KnownColors.White;
        
        return resource.Color;
    }
    
    public async Task<string> GetColorSpreadPricePosition(string value)
    {
        var colorPalette = await resourceStoreService.GetColorPaletteSpreadPricePositionAsync();
        var resource = colorPalette.FirstOrDefault(x => x.Value == value);
        
        if (resource is null)
            return KnownColors.White;
        
        return resource.Color;
    }
    
    public async Task<string> GetColorMarketEvent(string value)
    {
        var colorPalette = await resourceStoreService.GetColorPaletteMarketEventAsync();
        var resource = colorPalette.FirstOrDefault(x => x.Value == value);
        
        if (resource is null)
            return KnownColors.White;
        
        return resource.Color;
    }
    
    public async Task<string> GetColorRiskLevel(string value)
    {
        var colorPalette = await resourceStoreService.GetColorPaletteRiskLevelAsync();
        var resource = colorPalette.FirstOrDefault(x => x.Value == value);
        
        if (resource is null)
            return KnownColors.White;
        
        return resource.Color;
    }

    public string GetColorForForecastPrice(double price, double minTarget, double maxTarget)
    {
        if (price >= minTarget && price <= maxTarget)
            return KnownColors.Green;
        
        return KnownColors.White;
    }
}