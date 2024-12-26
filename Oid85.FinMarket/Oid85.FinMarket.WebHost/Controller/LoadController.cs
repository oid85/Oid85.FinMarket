using Microsoft.AspNetCore.Mvc;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Application.Models.Responses;
using Oid85.FinMarket.WebHost.Controller.Base;

namespace Oid85.FinMarket.WebHost.Controller;

[Route("api")]
[ApiController]
public class LoadController(ILoadService loadService) : FinMarketBaseController
{
    /// <summary>
    /// Загрузить справочник акций
    /// </summary>
    [HttpGet("load-stocks")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadStocksAsync() =>
        GetResponseAsync<object, BaseResponse<object>>(
            loadService.LoadStocksAsync);

    /// <summary>
    /// Загрузить справочник фьючерсов
    /// </summary>
    [HttpGet("load-futures")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadFuturesAsync() =>
        GetResponseAsync<object, BaseResponse<object>>(
            loadService.LoadFuturesAsync);        
        
    /// <summary>
    /// Загрузить справочник облигаций
    /// </summary>
    [HttpGet("load-bonds")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadBondsAsync() =>
        GetResponseAsync<object, BaseResponse<object>>(
            loadService.LoadBondsAsync);
        
    /// <summary>
    /// Загрузить справочник индикативных инструментов
    /// </summary>
    [HttpGet("load-indicatives")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadIndicativesAsync() =>
        GetResponseAsync<object, BaseResponse<object>>(
            loadService.LoadIndicativesAsync);        
        
    /// <summary>
    /// Загрузить справочник валют
    /// </summary>
    [HttpGet("load-currencies")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadCurrenciesAsync() =>
        GetResponseAsync<object, BaseResponse<object>>(
            loadService.LoadCurrenciesAsync);         
        
    /// <summary>
    /// Подгрузить свечи по акциям
    /// </summary>
    [HttpGet("load-stock-daily-candles")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadStockDailyCandlesAsync() =>
        GetResponseAsync<object, BaseResponse<object>>(
            loadService.LoadStockDailyCandlesAsync);
    
    /// <summary>
    /// Подгрузить свечи по фьючерсам
    /// </summary>
    [HttpGet("load-future-daily-candles")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadFutureDailyCandlesAsync() =>
        GetResponseAsync<object, BaseResponse<object>>(
            loadService.LoadFutureDailyCandlesAsync);
    
    /// <summary>
    /// Загрузить данные о дивидендах
    /// </summary>
    [HttpGet("load-dividend-infos")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadDividendInfosAsync() =>
        GetResponseAsync<object, BaseResponse<object>>(
            loadService.LoadDividendInfosAsync);
        
    /// <summary>
    /// Загрузить данные о купонах
    /// </summary>
    [HttpGet("load-bond-coupons")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadBondCouponsAsync() =>
        GetResponseAsync<object, BaseResponse<object>>(
            loadService.LoadBondCouponsAsync);   
        
    /// <summary>
    /// Загрузить фундаментальные данные
    /// </summary>
    [HttpGet("load-asset-fundamentals")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadAssetFundamentalsAsync() =>
        GetResponseAsync<object, BaseResponse<object>>(
            loadService.LoadAssetFundamentalsAsync);    
    
    /// <summary>
    /// Загрузить последние цены акций
    /// </summary>
    [HttpGet("load-stock-prices")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadStockPricesAsync() =>
        GetResponseAsync<object, BaseResponse<object>>(
            loadService.LoadStockPricesAsync);
        
    /// <summary>
    /// Загрузить последние цены фьючерсов
    /// </summary>
    [HttpGet("load-future-prices")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadFuturePricesAsync() =>
        GetResponseAsync<object, BaseResponse<object>>(
            loadService.LoadFuturePricesAsync);
        
    /// <summary>
    /// Загрузить последние цены облигаций
    /// </summary>
    [HttpGet("load-bond-prices")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadBondPricesAsync() =>
        GetResponseAsync<object, BaseResponse<object>>(
            loadService.LoadBondPricesAsync);
        
    /// <summary>
    /// Загрузить последние цены индикативных инструментов
    /// </summary>
    [HttpGet("load-indicative-prices")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadIndicativePricesAsync() =>
        GetResponseAsync<object, BaseResponse<object>>(
            loadService.LoadIndicativePricesAsync);
        
    /// <summary>
    /// Загрузить последние цены валют
    /// </summary>
    [HttpGet("load-curency-prices")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadCurrencyPricesAsync() =>
        GetResponseAsync<object, BaseResponse<object>>(
            loadService.LoadCurrencyPricesAsync);
}