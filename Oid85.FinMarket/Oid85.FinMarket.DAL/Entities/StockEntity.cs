﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oid85.FinMarket.DAL.Entities;

[Table("stocks", Schema = "public")]
public class StockEntity
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    
    [Column("ticker")]
    public string Ticker { get; set; }
    
    [Column("name")]
    public string Name { get; set; }    
    
    [Column("figi")]
    public string Figi { get; set; }

    [Column("in_watch_list")]
    public bool InWatchList { get; set; }
}