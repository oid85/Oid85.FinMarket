namespace Oid85.FinMarket.Application.Models.Reports;

public class ReportParameter(
    string displayType, string displayValue)
{
    public string DisplayType { get; set; } = displayType;
    public string DisplayValue { get; set; } = displayValue;
}