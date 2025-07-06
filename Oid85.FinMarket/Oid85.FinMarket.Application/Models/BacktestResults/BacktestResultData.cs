using Oid85.FinMarket.Application.Models.Diagrams;
using Oid85.FinMarket.Application.Models.Reports;

namespace Oid85.FinMarket.Application.Models.BacktestResults;

public class BacktestResultData
{
    public ReportData ReportData { get; set; } = new ();
    public BacktestResultDiagramData DiagramData { get; set; } = new ();
}