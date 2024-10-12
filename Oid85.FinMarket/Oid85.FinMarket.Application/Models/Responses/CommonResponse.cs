using Oid85.FinMarket.Application.Models.Results;

namespace Oid85.FinMarket.Application.Models.Responses
{
    public class CommonResponse<TResult> : BaseResponse<TResult>
    {
        public CommonResponse(TResult result)
        {
            Result = result;
        }

        public CommonResponse(ResponseError error)
        {
            Error = error;
        }
    }
}
