namespace Oid85.FinMarket.Domain.Models
{
    /// <summary>
    /// Объект результата анализа
    /// </summary>
    public class AnalyseResult
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Дата
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Тикер
        /// </summary>
        public string Ticker { get; set; } = string.Empty;

        /// <summary>
        /// Таймфрейм
        /// </summary>
        public string Timeframe { get; set; } = string.Empty;

        /// <summary>
        /// Результат анализа
        /// </summary>
        public string Result { get; set; } = string.Empty;
    }
}
