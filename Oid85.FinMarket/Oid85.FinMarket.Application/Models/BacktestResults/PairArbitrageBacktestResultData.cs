using Oid85.FinMarket.Application.Models.Diagrams;
using Oid85.FinMarket.Application.Models.Reports;

namespace Oid85.FinMarket.Application.Models.BacktestResults;

public class PairArbitrageBacktestResultData
{
    public ReportData ReportData { get; set; } = new ();
    public PairArbitrageBacktestResultDiagramData DiagramData { get; set; } = new ();
}