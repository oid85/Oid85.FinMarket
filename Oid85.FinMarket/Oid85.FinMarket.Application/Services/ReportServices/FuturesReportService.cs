using Microsoft.Extensions.Configuration;
using Oid85.FinMarket.Application.Helpers;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services.ReportServices;
using Oid85.FinMarket.Application.Models.Reports;
using Oid85.FinMarket.Application.Models.Requests;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Services.ReportServices;

/// <inheritdoc />
public class FuturesReportService(
    IConfiguration configuration,
    IAnalyseResultRepository analyseResultRepository,
    IFutureRepository futureRepository,
    ISpreadRepository spreadRepository,
    ReportHelper reportHelper)
    : IFuturesReportService
{
    public Task<ReportData> GetAggregatedAnalyseAsync(GetAnalyseByTickerRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<ReportData> GetSupertrendAnalyseAsync(GetAnalyseRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<ReportData> GetCandleSequenceAnalyseAsync(GetAnalyseRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<ReportData> GetCandleVolumeAnalyseAsync(GetAnalyseRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<ReportData> GetRsiAnalyseAsync(GetAnalyseRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<ReportData> GetYieldLtmAnalyseAsync(GetAnalyseRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<ReportData> GetSpreadAnalyseAsync(GetAnalyseRequest request)
    {
        throw new NotImplementedException();
    }
}