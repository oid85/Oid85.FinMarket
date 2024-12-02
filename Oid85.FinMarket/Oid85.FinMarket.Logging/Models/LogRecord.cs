namespace Oid85.FinMarket.Logging.Models;

public class LogRecord
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Время сообщения
    /// </summary>
    public DateTime Date { get; set; }
    
    /// <summary>
    /// Уровень логирования
    /// </summary>
    public string Level { get; set; } = string.Empty;

    /// <summary>
    /// Сообщение
    /// </summary>
    public string Message { get; set; } = string.Empty;
}