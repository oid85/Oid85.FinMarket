using Oid85.FinMarket.Application.Models.Results;

namespace Oid85.FinMarket.Application.Models.Responses
{
    public class GetReportAnalyseResponse : BaseResponse<ReporData>
    {
        public GetReportAnalyseResponse(ReporData result)
        {
            Result = result;
        }

        public GetReportAnalyseResponse(ResponseError error)
        {
            Error = error;
        }
    }
}
