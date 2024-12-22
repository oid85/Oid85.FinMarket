namespace Oid85.FinMarket.Application.Models.Requests;

public class GetReportAnalyseRequest
{
    public DateTime From { get; set; } = DateTime.MinValue;
    public DateTime To { get; set; } = DateTime.MaxValue;
}