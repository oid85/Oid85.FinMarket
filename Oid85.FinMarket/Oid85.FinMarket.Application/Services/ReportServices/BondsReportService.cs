using Microsoft.Extensions.Configuration;
using Oid85.FinMarket.Application.Helpers;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services.ReportServices;
using Oid85.FinMarket.Application.Models.Reports;
using Oid85.FinMarket.Application.Models.Requests;

namespace Oid85.FinMarket.Application.Services.ReportServices;

/// <inheritdoc />
public class BondsReportService(
    IConfiguration configuration,
    IAnalyseResultRepository analyseResultRepository,
    IBondRepository bondRepository,
    ReportHelper reportHelper) 
    : IBondsReportService
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
    public Task<ReportData> GetCandleVolumeAnalyseAsync(GetAnalyseRequest request)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task<ReportData> GetCouponAnalyseAsync()
    {
        throw new NotImplementedException();
    }
}