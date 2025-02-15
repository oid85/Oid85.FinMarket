using System.Text.Json;
using Microsoft.Extensions.Configuration;
using NLog;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.External.ResourceStore.Models;

namespace Oid85.FinMarket.External.ResourceStore;

/// <inheritdoc />
public class ResourceStoreService(
    ILogger logger,
    IConfiguration configuration) 
    : IResourceStoreService
{
    /// <inheritdoc />
    public async Task<List<string>> GetSharesWatchlistAsync()
    {
        string path = Path.Combine(
            configuration.GetValue<string>(KnownSettingsKeys.ResourceStorePath)!,
            "watchLists",
            "shares.json");

        var result = await ReadAsync<List<string>>(path);

        if (result is null)
            return [];
        
        return result;
    }

    /// <inheritdoc />
    public async Task<List<string>> GetBondsWatchlistAsync()
    {
        string path = Path.Combine(
            configuration.GetValue<string>(KnownSettingsKeys.ResourceStorePath)!,
            "watchLists",
            "bonds.json");

        var result = await ReadAsync<List<string>>(path);

        if (result is null)
            return [];
        
        return result;
    }

    /// <inheritdoc />
    public async Task<List<string>> GetFuturesWatchlistAsync()
    {
        string path = Path.Combine(
            configuration.GetValue<string>(KnownSettingsKeys.ResourceStorePath)!,
            "watchLists",
            "futures.json");

        var result = await ReadAsync<List<string>>(path);

        if (result is null)
            return [];
        
        return result;
    }

    /// <inheritdoc />
    public async Task<List<string>> GetCurrenciesWatchlistAsync()
    {
        string path = Path.Combine(
            configuration.GetValue<string>(KnownSettingsKeys.ResourceStorePath)!,
            "watchLists",
            "currencies.json");

        var result = await ReadAsync<List<string>>(path);

        if (result is null)
            return [];
        
        return result;
    }

    /// <inheritdoc />
    public async Task<List<string>> GetIndexesWatchlistAsync()
    {
        string path = Path.Combine(
            configuration.GetValue<string>(KnownSettingsKeys.ResourceStorePath)!,
            "watchLists",
            "indexes.json");

        var result = await ReadAsync<List<string>>(path);

        if (result is null)
            return [];
        
        return result;
    }

    /// <inheritdoc />
    public async Task<List<string>> GetIMoexTickersAsync()
    {
        string path = Path.Combine(
            configuration.GetValue<string>(KnownSettingsKeys.ResourceStorePath)!,
            "tickerLists",
            "imoex.json");

        var result = await ReadAsync<List<string>>(path);

        if (result is null)
            return [];
        
        return result;
    }

    /// <inheritdoc />
    public async Task<List<MultiplicatorResource>> GetMultiplicatorsLtmAsync()
    {
        string directoryPath = Path.Combine(
            configuration.GetValue<string>(KnownSettingsKeys.ResourceStorePath)!,
            "multiplicators",
            "ltm");

        var filePathes = Directory.GetFiles(directoryPath);

        var resources = new List<MultiplicatorResource>();

        foreach (var filePath in filePathes)
        {
            var resource = await ReadAsync<MultiplicatorResource>(filePath);
            
            if (resource is not null) 
                resources.Add(resource);
        }

        return resources;
    }

    /// <inheritdoc />
    public async Task<List<PriceLevelResource>> GetPriceLevelsAsync(string ticker)
    {
        string path = Path.Combine(
            configuration.GetValue<string>(KnownSettingsKeys.ResourceStorePath)!,
            "priceLevels",
            $"{ticker.ToLower()}.json");

        var result = await ReadAsync<List<PriceLevelResource>>(path);

        if (result is null)
            return [];
        
        return result;
    }

    /// <inheritdoc />
    public async Task<List<FutureCodeResource>> GetFutureCodesAsync()
    {
        string path = Path.Combine(
            configuration.GetValue<string>(KnownSettingsKeys.ResourceStorePath)!,
            "futureCodes.json");

        var result = await ReadAsync<List<FutureCodeResource>>(path);

        if (result is null)
            return [];
        
        return result;
    }

    /// <inheritdoc />
    public async Task<List<DateValueResource<double>>> GetKeyRatesAsync()
    {
        string path = Path.Combine(
            configuration.GetValue<string>(KnownSettingsKeys.ResourceStorePath)!,
            "keyRates.json");

        var result = await ReadAsync<List<DateValueResource<double>>>(path);

        if (result is null)
            return [];
        
        return result;
    }

    /// <inheritdoc />
    public async Task<List<SpreadResource>> GetSpreadsAsync()
    {
        string path = Path.Combine(
            configuration.GetValue<string>(KnownSettingsKeys.ResourceStorePath)!,
            "spreads.json");

        var result = await ReadAsync<List<SpreadResource>>(path);

        if (result is null)
            return [];
        
        return result;
    }

    /// <inheritdoc />
    public async Task<List<ValueColorResource<int>>> GetColorPaletteAggregatedAnalyseAsync()
    {
        string path = Path.Combine(
            configuration.GetValue<string>(KnownSettingsKeys.ResourceStorePath)!,
            "colorPalettes",
            "aggregatedAnalyse.json");

        var result = await ReadAsync<List<ValueColorResource<int>>>(path);

        if (result is null)
            return [];
        
        return result;
    }

    /// <inheritdoc />
    public async Task<List<ValueColorResource<string>>> GetColorPaletteCandleSequenceAsync()
    {
        string path = Path.Combine(
            configuration.GetValue<string>(KnownSettingsKeys.ResourceStorePath)!,
            "colorPalettes",
            "candleSequence.json");

        var result = await ReadAsync<List<ValueColorResource<string>>>(path);

        if (result is null)
            return [];
        
        return result;
    }

    /// <inheritdoc />
    public async Task<List<ValueColorResource<string>>> GetColorPaletteRsiInterpretationAsync()
    {
        string path = Path.Combine(
            configuration.GetValue<string>(KnownSettingsKeys.ResourceStorePath)!,
            "colorPalettes",
            "rsiInterpretation.json");

        var result = await ReadAsync<List<ValueColorResource<string>>>(path);

        if (result is null)
            return [];
        
        return result;
    }

    /// <inheritdoc />
    public async Task<List<ValueColorResource<string>>> GetColorPaletteTrendDirectionAsync()
    {
        string path = Path.Combine(
            configuration.GetValue<string>(KnownSettingsKeys.ResourceStorePath)!,
            "colorPalettes",
            "trendDirection.json");

        var result = await ReadAsync<List<ValueColorResource<string>>>(path);

        if (result is null)
            return [];
        
        return result;
    }

    /// <inheritdoc />
    public async Task<List<ValueColorResource<string>>> GetColorPaletteVolumeDirectionAsync()
    {
        string path = Path.Combine(
            configuration.GetValue<string>(KnownSettingsKeys.ResourceStorePath)!,
            "colorPalettes",
            "volumeDirection.json");

        var result = await ReadAsync<List<ValueColorResource<string>>>(path);

        if (result is null)
            return [];
        
        return result;
    }

    /// <inheritdoc />
    public async Task<List<RangeColorResource>> GetColorPaletteYieldDividendAsync()
    {
        string path = Path.Combine(
            configuration.GetValue<string>(KnownSettingsKeys.ResourceStorePath)!,
            "colorPalettes",
            "limits",
            "yieldDividendLimits.json");

        var result = await ReadAsync<List<RangeColorResource>>(path);

        if (result is null)
            return [];
        
        return result;
    }

    /// <inheritdoc />
    public async Task<List<RangeColorResource>> GetColorPaletteYieldCouponAsync()
    {
        string path = Path.Combine(
            configuration.GetValue<string>(KnownSettingsKeys.ResourceStorePath)!,
            "colorPalettes",
            "limits",
            "yieldCouponLimits.json");

        var result = await ReadAsync<List<RangeColorResource>>(path);

        if (result is null)
            return [];
        
        return result;
    }

    /// <inheritdoc />
    public async Task<List<RangeColorResource>> GetColorPaletteYieldLtmAsync()
    {
        string path = Path.Combine(
            configuration.GetValue<string>(KnownSettingsKeys.ResourceStorePath)!,
            "colorPalettes",
            "limits",
            "yieldLtmLimits.json");

        var result = await ReadAsync<List<RangeColorResource>>(path);

        if (result is null)
            return [];
        
        return result;
    }

    public async Task<List<RangeColorResource>> GetColorPalettePeAsync()
    {
        string path = Path.Combine(
            configuration.GetValue<string>(KnownSettingsKeys.ResourceStorePath)!,
            "colorPalettes",
            "limits",
            "peLimits.json");

        var result = await ReadAsync<List<RangeColorResource>>(path);

        if (result is null)
            return [];
        
        return result;
    }

    public async Task<List<RangeColorResource>> GetColorPaletteEvToEbitdaAsync()
    {
        string path = Path.Combine(
            configuration.GetValue<string>(KnownSettingsKeys.ResourceStorePath)!,
            "colorPalettes",
            "limits",
            "evEbitdaLimits.json");

        var result = await ReadAsync<List<RangeColorResource>>(path);

        if (result is null)
            return [];
        
        return result;
    }

    public async Task<List<RangeColorResource>> GetColorPaletteNetDebtToEbitdaAsync()
    {
        string path = Path.Combine(
            configuration.GetValue<string>(KnownSettingsKeys.ResourceStorePath)!,
            "colorPalettes",
            "limits",
            "netDebtEbitdaLimits.json");

        var result = await ReadAsync<List<RangeColorResource>>(path);

        if (result is null)
            return [];
        
        return result;
    }

    /// <inheritdoc />
    public async Task<List<ValueColorResource<string>>> GetColorPaletteSpreadPricePositionAsync()
    {
        string path = Path.Combine(
            configuration.GetValue<string>(KnownSettingsKeys.ResourceStorePath)!,
            "colorPalettes",
            "spreadPricePosition.json");

        var result = await ReadAsync<List<ValueColorResource<string>>>(path);

        if (result is null)
            return [];
        
        return result;
    }

    /// <inheritdoc />
    public async Task<List<ValueColorResource<string>>> GetColorPaletteForecastRecommendationAsync()
    {
        string path = Path.Combine(
            configuration.GetValue<string>(KnownSettingsKeys.ResourceStorePath)!,
            "colorPalettes",
            "forecastRecommendation.json");

        var result = await ReadAsync<List<ValueColorResource<string>>>(path);

        if (result is null)
            return [];
        
        return result;
    }

    public async Task<FilterBondsResource?> GetFilterBondsResourceAsync()
    {
        string path = Path.Combine(
            configuration.GetValue<string>(KnownSettingsKeys.ResourceStorePath)!,
            "filters",
            "filterBonds.json");

        var result = await ReadAsync<FilterBondsResource>(path);
        
        return result;
    }

    private async Task<T?> ReadAsync<T>(string path)
    {
        if (!File.Exists(path))
        {
            logger.Error(@$"Файл не существует. {path}");
            return default;
        }

        await using var stream = File.OpenRead(path);
        return await JsonSerializer.DeserializeAsync<T>(stream);
    }
}