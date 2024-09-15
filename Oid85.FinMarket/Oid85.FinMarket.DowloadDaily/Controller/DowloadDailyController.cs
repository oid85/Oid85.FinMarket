﻿using Microsoft.AspNetCore.Mvc;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.External.Catalogs;
using Oid85.FinMarket.External.Storage;
using Oid85.FinMarket.External.Tinkoff;
using ILogger = NLog.ILogger;

namespace DaGroup.WPAnalyst.DataLake.WebHost.Controllers
{
    [Route("api")]
    [ApiController]
    public class DowloadDailyController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ITinkoffService _tinkoffService;
        private readonly ICatalogService _catalogService;
        private readonly IStorageService _storageService;

        public DowloadDailyController(
            ILogger logger,
            ITinkoffService tinkoffService,
            ICatalogService catalogService,
            IStorageService storageService)
        {
            _logger = logger;
            _tinkoffService = tinkoffService;
            _catalogService = catalogService;
            _storageService = storageService;
        }

        [HttpGet("load-daily-candles")]
        public async Task LoadDailyCandlesAsync()
        {
            _logger.Trace($"Request - /api/load-daily-candles");
            
            try
            {
                var instrument = new FinancicalInstrument() 
                { 
                    Ticker = "SBER",
                    Figi = "BBG004730N88"
                };

                var candles = await _tinkoffService.GetCandlesAsync(instrument, KnownTimeframes.Daily);
            }
            
            catch (Exception exception)
            {
                _logger.Error(exception);
            }
        }

        [HttpGet("load-stocks-catalog")]
        public async Task LoadStocksCatalogAsync()
        {
            _logger.Trace($"Request - /api/load-stocks-catalog");

            try
            {
                var stocks = _tinkoffService.GetStocks();

                await _catalogService.LoadFinancicalInstrumentsAsync(
                    KnownFinancicalInstrumentTypes.Stocks, stocks);
            }

            catch (Exception exception)
            {
                _logger.Error(exception);
            }
        }

        [HttpGet("load-bonds-catalog")]
        public async Task LoadBondsCatalogAsync()
        {
            _logger.Trace($"Request - /api/load-bonds-catalog");

            try
            {
                var bonds = _tinkoffService.GetBonds();

                await _catalogService.LoadFinancicalInstrumentsAsync(
                    KnownFinancicalInstrumentTypes.Bonds, bonds);
            }

            catch (Exception exception)
            {
                _logger.Error(exception);
            }
        }

        [HttpGet("load-futures-catalog")]
        public async Task LoadFuturesCatalogAsync()
        {
            _logger.Trace($"Request - /api/load-futures-catalog");

            try
            {
                var futures = _tinkoffService.GetFutures();

                await _catalogService.LoadFinancicalInstrumentsAsync(
                    KnownFinancicalInstrumentTypes.Futures, futures);
            }

            catch (Exception exception)
            {
                _logger.Error(exception);
            }
        }

        [HttpGet("load-currencies-catalog")]
        public async Task LoadCurrenciesCatalogAsync()
        {
            _logger.Trace($"Request - /api/load-currencies-catalog");

            try
            {
                var currencies = _tinkoffService.GetCurrencies();

                await _catalogService.LoadFinancicalInstrumentsAsync(
                    KnownFinancicalInstrumentTypes.Currencies, currencies);
            }

            catch (Exception exception)
            {
                _logger.Error(exception);
            }
        }

        [HttpGet("save_daily-stocks")]
        public async Task SaveDailyStocksAsync()
        {
            _logger.Trace($"Request - /api/save_daily-stocks");

            try
            {
                var stocks = await _catalogService
                    .GetActiveFinancicalInstrumentsAsync(KnownFinancicalInstrumentTypes.Stocks);

                foreach (var stock in stocks)
                {
                    var candles = await _tinkoffService.GetCandlesAsync(stock, KnownTimeframes.Daily);
                    int result = await _storageService.SaveCandlesAsync(
                        $"{stock.Ticker}_{KnownTimeframes.Daily}", candles);
                }
            }

            catch (Exception exception)
            {
                _logger.Error(exception);
            }
        }
    }
}
