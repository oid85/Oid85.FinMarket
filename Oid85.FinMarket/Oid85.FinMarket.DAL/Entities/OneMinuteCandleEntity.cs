using System.ComponentModel.DataAnnotations.Schema;

namespace Oid85.FinMarket.DAL.Entities;

[Table("one_minute_candles", Schema = "public")]
public class OneMinuteCandleEntity : BaseCandleEntity
{

}