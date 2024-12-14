namespace Oid85.FinMarket.Application.Models.Reports;

public class ReportData
{
    public string Title { get; set; } = string.Empty;
    public List<ReportParameter> Header { get; set; } = [];
    public List<List<ReportParameter>> Data { get; set; } = [];
}