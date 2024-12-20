namespace Oid85.FinMarket.Common.KnownConstants
{
    public static class KnownSettingsKeys
    {
        public const string PostgresFinMarketConnectionString = "Postgres:FinMarketConnectionString";
        public const string PostgresHangfireConnectionString = "Postgres:HangfireConnectionString";
        
        public const string TinkoffToken = "Tinkoff:Token";
        
        public const string ApplicationSettingsBuffer = "ApplicationSettings:Buffer";
        
        public const string HangfireLoadInstrumentsJobId = "Hangfire:LoadInstruments:JobId";
        public const string HangfireLoadInstrumentsEnable = "Hangfire:LoadInstruments:Enable";
        public const string HangfireLoadInstrumentsCron = "Hangfire:LoadInstruments:Cron";
        
        public const string HangfireLoadPricesJobId = "Hangfire:LoadPrices:JobId";
        public const string HangfireLoadPricesEnable = "Hangfire:LoadPrices:Enable";
        public const string HangfireLoadPricesCron = "Hangfire:LoadPrices:Cron";
    }
}
