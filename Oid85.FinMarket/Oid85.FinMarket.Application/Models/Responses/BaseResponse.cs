namespace Oid85.FinMarket.Application.Models.Responses
{
    public class BaseResponse<TResult>
    {
        public Guid TraceId { get; set; } = Guid.NewGuid();
        public TResult? Result { get; set; }
        public ResponseError? Error { get; set; }
        public DateTime MessageDate { get; set; } = DateTime.UtcNow;
    }
}
