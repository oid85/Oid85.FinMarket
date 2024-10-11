using Oid85.FinMarket.Application.Models.Results;

namespace Oid85.FinMarket.Application.Models.Responses
{
    public class GetReportAnalyseSupertrendResponse : BaseResponse<ReporData>
    {
        public GetReportAnalyseSupertrendResponse(ReporData result)
        {
            Result = result;
        }

        public GetReportAnalyseSupertrendResponse(ResponseError error)
        {
            Error = error;
        }
    }
}
