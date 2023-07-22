using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oid85.FinMarket.DAL.Entities;

[Table("candles_one_day", Schema = "public")]
public class CandleOneDayEntity : BaseCandleEntity
{

}