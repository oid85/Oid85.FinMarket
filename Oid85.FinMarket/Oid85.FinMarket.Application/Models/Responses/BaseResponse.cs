namespace Oid85.FinMarket.Application.Models.Responses
{
    public class BaseResponse<TResponseResult>
    {
        public Guid TraceId { get; set; } = Guid.NewGuid();
        public TResponseResult? Result { get; set; }
        public ResponseError? Error { get; set; }
        public DateTime MessageDate { get; set; } = DateTime.UtcNow;
    }
}
