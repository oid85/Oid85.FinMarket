﻿using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface ISpreadRepository
{
    Task AddAsync(List<Spread> spreads);
    Task UpdateSpreadAsync(Spread spread);
    Task SetAsDeletedAsync(Spread spread);
    Task<List<Spread>> GetAllAsync();
    Task<Spread?> GetByTickerAsync(string firstInstrumentTicker);
}