﻿using System.Text.Json.Serialization;

namespace Oid85.FinMarket.External.ResourceStore.Models;

public class RiskLevelsResource
{
    [JsonPropertyName("low")]
    public bool Low { get; set; }
    
    [JsonPropertyName("middle")]
    public bool Middle { get; set; }
    
    [JsonPropertyName("high")]
    public bool High { get; set; }
    
    [JsonPropertyName("veryHigh")]
    public bool VeryHigh { get; set; }
}