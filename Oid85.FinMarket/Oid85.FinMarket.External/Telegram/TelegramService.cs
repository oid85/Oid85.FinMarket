namespace Oid85.FinMarket.External.Telegram;

/// <inheritdoc />
public class TelegramService : ITelegramService
{
    /// <inheritdoc />
    public Task SendMessageAsync(string message)
    {
        return Task.CompletedTask;
    }
}