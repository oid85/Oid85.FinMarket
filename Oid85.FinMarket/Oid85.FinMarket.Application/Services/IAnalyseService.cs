﻿using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Services
{
    /// <summary>
    /// Сервис анализа
    /// </summary>
    public interface IAnalyseService
    {
        /// <summary>
        /// Анализ с индикатором Супертренд
        /// </summary>
        public Task<List<AnalyseResult>> SupertrendAnalyseAsync(
            FinInstrument stock, string timeframe);

        /// <summary>
        /// Анализ последовательности подряд идущих свечей
        /// </summary>
        public Task<List<AnalyseResult>> CandleSequenceAnalyseAsync(
            FinInstrument stock, string timeframe);
    }
}
