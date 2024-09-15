﻿using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.External.Storage
{
    /// <summary>
    /// Сервис работы с хранилищем свечей
    /// </summary>
    public interface IStorageService
    {
        /// <summary>
        /// Добавить свечи в хранилище
        /// </summary>
        /// <param name="tableName">Имя таблицы</param>
        /// <param name="candles">Свечи фин. инструмента</param>
        /// <returns></returns>
        public Task<int> SaveCandlesAsync(string tableName, IList<Candle> candles);

        /// <summary>
        /// Получить свечи из хранилища
        /// </summary>
        /// <param name="tableName">Имя таблицы</param>
        /// <param name="count">Кол-во последних свечей</param>
        /// <returns></returns>
        public Task<List<Candle>> GetCandlesAsync(string tableName, int count);
    }
}