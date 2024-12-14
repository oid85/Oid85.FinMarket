namespace Oid85.FinMarket.Application.Models.Reports
{
    public class ReportData
    {
        public string Title { get; set; } = string.Empty;
        public List<string> Header { get; set; } = [];
        public List<List<string>> Data { get; set; } = [];
    }
}
