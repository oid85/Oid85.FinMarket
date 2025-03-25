﻿namespace Oid85.FinMarket.Application.Models.Diagrams;

public class SimpleDiagramData
{
    public string Title { get; set; } = string.Empty;
    public List<SimpleDataPointSeries> Data { get; set; } = new();
}