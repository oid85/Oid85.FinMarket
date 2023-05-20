using System.ComponentModel.DataAnnotations.Schema;

namespace Oid85.FinMarket.DAL.Entities;

[Table("one_day_candles", Schema = "public")]
public class OneDayCandleEntity : BaseCandleEntity
{

}