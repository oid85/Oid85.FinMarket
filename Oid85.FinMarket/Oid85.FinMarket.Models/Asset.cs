﻿namespace Oid85.FinMarket.Models;

public class Asset
{
    public Guid Id { get; set; }
    
    public string Ticker { get; set; }
    
    public string Name { get; set; }   
    
    public string Figi { get; set; }    
}