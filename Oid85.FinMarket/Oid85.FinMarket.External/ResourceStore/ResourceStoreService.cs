﻿using System.Text.Json;
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
    public async Task<MultiplicatorResource> GetMultiplicatorLtmAsync(string ticker)
    {
        string path = Path.Combine(
            configuration.GetValue<string>(KnownSettingsKeys.ResourceStorePath)!,
            "multiplicators",
            "ltm",
            $"{ticker.ToLower()}.json");

        var result = await ReadAsync<MultiplicatorResource>(path);

        if (result is null)
            return new();
        
        return result;
    }

    private async Task<T?> ReadAsync<T>(string path)
    {
        try
        {
            await using var stream = File.OpenRead(path);
            return await JsonSerializer.DeserializeAsync<T>(stream);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}