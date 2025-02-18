using Oid85.FinMarket.Common.KnownConstants;

namespace Oid85.FinMarket.Application.Models.Reports;

public class ReportParameter(
    string type, string value, string colorRgb = KnownColors.White)
{
    public string Type { get; set; } = type;
    public string Value { get; set; } = value;
    public string ColorRgb { get; set; } = colorRgb;
}