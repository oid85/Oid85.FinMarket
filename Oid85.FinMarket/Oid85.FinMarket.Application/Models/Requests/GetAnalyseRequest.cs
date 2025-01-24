namespace Oid85.FinMarket.Application.Models.Requests;

public class GetAnalyseRequest
{
    public DateOnly From { get; } = DateOnly.MinValue;
    public DateOnly To { get; } = DateOnly.MaxValue;
}