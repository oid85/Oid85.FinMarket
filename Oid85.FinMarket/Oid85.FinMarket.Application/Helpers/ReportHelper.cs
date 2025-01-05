using Oid85.FinMarket.Application.Models.Reports;
using Oid85.FinMarket.Common.KnownConstants;

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
}