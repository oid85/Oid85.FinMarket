namespace Oid85.FinMarket.Common.KnownConstants;

public static class KnownSettingsKeys
{
    public const string PostgresFinMarketConnectionString = "Postgres:FinMarketConnectionString";
    public const string PostgresApplyMigrationsOnStart = "Postgres:ApplyMigrationsOnStart";
    public const string TinkoffToken = "Tinkoff:Token";
    public const string TelegramChatId = "Telegram:ChatId";
    public const string TelegramToken = "Telegram:Token";
    public const string ResourceStorePath = "ResourceStore:Path";
    public const string DeployPort = "DeployPort";
    public const string ApplicationSettingsOutputWindowInDays = "ApplicationSettings:OutputWindowInDays";
}