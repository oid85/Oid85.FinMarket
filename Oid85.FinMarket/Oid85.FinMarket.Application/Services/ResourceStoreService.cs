using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Common.KnownConstants;

namespace Oid85.FinMarket.Application.Services;

/// <inheritdoc />
public class ResourceStoreService(
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
    
    private async Task<T?> ReadAsync<T>(string path)
    {
        await using var stream = File.OpenRead(path);
        return await JsonSerializer.DeserializeAsync<T>(stream);
    }
}