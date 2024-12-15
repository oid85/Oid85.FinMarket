namespace Oid85.FinMarket.Application.Models.Reports;

public class ReportParameter(
    string type, string value)
{
    public string Type { get; set; } = type;
    public string Value { get; set; } = value;
}