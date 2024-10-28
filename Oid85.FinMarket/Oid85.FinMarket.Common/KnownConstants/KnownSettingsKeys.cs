namespace Oid85.FinMarket.Common.KnownConstants
{
    public class KnownSettingsKeys
    {
        public static string Postgres_ConnectionString = "Postgres:ConnectionString";
        public static string Tinkoff_Token = "Tinkoff:Token";
        public static string ApplicationSettings_Buffer = "ApplicationSettings:Buffer";
        public static string ApplicationSettings_LoadDailyCandlesOnStart = "ApplicationSettings:LoadDailyCandlesOnStart";
        public static string Quartz_DowloadDaily_Cron = "Quartz:DowloadDaily:Cron";
        public static string Quartz_DowloadDaily_Enable = "Quartz:DowloadDaily:Enable";
        public static string Quartz_DowloadHourly_Cron = "Quartz:DowloadHourly:Cron";
        public static string Quartz_DowloadHourly_Enable = "Quartz:DowloadHourly:Enable";
    }
}
