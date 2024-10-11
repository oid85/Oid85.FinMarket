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
        public long Id { get; set; }

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
        /// Направление тренда
        /// </summary>
        public string TrendDirection { get; set; } = string.Empty;

        /// <summary>
        /// Дополнительные данные
        /// </summary>
        public string Data { get; set; } = string.Empty;
    }
}
