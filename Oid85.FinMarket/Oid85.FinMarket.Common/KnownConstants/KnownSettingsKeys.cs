namespace Oid85.FinMarket.Common.KnownConstants
{
    public class KnownSettingsKeys
    {
        public static readonly string PostgresConnectionString = "Postgres:ConnectionString";
        public static readonly string TinkoffToken = "Tinkoff:Token";
        public static readonly string ApplicationSettings_Buffer = "ApplicationSettings:Buffer";
        public static readonly string ApplicationSettings_LoadDailyCandlesOnStart = "ApplicationSettings:LoadDailyCandlesOnStart";
        public static readonly string Hangfire_LoadDailyCandles_Enable = "Hangfire:LoadDailyCandles:Enable";
        public static readonly string Hangfire_LoadDailyCandles_Cron = "Hangfire:LoadDailyCandles:Cron";
    }
}
