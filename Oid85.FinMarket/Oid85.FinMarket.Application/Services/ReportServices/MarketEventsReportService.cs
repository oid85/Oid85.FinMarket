﻿using Oid85.FinMarket.Application.Interfaces.Factories;
using Oid85.FinMarket.Application.Interfaces.Services.ReportServices;
using Oid85.FinMarket.Application.Models.Reports;

namespace Oid85.FinMarket.Application.Services.ReportServices;

/// <inheritdoc />
public class MarketEventsReportService(
    IReportDataFactory reportDataFactory) 
    : IMarketEventsReportService
{
    /// <inheritdoc />
    public async Task<ReportData> GetActiveMarketEventsAnalyseAsync() =>
        await reportDataFactory.CreateActiveMarketEventsReportDataAsync();
}