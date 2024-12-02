using System.ComponentModel.DataAnnotations.Schema;

namespace Oid85.FinMarket.Logging.DataAccess.Entities;

public class LogRecordEntity
{
    [Column("id")]
    public Guid Id { get; set; }
    
    /// <summary>
    /// Время сообщения
    /// </summary>
    [Column("date", TypeName = "timestamp with time zone")]
    public DateTime Date { get; set; }
    
    /// <summary>
    /// Уровень логирования
    /// </summary>
    [Column("level")]
    public string Level { get; set; } = string.Empty;

    /// <summary>
    /// Сообщение
    /// </summary>
    [Column("message")]
    public string Message { get; set; } = string.Empty;
    
    /// <summary>
    /// Параметры
    /// </summary>
    [Column("parameters")]
    public string Parameters { get; set; } = string.Empty;
}