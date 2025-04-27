using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.External.ResourceStore.Models;

namespace Oid85.FinMarket.External.ResourceStore;

/// <inheritdoc />
public class ResourceStoreService(
    IConfiguration configuration) 
    : IResourceStoreService
{
    /// <inheritdoc />
    public async Task<List<string>> GetSharesWatchlistAsync() =>
        await ReadAsync<List<string>>(
            Path.Combine(configuration.GetValue<string>(KnownSettingsKeys.ResourceStorePath)!,
            "watchLists", "shares.json")) ?? [];

    /// <inheritdoc />
    public async Task<List<string>> GetBondsWatchlistAsync() =>
        await ReadAsync<List<string>>(
            Path.Combine(configuration.GetValue<string>(KnownSettingsKeys.ResourceStorePath)!,
            "watchLists", "bonds.json")) ?? [];

    /// <inheritdoc />
    public async Task<List<string>> GetFuturesWatchlistAsync() =>
        await ReadAsync<List<string>>(
            Path.Combine(configuration.GetValue<string>(KnownSettingsKeys.ResourceStorePath)!,
            "watchLists", "futures.json")) ?? [];

    /// <inheritdoc />
    public async Task<List<string>> GetCurrenciesWatchlistAsync() =>
        await ReadAsync<List<string>>(
            Path.Combine(configuration.GetValue<string>(KnownSettingsKeys.ResourceStorePath)!,
            "watchLists", "currencies.json")) ?? [];

    /// <inheritdoc />
    public async Task<List<string>> GetIndexesWatchlistAsync() =>
        await ReadAsync<List<string>>(
            Path.Combine(configuration.GetValue<string>(KnownSettingsKeys.ResourceStorePath)!,
            "watchLists", "indexes.json")) ?? [];

    /// <inheritdoc />
    public async Task<List<string>> GetIndexMoexTickersAsync() =>
        await ReadAsync<List<string>>(
            Path.Combine(configuration.GetValue<string>(KnownSettingsKeys.ResourceStorePath)!,
            "tickerLists", "imoex.json")) ?? [];

    /// <inheritdoc />
    public async Task<List<MultiplicatorResource>> GetMultiplicatorsLtmAsync()
    {
        string directoryPath = Path.Combine(
            configuration.GetValue<string>(KnownSettingsKeys.ResourceStorePath)!,
            "multiplicators");

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
    public async Task<List<PriceLevelResource>> GetPriceLevelsAsync(string ticker) =>
        await ReadAsync<List<PriceLevelResource>>(
            Path.Combine(configuration.GetValue<string>(KnownSettingsKeys.ResourceStorePath)!,
            "priceLevels", $"{ticker.ToLower()}.json")) ?? [];

    /// <inheritdoc />
    public async Task<List<FutureCodeResource>> GetFutureCodesAsync() =>
        await ReadAsync<List<FutureCodeResource>>(
            Path.Combine(configuration.GetValue<string>(KnownSettingsKeys.ResourceStorePath)!,
            "futureCodes.json")) ?? [];

    /// <inheritdoc />
    public async Task<List<DateValueResource<double>>> GetKeyRatesAsync() =>
        await ReadAsync<List<DateValueResource<double>>>(
            Path.Combine(configuration.GetValue<string>(KnownSettingsKeys.ResourceStorePath)!,
                "keyRates.json")) ?? [];

    /// <inheritdoc />
    public async Task<List<SpreadResource>> GetSpreadsAsync() =>
        await ReadAsync<List<SpreadResource>>(
            Path.Combine(configuration.GetValue<string>(KnownSettingsKeys.ResourceStorePath)!,
                "spreads.json")) ?? [];

    /// <inheritdoc />
    public async Task<List<ValueColorResource<int>>> GetColorPaletteAggregatedAnalyseAsync() =>
        await ReadAsync<List<ValueColorResource<int>>>(
            Path.Combine(configuration.GetValue<string>(KnownSettingsKeys.ResourceStorePath)!,
                "colorPalettes", "aggregatedAnalyse.json")) ?? [];

    /// <inheritdoc />
    public async Task<List<ValueColorResource<string>>> GetColorPaletteCandleSequenceAsync() =>
        await ReadAsync<List<ValueColorResource<string>>>(
            Path.Combine(configuration.GetValue<string>(KnownSettingsKeys.ResourceStorePath)!,
                "colorPalettes", "candleSequence.json")) ?? [];

    /// <inheritdoc />
    public async Task<List<ValueColorResource<string>>> GetColorPaletteRsiInterpretationAsync() =>
        await ReadAsync<List<ValueColorResource<string>>>(
            Path.Combine(configuration.GetValue<string>(KnownSettingsKeys.ResourceStorePath)!,
                "colorPalettes", "rsiInterpretation.json")) ?? [];

    /// <inheritdoc />
    public async Task<List<ValueColorResource<string>>> GetColorPaletteTrendDirectionAsync() =>
        await ReadAsync<List<ValueColorResource<string>>>(
            Path.Combine(configuration.GetValue<string>(KnownSettingsKeys.ResourceStorePath)!,
                "colorPalettes", "trendDirection.json")) ?? [];

    /// <inheritdoc />
    public async Task<List<ValueColorResource<string>>> GetColorPaletteVolumeDirectionAsync() =>
        await ReadAsync<List<ValueColorResource<string>>>(
            Path.Combine(configuration.GetValue<string>(KnownSettingsKeys.ResourceStorePath)!,
                "colorPalettes", "volumeDirection.json")) ?? [];

    /// <inheritdoc />
    public async Task<List<RangeColorResource>> GetColorPaletteYieldDividendAsync() =>
        await ReadAsync<List<RangeColorResource>>(
            Path.Combine(configuration.GetValue<string>(KnownSettingsKeys.ResourceStorePath)!,
                "colorPalettes", "limits", "yieldDividendLimits.json")) ?? [];

    /// <inheritdoc />
    public async Task<List<RangeColorResource>> GetColorPaletteYieldCouponAsync() =>
        await ReadAsync<List<RangeColorResource>>(
            Path.Combine(configuration.GetValue<string>(KnownSettingsKeys.ResourceStorePath)!,
                "colorPalettes", "limits", "yieldCouponLimits.json")) ?? [];
    
    /// <inheritdoc />
    public async Task<List<RangeColorResource>> GetColorPalettePeAsync() =>
        await ReadAsync<List<RangeColorResource>>(
            Path.Combine(configuration.GetValue<string>(KnownSettingsKeys.ResourceStorePath)!,
                "colorPalettes", "limits", "peLimits.json")) ?? [];

    /// <inheritdoc />
    public async Task<List<RangeColorResource>> GetColorPaletteEvToEbitdaAsync() =>
        await ReadAsync<List<RangeColorResource>>(
            Path.Combine(configuration.GetValue<string>(KnownSettingsKeys.ResourceStorePath)!,
                "colorPalettes", "limits", "evEbitdaLimits.json")) ?? [];

    /// <inheritdoc />
    public async Task<List<RangeColorResource>> GetColorPaletteNetDebtToEbitdaAsync() =>
        await ReadAsync<List<RangeColorResource>>(
            Path.Combine(configuration.GetValue<string>(KnownSettingsKeys.ResourceStorePath)!,
                "colorPalettes", "limits", "netDebtEbitdaLimits.json")) ?? [];

    /// <inheritdoc />
    public async Task<List<RangeColorResource>> GetColorPaletteTotalDebtToEbitdaAsync() =>
        await ReadAsync<List<RangeColorResource>>(
            Path.Combine(configuration.GetValue<string>(KnownSettingsKeys.ResourceStorePath)!,
                "colorPalettes", "limits", "totalDebtEbitdaLimits.json")) ?? [];

    /// <inheritdoc />
    public async Task<List<ValueColorResource<string>>> GetColorPaletteSpreadPricePositionAsync() =>
        await ReadAsync<List<ValueColorResource<string>>>(
            Path.Combine(configuration.GetValue<string>(KnownSettingsKeys.ResourceStorePath)!,
                "colorPalettes", "spreadPricePosition.json")) ?? [];

    /// <inheritdoc />
    public async Task<List<ValueColorResource<string>>> GetColorPaletteForecastRecommendationAsync() =>
        await ReadAsync<List<ValueColorResource<string>>>(
            Path.Combine(configuration.GetValue<string>(KnownSettingsKeys.ResourceStorePath)!,
            "colorPalettes", "forecastRecommendation.json")) ?? [];

    /// <inheritdoc />
    public async Task<List<ValueColorResource<string>>> GetColorPaletteMarketEventAsync() =>
        await ReadAsync<List<ValueColorResource<string>>>(
            Path.Combine(configuration.GetValue<string>(KnownSettingsKeys.ResourceStorePath)!,
                "colorPalettes", "marketEvent.json")) ?? [];

    /// <inheritdoc />
    public async Task<List<ValueColorResource<string>>> GetColorPaletteRiskLevelAsync() =>
        await ReadAsync<List<ValueColorResource<string>>>(
            Path.Combine(configuration.GetValue<string>(KnownSettingsKeys.ResourceStorePath)!,
                "colorPalettes", "riskLevel.json")) ?? [];

    /// <inheritdoc />
    public async Task<List<ValueColorResource<string>>> GetColorPaletteAssetReportTypeAsync() =>
        await ReadAsync<List<ValueColorResource<string>>>(
            Path.Combine(configuration.GetValue<string>(KnownSettingsKeys.ResourceStorePath)!,
                "colorPalettes", "assetReportType.json")) ?? [];

    /// <inheritdoc />
    public async Task<FilterBondsResource?> GetFilterBondsResourceAsync() =>
        await ReadAsync<FilterBondsResource>(
            Path.Combine(configuration.GetValue<string>(KnownSettingsKeys.ResourceStorePath)!,
            "filters", "filterBonds.json"));

    /// <inheritdoc />
    public async Task<List<EnableNameResource>> GetSendFilterAsync() =>
        await ReadAsync<List<EnableNameResource>>(
            Path.Combine(configuration.GetValue<string>(KnownSettingsKeys.ResourceStorePath)!,
                "filters", "filterSendMarketEvent.json")) ?? [];

    public async Task<TickerListResource> GetTickerListAsync(string tickerListName) =>
        await ReadAsync<TickerListResource>(
            Path.Combine(configuration.GetValue<string>(KnownSettingsKeys.ResourceStorePath)!,
                "tickerLists", $"{tickerListName}.json")) ?? new();

    private async Task<T?> ReadAsync<T>(string path)
    {
        if (!File.Exists(path))
            return default;

        await using var stream = File.OpenRead(path);
        return await JsonSerializer.DeserializeAsync<T>(stream);
    }
}