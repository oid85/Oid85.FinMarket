﻿namespace Oid85.FinMarket.Domain.Models
{
    /// <summary>
    /// Свеча
    /// </summary>
    public class Candle
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Цена открытия
        /// </summary>
        public double Open { get; set; }

        /// <summary>
        /// Цена закрытия
        /// </summary>
        public double Close { get; set; }

        /// <summary>
        /// Макс. цена
        /// </summary>
        public double High { get; set; }

        /// <summary>
        /// Мин. цена
        /// </summary>
        public double Low { get; set; }

        /// <summary>
        /// Объем
        /// </summary>
        public long Volume { get; set; }

        /// <summary>
        /// Дата
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Свеча сформирована
        /// </summary>
        public int IsComplete { get; set; }
    }
}
