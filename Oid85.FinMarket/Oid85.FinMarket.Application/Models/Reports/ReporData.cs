﻿namespace Oid85.FinMarket.Application.Models.Results
{
    public class ReporData
    {
        public string Title { get; set; } = string.Empty;
        public List<string> Header { get; set; } = [];
        public List<List<string>> Data { get; set; } = [];
    }
}