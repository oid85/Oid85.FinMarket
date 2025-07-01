using Oid85.FinMarket.Application.Models.Diagrams;
using Oid85.FinMarket.Application.Models.Reports;

namespace Oid85.FinMarket.Application.Models.BacktestResults;

public class BacktestResultData
{
    public ReportData Report { get; set; } = new ();
    public BacktestResultDiagramData Diagram { get; set; } = new ();
}