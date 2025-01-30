using Oid85.FinMarket.Application.Models.Reports;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Helpers;

public class ReportHelper
{
    public List<ReportParameter> GetDates(
        DateOnly from, DateOnly to)
    {
        var curDate = from;
        var dates = new List<ReportParameter>();

        while (curDate <= to)
        {
            dates.Add(new ReportParameter(
                KnownDisplayTypes.Date,
                curDate.ToString(KnownDateTimeFormats.DateISO)));

            curDate = curDate.AddDays(1);
        }

        return dates;
    }
    
    public string GetColor(string analyseType, AnalyseResult analyseResult)
    {
        return KnownColors.White;
    }
}