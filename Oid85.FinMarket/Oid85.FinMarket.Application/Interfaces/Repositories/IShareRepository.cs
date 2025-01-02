﻿using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface IShareRepository
{
    Task AddOrUpdateAsync(List<Share> shares);
    Task UpdateLastPricesAsync(Guid instrumentId, double lastPrice);
    Task<List<Share>> GetAllAsync();
    Task<List<Share>> GetWatchListAsync();
    Task<Share?> GetByTickerAsync(string ticker);
    Task<Share?> GetByInstrumentIdAsync(Guid instrumentId);
}