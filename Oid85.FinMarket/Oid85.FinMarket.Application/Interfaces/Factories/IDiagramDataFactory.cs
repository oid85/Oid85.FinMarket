﻿using Oid85.FinMarket.Application.Models.Diagrams;

namespace Oid85.FinMarket.Application.Interfaces.Factories;

public interface IDiagramDataFactory
{
    Task<SimpleDiagramData> CreateDailyClosePricesDiagramDataAsync(
        List<Guid> instrumentIds, DateOnly from, DateOnly to);

    Task<BubbleDiagramData> CreateMultiplicatorsMCapPeNetDebtEbitdaAsync(
        List<Guid> instrumentIds);
}