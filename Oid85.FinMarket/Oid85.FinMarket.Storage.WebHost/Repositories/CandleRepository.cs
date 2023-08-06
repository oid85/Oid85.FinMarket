﻿using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Configuration.Common;
using Oid85.FinMarket.DAL;
using Oid85.FinMarket.DAL.Entities;
using Oid85.FinMarket.Models;

namespace Oid85.FinMarket.Storage.WebHost.Repositories;

public class CandleRepository
{
    private readonly IServiceScopeFactory _scopeFactory;

    public CandleRepository(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task<Candle?> GetLastCandleAsync(Asset asset, string table)
    {
        if (table == "_1M")
        {
            var candle = await GetLast_1M_CandleAsync(asset);
            return candle;
        }

        if (table == "_1H")
        {
            var candle = await GetLast_1H_CandleAsync(asset);
            return candle;
        }

        if (table == "_1D")
        {
            var candle = await GetLast_1D_CandleAsync(asset);
            return candle;
        }

        return null;
    }

    public async Task SaveCandlesAsync(List<Candle> candles, string table)
    {
        if (table == TableNames.M1) 
            await Save_1M_CandlesAsync(candles);

        if (table == TableNames.H) 
            await Save_1H_CandlesAsync(candles);

        if (table == TableNames.D) 
            await Save_1D_CandlesAsync(candles);
    }

    private async Task<Candle?> GetLast_1M_CandleAsync(Asset asset)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<StorageDataBaseContext>();
            
            var candleEntity = await context._1M_CandleEntities
                .Where(entity => entity.Ticker == asset.Ticker)   
                .OrderByDescending(entity => entity.DateTime)
                .FirstOrDefaultAsync();
        
            if (candleEntity == null)
                return null;

            var candle = new Candle()
            {
                Id = candleEntity.Id,
                DateTime = candleEntity.DateTime,
                Open = candleEntity.Open,
                Close = candleEntity.Close,
                High = candleEntity.High,
                Low = candleEntity.Low,
                Volume = candleEntity.Volume,
                Ticker = candleEntity.Ticker
            };
        
            return candle;
        }
    }
    
    private async Task<Candle?> GetLast_1H_CandleAsync(Asset asset)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<StorageDataBaseContext>();

            var candleEntity = await context._1H_CandleEntities
                .Where(entity => entity.Ticker == asset.Ticker)
                .OrderByDescending(entity => entity.DateTime)
                .FirstOrDefaultAsync();

            if (candleEntity == null)
                return null;

            var candle = new Candle()
            {
                Id = candleEntity.Id,
                DateTime = candleEntity.DateTime,
                Open = candleEntity.Open,
                Close = candleEntity.Close,
                High = candleEntity.High,
                Low = candleEntity.Low,
                Volume = candleEntity.Volume,
                Ticker = candleEntity.Ticker
            };

            return candle;
        }
    }
    
    private async Task<Candle?> GetLast_1D_CandleAsync(Asset asset)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<StorageDataBaseContext>();

            var candleEntity = await context._1D_CandleEntities
                .Where(entity => entity.Ticker == asset.Ticker)
                .OrderByDescending(entity => entity.DateTime)
                .FirstOrDefaultAsync();

            if (candleEntity == null)
                return null;

            var candle = new Candle()
            {
                Id = candleEntity.Id,
                DateTime = candleEntity.DateTime,
                Open = candleEntity.Open,
                Close = candleEntity.Close,
                High = candleEntity.High,
                Low = candleEntity.Low,
                Volume = candleEntity.Volume,
                Ticker = candleEntity.Ticker
            };

            return candle;
        }
    }
    
    private async Task Save_1M_CandlesAsync(List<Candle> candles)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<StorageDataBaseContext>();

            var candlesEntities = new List<_1M_CandleEntity>();

            for (int i = 0; i < candles.Count; i++)
            {
                var candlesEntity = new _1M_CandleEntity();

                candlesEntity.Open = candles[i].Open;
                candlesEntity.Close = candles[i].Close;
                candlesEntity.High = candles[i].High;
                candlesEntity.Low = candles[i].Low;
                candlesEntity.Volume = candles[i].Volume;
                candlesEntity.DateTime = candles[i].DateTime;
                candlesEntity.Ticker = candles[i].Ticker;

                candlesEntities.Add(candlesEntity);
            }

            await context._1M_CandleEntities.AddRangeAsync(candlesEntities);
            await context.SaveChangesAsync();
        }
    }
    
    private async Task Save_1H_CandlesAsync(List<Candle> candles)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<StorageDataBaseContext>();

            var candlesEntities = new List<_1H_CandleEntity>();

            for (int i = 0; i < candles.Count; i++)
            {
                var candlesEntity = new _1H_CandleEntity();

                candlesEntity.Open = candles[i].Open;
                candlesEntity.Close = candles[i].Close;
                candlesEntity.High = candles[i].High;
                candlesEntity.Low = candles[i].Low;
                candlesEntity.Volume = candles[i].Volume;
                candlesEntity.DateTime = candles[i].DateTime;
                candlesEntity.Ticker = candles[i].Ticker;

                candlesEntities.Add(candlesEntity);
            }

            await context._1H_CandleEntities.AddRangeAsync(candlesEntities);
            await context.SaveChangesAsync();
        }
    }
    
    private async Task Save_1D_CandlesAsync(List<Candle> candles)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<StorageDataBaseContext>();

            var candlesEntities = new List<_1D_CandleEntity>();

            for (int i = 0; i < candles.Count; i++)
            {
                var candlesEntity = new _1D_CandleEntity();

                candlesEntity.Open = candles[i].Open;
                candlesEntity.Close = candles[i].Close;
                candlesEntity.High = candles[i].High;
                candlesEntity.Low = candles[i].Low;
                candlesEntity.Volume = candles[i].Volume;
                candlesEntity.DateTime = candles[i].DateTime;
                candlesEntity.Ticker = candles[i].Ticker;

                candlesEntities.Add(candlesEntity);
            }

            await context._1D_CandleEntities.AddRangeAsync(candlesEntities);
            await context.SaveChangesAsync();
        }
    }
}