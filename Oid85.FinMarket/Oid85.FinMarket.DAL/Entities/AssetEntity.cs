﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oid85.FinMarket.DAL.Entities;

[Table("assets", Schema = "public")]
public class AssetEntity
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    
    [Column("ticker")]
    public string Ticker { get; set; }
    
    [Column("figi")]
    public string Figi { get; set; }    
}