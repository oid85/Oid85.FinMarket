using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oid85.FinMarket.Models;

public class Candle
{
    public long Id { get; set; }
    
    public DateTime DateTime { get; set; }

    public decimal Open { get; set; }

    public decimal Close { get; set; }

    public decimal High { get; set; }

    public decimal Low { get; set; }

    public decimal Volume { get; set; }
}