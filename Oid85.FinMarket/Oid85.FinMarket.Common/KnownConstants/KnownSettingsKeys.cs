namespace Oid85.FinMarket.Common.KnownConstants
{
    public static class KnownSettingsKeys
    {
        public const string PostgresConnectionString = "Postgres:ConnectionString";
        public const string TinkoffToken = "Tinkoff:Token";
        public const string ApplicationSettingsBuffer = "ApplicationSettings:Buffer";
        public const string ApplicationSettingsLoadDailyCandlesOnStart = "ApplicationSettings:LoadDailyCandlesOnStart";
        public const string HangfireLoadDailyCandlesEnable = "Hangfire:LoadDailyCandles:Enable";
        public const string HangfireLoadDailyCandlesCron = "Hangfire:LoadDailyCandles:Cron";
    }
}
