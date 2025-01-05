using Microsoft.Extensions.Configuration;
using Oid85.FinMarket.Application.Helpers;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services.ReportServices;
using Oid85.FinMarket.Application.Models.Reports;
using Oid85.FinMarket.Application.Models.Requests;

namespace Oid85.FinMarket.Application.Services.ReportServices;

/// <inheritdoc />
public class CurrenciesReportService(
    IConfiguration configuration,
    IAnalyseResultRepository analyseResultRepository,
    ICurrencyRepository currencyRepository,
    ReportHelper reportHelper) 
    : ICurrenciesReportService
{
    /// <inheritdoc />
    public Task<ReportData> GetAggregatedAnalyseAsync(GetAnalyseByTickerRequest request)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task<ReportData> GetSupertrendAnalyseAsync(GetAnalyseRequest request)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task<ReportData> GetCandleSequenceAnalyseAsync(GetAnalyseRequest request)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task<ReportData> GetRsiAnalyseAsync(GetAnalyseRequest request)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task<ReportData> GetYieldLtmAnalyseAsync(GetAnalyseRequest request)
    {
        throw new NotImplementedException();
    }
}