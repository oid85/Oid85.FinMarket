using Microsoft.AspNetCore.Mvc;
using Oid85.FinMarket.Models;
using Oid85.FinMarket.Storage.WebHost.Services;
using ILogger = NLog.ILogger;

namespace Oid85.FinMarket.Storage.WebHost.Controllers;

[Route("api")]
[ApiController]
public class AssetController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly DownloadCandlesService _downloadCandlesService;

    public AssetController(
        ILogger logger, 
        DownloadCandlesService downloadCandlesService)
    {
        _logger = logger;
        _downloadCandlesService = downloadCandlesService;
    }

    /// <summary>
    /// Загрузить дневные свечи
    /// </summary>
    [HttpGet("daily-candles")]
    public async Task GetDailyCandles()
    {
        try
        {
            await _downloadCandlesService._1D_DownloadCandles();
        }
            
        catch (Exception exception)
        {
            _logger.Error(exception);
        }
    }
}