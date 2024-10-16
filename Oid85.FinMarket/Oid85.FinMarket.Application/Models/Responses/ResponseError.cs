namespace Oid85.FinMarket.Application.Models.Responses
{
    public class ResponseError
    {
        public string Description { get; set; } = string.Empty;
        public int Code { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
