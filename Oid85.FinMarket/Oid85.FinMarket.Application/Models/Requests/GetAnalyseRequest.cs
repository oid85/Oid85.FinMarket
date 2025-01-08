namespace Oid85.FinMarket.Application.Models.Requests;

public class GetAnalyseRequest
{
    public DateOnly From { get; set; } = DateOnly.MinValue;
    public DateOnly To { get; set; } = DateOnly.MaxValue;
}