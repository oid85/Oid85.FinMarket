namespace Oid85.FinMarket.Application.Models.Responses
{
    public class ResponseError
    {
        public string ErrorDescription { get; set; } = string.Empty;
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
